﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff.Models
{
    public class OPTReport
    {
        public List<OPTRow> data;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string title { get; set; }

        public OPTReport(GraphPeriod timeframe, DateTime dateProvided)
        {
            switch (timeframe)
            {
                case GraphPeriod.daily:
                    startDate = dateProvided;
                    endDate = startDate.AddDays(1).AddSeconds(-1);
                    title = string.Format("OPT Data for {0} ", startDate.ToShortDateString());
                    break;
                case GraphPeriod.week:
                    startDate = dateProvided.StartOfWeek(DayOfWeek.Monday);
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    title = string.Format("OPT Data between {0} and {1}", startDate.ToShortDateString(), endDate.ToShortDateString());
                    break;
                case GraphPeriod.month:
                    startDate = dateProvided.StartOfMonth();
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    title = string.Format("OPT Data for {0}", startDate.ToString("MMMM yyyy"));
                    break;
                case GraphPeriod.year:
                    startDate = dateProvided.StartOfYear();
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    title = string.Format("OPT Data for {0}", startDate.ToString("yyyy"));
                    break;
                default:
                    break;
            }

            data = new List<OPTRow>();
            data.Add(new OPTRow("CA", startDate, endDate));
            foreach (var div in myDBContextHelper.CurrentContext.Divisions)
            {
                data.Add(new OPTRow(div.Detail, startDate, endDate));
            }
        }
        public string Totals
        {
            get
            {
                int issued = 0;
                int executed = 0;
                int suspendedDischarged = 0;
                int expired = 0;
                int pending = 0;
                int toPrison = 0;

                foreach (var row in data)
                {
                    issued += row.issued;
                    executed += row.executed;
                    suspendedDischarged += row.suspendedDischarged;
                    expired += row.expired;
                    pending += row.pending;
                    toPrison += row.toPrison;

                }
                return string.Format("<tr><th>Totals</th><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th></tr>",
                                                                    issued,
                                                                    executed,
                                                                    suspendedDischarged,
                                                                    expired,
                                                                    pending,
                                                                    toPrison);
            }
        }
        public string output
        {
            get
            {
                return string.Format("{0} rows of data stored", data.Count());
            }
        }
 
       public class OPTRow 
        {
            TipstaffDB db = myDBContextHelper.CurrentContext;
            public string division { get; set; }
            public int issued { get; set; }
            public int executed { get; set; }
            public int suspendedDischarged { get; set; }
            public int expired { get; set; }
            public int pending { get; set; }
            public int toPrison { get; set; }

            public OPTRow(string Division, DateTime start, DateTime end)
            {
                switch (Division)
                {
                    case "CA":
                        division = "Child Abduction";
                        var allCAs = db.ChildAbductions;
                        issued = allCAs.Where(x => x.createdOn >= start && x.createdOn <= end).Count();
                        pending = allCAs.Where(x=>x.createdOn<=end).Where(x => x.resultID == null || x.DateExecuted>end).Count();
                        var closedCAs = allCAs.Where(x => x.DateExecuted >= start && x.DateExecuted <= end);
                        System.Diagnostics.Debug.Print(allCAs.Count().ToString());
                        System.Diagnostics.Debug.Print(closedCAs.Count().ToString());
                        executed =  closedCAs.Where(x => x.result.Detail == "Executed").Count();
                        suspendedDischarged = closedCAs.Where(x => x.result.Detail == "Suspended" || x.result.Detail == "Discharged").Count();
                        expired = closedCAs.Where(x => x.result.Detail == "Expired").Count();
                        toPrison = closedCAs.Sum(x => x.prisonCount) ?? 0;
                        break;
                    default:
                        //get Warrant data
                        division = Division;
                        //var divisions = db.Warrants.Where(x => x.division.Detail==Division);
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            issued = rpt.Warrants.Where(x => x.division.Detail == division && x.createdOn >= start && x.createdOn <= end).Count();
                        };
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            pending = rpt.Warrants.Where(x => x.division.Detail == Division && x.createdOn <= end).Where(x => x.resultID == null || x.DateExecuted > end).Count();
                        };
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            executed = rpt.Warrants.Where(x => x.division.Detail == Division).Where(x => x.resultID != null && x.DateExecuted >= start && x.DateExecuted <= end && x.result.Detail == "Executed").Count();
                        };
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            suspendedDischarged =  rpt.Warrants.Where(x => x.division.Detail == Division).Where(x => x.resultID != null && x.DateExecuted >= start && x.DateExecuted <= end).Where(x => x.result.Detail == "Suspended" || x.result.Detail== "Discharged").Count();
                        };
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            expired = rpt.Warrants.Where(x => x.division.Detail == Division).Where(x => x.resultID != null && x.DateExecuted >= start && x.DateExecuted <= end && x.result.Detail == "Expired").Count();
                        };
                        using  (TipstaffDB rpt = new TipstaffDB()){
                            toPrison=rpt.Warrants.Where(x => x.division.Detail == Division).Where(x => x.resultID != null && x.DateExecuted >= start && x.DateExecuted <= end).Sum(x=>x.prisonCount)??0;
                        };
                        break;
                }
            }
        }
    }

    public class ClosedCases_StandardReport
    {
        [Required, Display(Name = "Start Date")]
        public DateTime? Start { get; set; }
        [Required, Display(Name = "End Date")]
        public DateTime? End { get; set; }
        public List<Tipstaff.Models.WReportItem> WItems { get; set; }
        public List<Tipstaff.Models.CAReportItem> CAItems { get; set; }
    }

    public class CAReportItem
    {
        public int tipstaffRecordID { get; set; }
        public string UniqueRecordID { get; set; }
        public DateTime? DateCirculated { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string NPO { get; set; }
        public ICollection<Child> children { get; set; }
        public ICollection<Respondent> respondents { get; set; }
    }

    public class WReportItem
    {
        public int tipstaffRecordID { get; set; }
        public string UniqueRecordID { get; set; }
        public DateTime? DateCirculated { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string NPO { get; set; }
        public ICollection<Respondent> respondents { get; set; }
    }

    public class WExcelReportItem
    {
        public string UniqueRecordID { get; set; }
        public DateTime? DateCirculated { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string NPO { get; set; }
        public string RespondentName {get;set;}
        public string RespondentDOB { get; set; }
        public string RespondentPNCID { get; set; }
    }

    public class CAExcelReportItem
    {
        public string UniqueRecordID { get; set; }
        public DateTime? DateCirculated { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string NPO { get; set; }
        public string ChildrenName { get; set; }
        public string ChildrenDOB { get; set; }
        public string ChildrenPNCID { get; set; }
        public string RespondentName { get; set; }
        public string RespondentDOB { get; set; }
        public string RespondentPNCID { get; set; }
    }
}
