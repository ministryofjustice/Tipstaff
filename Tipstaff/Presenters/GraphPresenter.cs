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
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;

        public GraphPresenter(ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        public GraphData GetGraphData(GraphPeriod gp)
        {
            var graph = new GraphData();
            DateTime startDate = new DateTime();
            var graphPeriod = gp;
            var all = _tipstaffRecordPresenter.GetAll();
            var warants = all.Select(x => x.Discriminator == "Warrant") as List<Warrant>;
            var childAbductions = all.Select(x => x.Discriminator == "ChildAbduction") as List<ChildAbduction>;
            
            ////IEnumerable<Warrant> w = db.Warrants.Where(c => c.createdOn >= graph.startDate).
            ////    OrderBy(c => c.division.Detail);
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
            
            var w = warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail);

            graph.gData.Add("Child Abductions", childAbductions.Where(c => c.createdOn >= startDate).Count());
            graph.gData.Add("Bankruptcy", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(d => d.division.Detail == "Bankruptcy").Count());
            graph.gData.Add("Chancery", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(e => e.division.Detail == "Chancery").Count());
            graph.gData.Add("Family", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Family").Count());
            graph.gData.Add("Insolvency", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Insolvency").Count()); // null);//
            graph.gData.Add("Queen's Bench", warants.Where(c => c.createdOn >= startDate).OrderBy(c => c.division.Detail).Where(c => c.division.Detail == "Queen's Bench").Count());

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