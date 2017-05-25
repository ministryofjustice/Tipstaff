using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using System.Web.UI;
using Utils.Excel;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ReportsController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 5)]
        public PartialViewResult OPTReport(GraphPeriod period, DateTime chosenDate)
        {
            OPTReport rpt = new OPTReport(period, chosenDate);

            return PartialView("_OPTReport", rpt);

        }

        public ActionResult IssuesOverLast12Months()
        {
            var Chart1 = new Chart();
            Chart1.Width = 500;
            Chart1.Height = 350;
            Title title = new Title("Record issues and deletions during last 12 months");
            title.Font = new Font("Calibri", 12, FontStyle.Underline);
            Chart1.Titles.Add(title);
            var area = new ChartArea("ChartArea1");
            Chart1.ChartAreas.Add(area);
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.LabelStyle.Angle = -90;
            //build dataset
            IssuesForLastYear iData = new IssuesForLastYear();
            DeletionsForLastYear dData = new DeletionsForLastYear();
            //build series1
            Series iSeries = new Series(iData.ShortTitle);
            iSeries.Points.DataBindXY(iData.Keys, iData.Values);
            iSeries.ChartType = SeriesChartType.Line;
            iSeries.BorderWidth = 2;
            //build series2
            Series dSeries = new Series(dData.ShortTitle);
            dSeries.Points.DataBindXY(dData.Keys, dData.Values);
            dSeries.ChartType = SeriesChartType.Point;
            dSeries.EmptyPointStyle.IsValueShownAsLabel = false;
            dSeries.Color = Color.Red;
            //add datasets to graph
            Chart1.Series.Add(iSeries);
            Chart1.Series.Add(dSeries);
            //Configure legend
            Chart1.Legends.Add("Legend1");
            Chart1.Legends["Legend1"].Enabled = true;
            //show graph
            var returnStream = new MemoryStream();
            Chart1.ImageType = ChartImageType.Png;
            Chart1.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }

        public ActionResult OPTGraph()
        {

            GraphData wD = new GraphData(GraphPeriod.week);
            GraphData mD = new GraphData(GraphPeriod.month);
            GraphData yD = new GraphData(GraphPeriod.year);
            
            var Chart1 = new Chart();
            Chart1.Width = 500;
            Chart1.Height = 350;
            Title title = new Title("Combined Report for issues during...");
            title.Font = new Font("Calibri", 12, FontStyle.Underline);
            Chart1.Titles.Add(title);
            var area = new ChartArea("ChartArea1");
            Chart1.ChartAreas.Add(area);
            Series series = new Series(wD.ShortTitle);
            series.Points.DataBindXY(wD.Keys, wD.Values);
            // Populate series data
            series.IsValueShownAsLabel = true;
            series["LabelStyle"] = "Bottom";
            //series.LabelBackColor = Color.White;
            //series.LabelBorderColor = Color.Black;
            //series.Palette = ChartColorPalette.SeaGreen ;
            //series.EmptyPointStyle.IsValueShownAsLabel = false;
            //series.EmptyPointStyle["EmptyPointValue"] = "Zero";
            series.ChartType = SeriesChartType.Column;
            Chart1.Series.Add(series);
            Series series2 = new Series(mD.ShortTitle);
            series2.Points.DataBindXY(mD.Keys, mD.Values);
            // Populate series data
            series2.IsValueShownAsLabel = true;
            //series2.LabelBackColor = Color.White;
            //series2.LabelBorderColor = Color.Black;
            series2["LabelStyle"] = "Bottom";
            //series2.Palette = ChartColorPalette.EarthTones;
            series2.EmptyPointStyle.IsValueShownAsLabel = false;
            series2.EmptyPointStyle["EmptyPointValue"] = "Zero";
            // Set Doughnut chart type
            series2.ChartType = SeriesChartType.Column;
            Chart1.Series.Add(series2);

            Series series3 = new Series(yD.ShortTitle);
            series3.Points.DataBindXY(yD.Keys, yD.Values);
            series3.IsValueShownAsLabel = true;
            series3["LabelStyle"] = "Bottom";
            series3.EmptyPointStyle.IsValueShownAsLabel = false;
            series3.EmptyPointStyle["EmptyPointValue"] = "Zero";
            series3.ChartType = SeriesChartType.Column;
            Chart1.Series.Add(series3);
            
            // Enable 3D
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            // Disable the Legend
            Chart1.Legends.Add("Legend1");
            // show legend based on check box value
            Chart1.Legends["Legend1"].Enabled = true;

            var returnStream = new MemoryStream();
            Chart1.ImageType = ChartImageType.Png;
            Chart1.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }

        public ActionResult ActiveChildAbductions()
        {
            List<CAReportItem> results = GetActiveChildAbductions();
            return View(results);
        }

        public ActionResult ActiveWarrants()
        {
            List<WReportItem> results = GetActiveWarrants();
            return View(results);
        }

        public ActionResult ClosedChildAbductions()
        {
            ClosedCases_StandardReport closedCA = new ClosedCases_StandardReport();
            return View(closedCA);
        }

        [HttpPost]
        public ActionResult ClosedChildAbductions(ClosedCases_StandardReport closedCA)
        {
            closedCA.CAItems = GetClosedChildAbductions(closedCA.Start.Value, closedCA.End.Value);
            return View(closedCA);
        }

        public ActionResult ClosedWarrants()
        {
            ClosedCases_StandardReport closedCA = new ClosedCases_StandardReport();
            return View(closedCA);
        }

        [HttpPost]
        public ActionResult ClosedWarrants(ClosedCases_StandardReport closedCA)
        {
            closedCA.WItems = GetClosedWarrants(closedCA.Start.Value, closedCA.End.Value);
            return View(closedCA);
        }

        public ActionResult WarrantsExcel(DateTime? start = null, DateTime? end = null)
        {
            List<WReportItem> warrantItems = new List<WReportItem>();
            string wsName = "";
            if (start == null)
            {
                //Get Active Warrants
                warrantItems = GetActiveWarrants();
                wsName = "Active Warrants";
            }
            else
            {
                warrantItems = GetClosedWarrants(start.Value, end.Value);
                wsName = "Closed Warrants";
            }
            List<WExcelReportItem> excelItems = new List<WExcelReportItem>();
            foreach (WReportItem w in warrantItems)
            {
                WExcelReportItem e = new WExcelReportItem();
                e.UniqueRecordID = w.UniqueRecordID;
                e.DateCirculated = w.DateCirculated;
                e.ClosedDate = w.ClosedDate;
                e.NPO = w.NPO;
                if (w.respondents.Count>0)
                {
                    e.RespondentName = w.respondents.ToList()[0].PoliceDisplayName;
                    e.RespondentDOB = w.respondents.ToList()[0].DateofBirthDisplay;
                    e.RespondentPNCID = w.respondents.ToList()[0].PNCID;
                }
                excelItems.Add(e);
            }
            return GenerateWReport(excelItems, wsName);
        }

        public ActionResult ChildAbductionsExcel(DateTime? start = null, DateTime? end = null)
        {
            List<CAReportItem> caItems = new List<CAReportItem>();
            string wsName = "";
            if (start == null)
            {
                //Get Active Warrants
                caItems = GetActiveChildAbductions();
                wsName = "Active Child Abductions";
            }
            else
            {
                caItems = GetClosedChildAbductions(start.Value, end.Value);
                wsName = "Closed Warrants";
            }
            List<CAExcelReportItem> excelItems = new List<CAExcelReportItem>();
            foreach (CAReportItem c in caItems)
            {
                CAExcelReportItem e = new CAExcelReportItem();
                int numberOfChildren = c.children.Count();
                int numberOfRespondents = c.respondents.Count();
                e.UniqueRecordID = c.UniqueRecordID;
                e.DateCirculated = c.DateCirculated;
                e.ClosedDate = c.ClosedDate;
                e.NPO = c.NPO;
                if (numberOfChildren > 0)
                {
                    e.ChildrenName = c.children.ToList()[0].PoliceDisplayName;
                    e.ChildrenDOB= c.children.ToList()[0].DateofBirthDisplay;
                    e.ChildrenPNCID = c.children.ToList()[0].PNCID;
                }
                if (numberOfRespondents> 0)
                {
                    e.RespondentName = c.respondents.ToList()[0].PoliceDisplayName;
                    e.RespondentDOB = c.respondents.ToList()[0].DateofBirthDisplay;
                    e.RespondentPNCID = c.respondents.ToList()[0].PNCID;
                }
                excelItems.Add(e);
                if (numberOfChildren > 1 || numberOfRespondents > 1)
                {
                    int max = numberOfChildren > numberOfRespondents ? numberOfChildren : numberOfRespondents;
                    for (int i = 1; i < max; i++)
                    {
                        CAExcelReportItem e1 = new CAExcelReportItem();
                        e1.UniqueRecordID = "";
                        e1.DateCirculated = null;
                        e1.ClosedDate = null;
                        e1.NPO = "";
                        if (i < numberOfChildren)
                        {
                            e1.ChildrenName = c.children.ToList()[i].PoliceDisplayName;
                            e1.ChildrenDOB = c.children.ToList()[i].DateofBirthDisplay;
                            e1.ChildrenPNCID = c.children.ToList()[i].PNCID;
                        }
                        if (i < numberOfRespondents)
                        {
                            e1.RespondentName = c.respondents.ToList()[i].PoliceDisplayName;
                            e1.RespondentDOB = c.respondents.ToList()[i].DateofBirthDisplay;
                            e1.RespondentPNCID = c.respondents.ToList()[i].PNCID;
                        }
                        excelItems.Add(e1);
                    }
                }
            }
            return GenerateCAReport(excelItems, wsName);
        }

        private List<WReportItem> GetActiveWarrants()
        {
            List<WReportItem> results = new List<WReportItem>();
            List<Warrant> activeWs = db.Warrants.Where(c => c.caseStatusID == 1 || c.caseStatusID == 2).ToList();
            foreach (Warrant w in activeWs)
            {
                WReportItem i = new WReportItem();
                i.tipstaffRecordID = w.tipstaffRecordID;
                i.UniqueRecordID = w.UniqueRecordID;
                i.DateCirculated = w.DateCirculated;
                i.ClosedDate = null;
                i.respondents = w.Respondents;
                i.NPO = w.NPO;
                results.Add(i);
            }
            return results;
        }

        private List<WReportItem> GetClosedWarrants(DateTime start, DateTime end)
        {
            List<WReportItem> results = new List<WReportItem>();
            List<Warrant> closedWs = db.Warrants.Where(c => c.caseStatusID == 3 && c.resultDate >= start && c.resultDate <= end).OrderBy(c1 => c1.resultDate).ToList();
            foreach (Warrant w in closedWs)
            {
                WReportItem i = new WReportItem();
                i.tipstaffRecordID = w.tipstaffRecordID;
                i.UniqueRecordID = w.UniqueRecordID;
                i.DateCirculated = w.DateCirculated;
                i.ClosedDate = w.resultDate;
                i.respondents = w.Respondents;
                i.NPO = w.NPO;
                results.Add(i);
            }
            return results;
        }

        private List<CAReportItem> GetActiveChildAbductions()
        {
            List<CAReportItem> results = new List<CAReportItem>();
            List<ChildAbduction> activeCAs = db.ChildAbductions.Where(c => c.caseStatusID == 1 || c.caseStatusID == 2).ToList();
            foreach (ChildAbduction c in activeCAs)
            {
                CAReportItem i = new CAReportItem();
                i.tipstaffRecordID = c.tipstaffRecordID;
                i.UniqueRecordID = c.UniqueRecordID;
                i.DateCirculated = c.sentSCD26;
                i.ClosedDate = null;
                i.children = c.children;
                i.respondents = c.Respondents;
                i.NPO = c.NPO;
                results.Add(i);
            }
            return results;
        }

        private List<CAReportItem> GetClosedChildAbductions(DateTime start, DateTime end)
        {
            List<CAReportItem> results = new List<CAReportItem>();
            List<ChildAbduction> activeCAs = db.ChildAbductions.Where(c => c.caseStatusID == 3 && c.resultDate >= start && c.resultDate <= end).OrderBy(c1 => c1.resultDate).ToList();
            foreach (ChildAbduction c in activeCAs)
            {
                CAReportItem i = new CAReportItem();
                i.tipstaffRecordID = c.tipstaffRecordID;
                i.UniqueRecordID = c.UniqueRecordID;
                i.DateCirculated = c.sentSCD26;
                i.ClosedDate = c.resultDate;
                i.children = c.children;
                i.respondents = c.Respondents;
                i.NPO = c.NPO;
                results.Add(i);
            }
            return results;
        }
        private ActionResult GenerateWReport(List<WExcelReportItem> items, string wsName)
        {
            using (Excel xl = new Excel())
            {
                xl.AddDataWorksheet<WExcelReportItem>(wsName, items);
                MemoryStream ms = xl.getMemoryStream();
                string filename = wsName.Replace(" ","") + ".xls";
                return File(ms, "application/msexcel", filename);
            }
        }
        
        private ActionResult GenerateCAReport(List<CAExcelReportItem> items, string wsName)
        {
            using (Excel xl = new Excel())
            {
                xl.AddDataWorksheet<CAExcelReportItem>(wsName, items);
                MemoryStream ms = xl.getMemoryStream();
                string filename = wsName.Replace(" ", "") + ".xls";
                return File(ms, "application/msexcel", filename);
            }
        }
    }
}
