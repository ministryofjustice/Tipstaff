using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class Division
    {
        public int DivisionId { get; set; }

        public string Detail { get; set; }

        public string Prefix { get; set; }

        public int Active { get; set; }
    }

    public class DivisionsList
    {
        public static List<Division> GetResultList()
        {
            return new List<Division>()
            {
                new Division() {  DivisionId=1 ,  Detail = "Bankruptcy",    Active = 1  , Prefix ="B" },
                new Division() {  DivisionId=2 ,  Detail = "Chancery",      Active = 1  , Prefix ="CH" },
                new Division() {  DivisionId=3 ,  Detail = "Family",        Active = 1  , Prefix ="FAM" },
                new Division() {  DivisionId=4 ,  Detail = "Insolvency",    Active = 1  , Prefix ="IN" },
                new Division() {  DivisionId=5 ,  Detail = "Queen's Bench", Active = 1  , Prefix ="QB" }
            };
        }
        public static Division GetDivisionByID(int id)
        {
            return GetResultList().Where(x => x.DivisionId == id).FirstOrDefault();
        }
    }
}