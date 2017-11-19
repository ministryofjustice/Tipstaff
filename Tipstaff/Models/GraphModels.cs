using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Tipstaff.Models
{
    public enum GraphPeriod
    {
        daily,
        week,
        month,
        year
    }

    public class DeletionsForLastYear 
    {
        ////TipstaffDB db = myDBContextHelper.CurrentContext;
        private DateTime startDate { get; set; }

        private IDictionary<string, int?> gData = new Dictionary<string, int?>();
        private GraphPeriod graphPeriod { get; set; }

        private string prefix;
        private string displayDate;

        public DeletionsForLastYear()
        {
            //////////startDate = DateTime.Today.StartOfMonth().AddMonths(-11);

            //////////for (int i = 11; i >= 0; i--)
            //////////{
            //////////    DateTime dLoop = DateTime.Today.AddMonths(-i);
            //////////    int? data = db.AuditEvents.Where(a => a.EventDate.Month == dLoop.Month && a.EventDate.Year == dLoop.Year && (a.idAuditEventDescription == 8 || a.idAuditEventDescription == 12)).Count();
            //////////    if (data!=null && data == 0)
            //////////    {
            //////////        data = null;
            //////////    }
            //////////    gData.Add(dLoop.ToString("MMM yyyy"), data);
            //////////}
        }

        public ICollection<string> Keys
        {
            get { return gData.Keys; }
        }

        public ICollection<int?> Values
        {
            get
            {
                return gData.Values;
            }
        }

        public string Title
        {
            get
            {
                return string.Format("Records deleted during {0} {1}", prefix, displayDate);
            }
        }
        public string ShortTitle
        {
            get
            {
                return "Records deleted";
            }
        }
    }

    public class IssuesForLastYear :IGraphData
    {
        //////TipstaffDB db = myDBContextHelper.CurrentContext;
        private IDictionary<string, int?> gData = new Dictionary<string, int?>();
        private DateTime startDate;
        public IssuesForLastYear()
        {
            startDate = DateTime.Today.StartOfMonth().AddMonths(-11);

            for (int i = 11; i >= 0; i--)
            {
                DateTime dLoop = DateTime.Today.AddMonths(-i);
                /////int? data =  db.TipstaffRecord.Where(t => t.createdOn.Value.Month == dLoop.Month && t.createdOn.Value.Year==dLoop.Year).Count();
                int? data = 0;

                if (data != null && data == 0)
                {
                    //data = null;
                }
                gData.Add(dLoop.ToString("MMM yyyy"), data);

            }
        }
        public ICollection<int?> Values
        {
            get { return gData.Values; }
        }

        public ICollection<string> Keys
        {
            get { return gData.Keys; }
        }

        public string ShortTitle
        {
            get { return "Records issued"; }
        }

        public string Title
        {
            get { return string.Format("Records issued during {0}", startDate.ToString("mmm yyyy")); }
        }

    }

    public class GraphData 
    {
        TipstaffDB db = myDBContextHelper.CurrentContext;
        //public DateTime startDate { get; set; }

        //private IDictionary<string, int?> gData = new Dictionary<string, int?>();

        public  IDictionary<string, int?> gData { get; set; }


        ////public GraphPeriod graphPeriod { get; set; }

        public string prefix;

        public string displayDate;

        ////public GraphData(GraphPeriod graphPeriod)
        ////{
            
        ////    IEnumerable<Warrant> w = db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail);
        ////    switch (graphPeriod)
        ////    {
        ////        case GraphPeriod.week:
        ////            prefix = "w/c";
        ////            startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        ////            displayDate = startDate.ToString("d MMM yyyy");
        ////            break;
        ////        case GraphPeriod.month:
        ////            prefix = "";
        ////            startDate = DateTime.Today.StartOfMonth();
        ////            displayDate = startDate.ToString("MMMM yyyy");
        ////            break;
        ////        case GraphPeriod.year:
        ////            prefix = "";
        ////            startDate = Convert.ToDateTime(string.Format("1/1/{0}", DateTime.Now.Year.ToString()));
        ////            displayDate = startDate.ToString("yyyy");
        ////            break;
        ////        default:
        ////            break;
        ////    }

        ////    gData.Add("Child Abductions", db.ChildAbductions.Where(c => c.createdOn >= startDate).Count());
        ////    gData.Add("Bankruptcy", db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(d => d.division.Detail == "Bankruptcy").Count());
        ////    gData.Add("Chancery", db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(e => e.division.Detail == "Chancery").Count());
        ////    gData.Add("Family", db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Family").Count());
        ////    gData.Add("Insolvency", db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Insolvency").Count()); // null);//
        ////    gData.Add("Queen's Bench", db.Warrants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Queen's Bench").Count());

        ////    if (gData["Child Abductions"] == 0) gData["Child Abductions"] = null;
        ////    if (gData["Bankruptcy"] == 0) gData["Bankruptcy"] = null;
        ////    if (gData["Chancery"] == 0) gData["Chancery"] = null;
        ////    if (gData["Family"] == 0) gData["Family"] = null;
        ////    if (gData["Insolvency"] == 0) gData["Insolvency"] = null;
        ////    if (gData["Queen's Bench"] == 0) gData["Queen's Bench"] = null;
        ////}

        ////public IEnumerable<string> Keys
        ////{
        ////    get{return gData.Keys;}
        ////}

        ////public IEnumerable<int?> Values
        ////{
        ////    get
        ////    {
        ////        return gData.Values;
        ////    }
        ////}

        public string Title
        {
            get
            {
                return string.Format("Records issued during {0} {1}", prefix, displayDate);
            }
        }
        public string ShortTitle
        {
            get
            {
                return string.Format("{0} {1}", prefix, displayDate);
            }
        }
    }


}