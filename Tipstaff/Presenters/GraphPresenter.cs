using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Presenters
{
    public class GraphPresenter : IGraphPresenter
    {
        
        private readonly IWarrantPresenter _warrantPresenter;
        private readonly IChildAbductionPresenter _childAbductionPresenter;

        public GraphPresenter(IWarrantPresenter warrantPresenter, IChildAbductionPresenter childAbductionPresenter)
        {
            _childAbductionPresenter = childAbductionPresenter;
            _warrantPresenter = warrantPresenter;
        }

        public GraphData GetGraphData(GraphPeriod gp)
        {
            var graph = new GraphData();
            graph.gData = new Dictionary<string, int?>();
            DateTime startDate = new DateTime();
            var graphPeriod = gp;
            var warants = _warrantPresenter.GetAllWarrants();
            var childAbductions = _childAbductionPresenter.GetAllChildAbductions();
            
            switch (graphPeriod)
            {
                case GraphPeriod.week:
                    graph.prefix = "w/c";
                    startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                    graph.displayDate = startDate.ToString("d MMM yyyy");
                    break;
                case GraphPeriod.month:
                    graph.prefix = "";
                    startDate = DateTime.Today.StartOfMonth();
                    graph.displayDate = startDate.ToString("MMMM yyyy");
                    break;
                case GraphPeriod.year:
                    graph.prefix = "";
                    startDate = Convert.ToDateTime(string.Format("1/1/{0}", DateTime.Now.Year.ToString()));
                    graph.displayDate = startDate.ToString("yyyy");
                    break;
                default:
                    break;
            }
            
            var w = warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division.Detail);

            graph.gData.Add("Child Abductions", childAbductions.Where(c => c.createdOn >= startDate).Count());
            graph.gData.Add("Bankruptcy", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division?.Detail).Where(d => d.Division?.Detail == "Bankruptcy").Count());
            graph.gData.Add("Chancery", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division?.Detail).Where(e => e.Division?.Detail == "Chancery").Count());
            graph.gData.Add("Family", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division?.Detail).Where(c => c.Division?.Detail == "Family").Count());
            graph.gData.Add("Insolvency", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division?.Detail).Where(c => c.Division?.Detail == "Insolvency").Count()); // null);//
            graph.gData.Add("Queen's Bench", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.Division?.Detail).Where(c => c.Division?.Detail == "Queen's Bench").Count());

            if (graph.gData["Child Abductions"] == 0) graph.gData["Child Abductions"] = null;
            if (graph.gData["Bankruptcy"] == 0) graph.gData["Bankruptcy"] = null;
            if (graph.gData["Chancery"] == 0) graph.gData["Chancery"] = null;
            if (graph.gData["Family"] == 0) graph.gData["Family"] = null;
            if (graph.gData["Insolvency"] == 0) graph.gData["Insolvency"] = null;
            if (graph.gData["Queen's Bench"] == 0) graph.gData["Queen's Bench"] = null;

            return graph;
        }
    }
}