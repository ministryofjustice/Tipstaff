﻿using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using Tipstaff.Models;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using PagedList;
using System.Web;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Web.Security;
using Tipstaff.Presenters;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class HomeController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly ISearchPresenter _searchPresenter;
        private readonly IGraphPresenter _graphPresenter;

        public HomeController(ITipstaffRecordPresenter tipstaffRecordPresenter, 
                              ISearchPresenter searchPresenter, 
                              IGraphPresenter graphPresenter)
        {
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _searchPresenter = searchPresenter;
            _graphPresenter = graphPresenter;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = string.Format("Welcome to the {0}", ConfigurationManager.AppSettings["AppName"]);
            return View();
        }

        public PartialViewResult CaseClosedIssues()
        {
            TipstaffCaseClosedDataModel data = new TipstaffCaseClosedDataModel()
            {
                TipstaffRecords = _tipstaffRecordPresenter.GetAll().Where(x => x.caseStatus.CaseStatusId > 2 && x.result == null)
            };

            return PartialView("_CaseClosedIssues", data);
        }

        [HttpPost]
        public ActionResult Search(string searchRecord)
        {
            //////var sm = new SearchModel(searchRecord);
            var sm = _searchPresenter.GetSearchModel(searchRecord);

            if (sm.isValid)
            {
                switch (sm.searchType)
                {
                    case SearchType.RecordNumber:
                        ////using (TipstaffDB db = new TipstaffDB())
                        ////{
                        ////    var tipstaffRecord = db.TipstaffRecord.Find(sm.search);
                        ////    if (tipstaffRecord != null)
                        ////    {
                        ////        return RedirectToAction("Details", sm.RecordType, new { id = sm.tipstaffRecord.tipstaffRecordID });
                        ////    }
                        ////    else
                        ////    {
                        ////        return View(sm);
                        ////    }
                        ////}
                        var tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(sm.search.ToString());
                        if (tipstaffRecord != null)
                        {
                            return RedirectToAction("Details", sm.RecordType, new { id = sm.tipstaffRecord.tipstaffRecordID });
                        }
                        else
                        {
                            return View(sm);
                        }
                    case SearchType.Name:
                        return View(sm);
                    case SearchType.DateOfBirth:
                        return View(sm);
                    default:
                        break;
                }

            }
            return View(sm);
        }


        public ActionResult Issue(GraphPeriod gp)
        {

            //////GraphData wg = new GraphData(gp);
            GraphData wg = _graphPresenter.GetGraphData(gp);
            var Chart1 = new Chart();
            Chart1.Width = 500;
            Chart1.Height = 350;
            Title title = new Title(wg.Title);
            title.Font = new Font("Calibri", 12, FontStyle.Underline);
            Chart1.Titles.Add(title);

            var area = new ChartArea("ChartArea1");
            Chart1.ChartAreas.Add(area);
            Series series = new Series("Issues", 10);
            ////series.Points.DataBindXY(wg.Keys, wg.Values);
            series.Points.DataBindXY(wg.gData.Keys, wg.gData.Values);
            // Populate series data
            series.IsValueShownAsLabel = true;
            //series.LabelBackColor = Color.White;
            //series.LabelBorderColor = Color.Black;
            series["LabelStyle"] = "Bottom";
            series.Palette = gp==GraphPeriod.week? ChartColorPalette.SeaGreen : ChartColorPalette.EarthTones;
            series.EmptyPointStyle.IsValueShownAsLabel = false;
            series.EmptyPointStyle["EmptyPointValue"] = "Zero";
            // Set Doughnut chart type
            series.ChartType = SeriesChartType.Column;
            Chart1.Series.Add(series);
            // Enable 3D
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            // Disable the Legend
            Chart1.Legends.Add("Legend1");
            // show legend based on check box value
            Chart1.Legends["Legend1"].Enabled = false;

            var returnStream = new MemoryStream();
            Chart1.ImageType = ChartImageType.Png;
            Chart1.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }

        public ActionResult DrawBarChart()
        {
            var chart = new System.Web.Helpers.Chart(width: 500, height: 350)
                .SetXAxis("")
                .SetYAxis("")
                .AddTitle("Number of open warrants by type")
                .AddLegend("Legend", "Legend")
                .AddSeries(name: "Child Abductions",
                            chartType: "column",
                            legend: "Legend",
                            xValue: new[] { "Awaiting Information","Active","Port Alert Only" },
                            xField: "Status",
                            yValues: new[] { "27", "4","10" })
                .AddSeries(name:"Warrants",
                            chartType: "column",
                            legend: "Legend",
                            xValue: new[] { "Awaiting Information", "Active" },
                            xField: "Status",
                            yValues: new[] { "6", "12" })
                            
                .GetBytes("png");
            return File(chart, "image/bytes");
        }
        public ActionResult DrawPieChart()
        {
            var Chart1 = new Chart();
            Chart1.Titles.Add("Hi there!");
            Chart1.Width = 500;
            Chart1.Height = 350;
            var area = new ChartArea("ChartArea1");
            Chart1.ChartAreas.Add(area);
            var series = new Series();
            // Populate series data
            double[] yValues = { 30, 45 };
            string[] xValues = { "Warrants", "Child abductions" };
            series.Points.DataBindXY(xValues, yValues);
            // Set Doughnut chart type
            series.ChartType = SeriesChartType.Pie;
            // Set labels style
            series["PieLabelStyle"] = "inside";
            series["PieDrawingStyle"] = "Concave";
            // Set Doughnut radius percentage
            series["DoughnutRadius"] = "75";
            // Explode data point 
            //series.Points[1]["Exploded"] = "true";
            Chart1.Series.Add(series);
            // Enable 3D
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            // Disable the Legend
            Chart1.Legends.Add("Legend1");
            // show legend based on check box value
            Chart1.Legends["Legend1"].Enabled = false;

            var returnStream = new MemoryStream();
            Chart1.ImageType = ChartImageType.Png;
            Chart1.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }
    }
}
    