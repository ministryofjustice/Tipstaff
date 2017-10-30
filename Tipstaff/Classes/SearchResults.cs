using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff
{
    ////public class SearchResults
    ////{
    ////    public SearchResults()
    ////    {

    ////    }

    ////    public List<SearchResultRow> getMatches(int month, int day)
    ////    {
    ////        TipstaffDB db = myDBContextHelper.CurrentContext;
    ////        try
    ////        {
    ////            List<SearchResultRow> Results = new List<SearchResultRow>();
    ////            var kids = db.Children.Where(f => ((DateTime)f.dateOfBirth).Month == month || ((DateTime)f.dateOfBirth).Day==day);
    ////            if (kids != null)
    ////            {
    ////                foreach (Child child in kids)
    ////                {
    ////                    Results.Add(new SearchResultRow
    ////                    {
    ////                        name = child.fullname,
    ////                        partyID = child.childID,
    ////                        tipstaffRecordID = child.tipstaffRecordID,
    ////                        DateOfBirth = (DateTime)child.dateOfBirth,
    ////                        PartyType = "Child",
    ////                        tipstaffRecordType = "ChildAbduction",
    ////                        tipstaffUniqueRecordID = child.childAbduction.UniqueRecordID
    ////                    });
    ////                }
    ////            }
    ////            var Resps = db.Respondents.Where(f => ((DateTime)f.dateOfBirth).Month == month || ((DateTime)f.dateOfBirth).Day == day);
    ////            if (Resps != null)
    ////            {
    ////                foreach (Respondent resp in Resps)
    ////                {
    ////                    Results.Add(new SearchResultRow
    ////                    {
    ////                        name = resp.fullname,
    ////                        partyID = resp.respondentID.ToString(), //resp.respondentID,
    ////                        tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is incorrect, as we have to modify respondent to handle TipstaffRecordID as string
    ////                        DateOfBirth = (DateTime)resp.dateOfBirth,
    ////                        PartyType = "Respondent",
    ////                        tipstaffRecordType = genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID),
    ////                        tipstaffUniqueRecordID = resp.tipstaffRecord.UniqueRecordID
    ////                    });
    ////                }
    ////            }
    ////            return Results;
    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            System.Diagnostics.Debug.Print(ex.Message);
    ////            return null;

    ////        }
    ////    }
    ////    public List<SearchResultRow> getMatches(DateTime DoB)
    ////    {
    ////        /////TipstaffDB db = myDBContextHelper.CurrentContext;
    ////        try
    ////        {
    ////            List<SearchResultRow> Results = new List<SearchResultRow>();
    ////            var kids = db.Children.Where(f => f.dateOfBirth == DoB);
    ////            if (kids != null)
    ////            {
    ////                foreach (Child child in kids)
    ////                {
    ////                    string uniqID = "";
    ////                    using (TipstaffDB tempDB = new TipstaffDB())
    ////                    {
    ////                        TipstaffRecord TR = tempDB.TipstaffRecord.Find(child.tipstaffRecordID);
    ////                        uniqID = TR.UniqueRecordID;
    ////                    };

    ////                    Results.Add(new SearchResultRow
    ////                    {
    ////                        name = child.fullname,
    ////                        partyID = child.childID,
    ////                        tipstaffRecordID = child.tipstaffRecordID,
    ////                        DateOfBirth = (DateTime)child.dateOfBirth,
    ////                        PartyType = "Child",
    ////                        tipstaffRecordType = "ChildAbduction",
    ////                        tipstaffUniqueRecordID = uniqID
    ////                    });
    ////                }
    ////            }
    ////            var Resps = db.Respondents.Where(f => f.dateOfBirth == DoB);
    ////            if (Resps != null)
    ////            {
    ////                foreach (Respondent resp in Resps)
    ////                {
    ////                    string uniqID = "";
    ////                    using (TipstaffDB tempDB = new TipstaffDB())
    ////                    {
    ////                        TipstaffRecord TR = tempDB.TipstaffRecord.Find(resp.tipstaffRecordID);
    ////                        uniqID = TR.UniqueRecordID;
    ////                    };
    ////                    Results.Add(new SearchResultRow
    ////                    {
    ////                        name = resp.fullname,
    ////                        partyID = resp.respondentID.ToString(), //this is not correct, as respondentid will have to change to be string when migrated to nosql
    ////                        tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is not correct, we will have to change respondent.tipstaffrecordid to String when migrating to NoSQL
    ////                        DateOfBirth = (DateTime)resp.dateOfBirth,
    ////                        PartyType = "Respondent",
    ////                        tipstaffRecordType = genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID),
    ////                        tipstaffUniqueRecordID = uniqID
    ////                    });
    ////                }
    ////            }
    ////            return Results;
    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            System.Diagnostics.Debug.Print(ex.Message);
    ////            return null;

    ////        }
    ////    }
    ////    public static List<SearchResultRow> getMatches(string Name)
    ////    {
    ////        TipstaffDB db = myDBContextHelper.CurrentContext;
    ////        try
    ////        {
    ////            List<SearchResultRow> Results = new List<SearchResultRow>();
    ////            var kids = db.Children.Where(f => f.nameFirst.Contains(Name) || f.nameMiddle.Contains(Name) || f.nameLast.Contains(Name));
    ////            if (kids != null)
    ////            {
    ////                foreach (Child  child in kids)
    ////                {
    ////                    string uniqID = "";
    ////                    using (TipstaffDB tempDB = new TipstaffDB())
    ////                    {
    ////                        TipstaffRecord TR = tempDB.TipstaffRecord.Find(child.tipstaffRecordID);
    ////                        uniqID = TR.UniqueRecordID;
    ////                    };
    ////                    Results.Add(new SearchResultRow 
    ////                    { 
    ////                        name = child.fullname, 
    ////                        partyID = child.childID,
    ////                        tipstaffRecordID = child.tipstaffRecordID, 
    ////                        DateOfBirth = (DateTime)child.dateOfBirth, 
    ////                        PartyType="Child", 
    ////                        tipstaffRecordType = "ChildAbduction", 
    ////                        tipstaffUniqueRecordID = uniqID
    ////                    });
    ////                }
    ////            }
    ////            var Resps = db.Respondents.Where(f => f.nameFirst.Contains(Name) || f.nameMiddle.Contains(Name) || f.nameLast.Contains(Name));
    ////            System.Diagnostics.Debug.Print(Resps.Count().ToString());
    ////            if (Resps != null)
    ////            {
    ////                foreach (Respondent resp in Resps)
    ////                {
    ////                    string typePart2 = "";
    ////                    string uniqID = "";
    ////                    using (TipstaffDB tempDB = new TipstaffDB())
    ////                    {
    ////                        if (genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID)=="Warrant") {
    ////                            Warrant temp = tempDB.Warrants.Find(resp.tipstaffRecordID);
    ////                            typePart2 = string.Format(" ({0})", temp.division.Detail);
    ////                        }
    ////                        TipstaffRecord TR = tempDB.TipstaffRecord.Find(resp.tipstaffRecordID);
    ////                        uniqID = TR.UniqueRecordID;
    ////                    };
    ////                    Results.Add(new SearchResultRow
    ////                    {
    ////                        name = resp.fullname,
    ////                        partyID = resp.respondentID.ToString(), //this is not correct, as respondentid will have to change to be string when migrated to nosql
    ////                        tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is not correct, we will have to change respondent.tipstaffrecordid to String when migrating to NoSQL
    ////                        DateOfBirth = resp.dateOfBirth,
    ////                        PartyType = "Respondent",
    ////                        tipstaffRecordType = string.Format("{0}{1}", genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID)
    ////                                                                    ,typePart2),
    ////                        tipstaffUniqueRecordID = uniqID
    ////                    });
    ////                }
    ////            }
    ////            return Results;
    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            System.Diagnostics.Debug.Print(ex.Message);
    ////            //return null;
    ////            throw new NotUploaded(ex.Message);
    ////        }
    ////    }
       
    ////}

    public class SearchResultRow
    {
        public string name { get; set; }
        //public int tipstaffRecordID { get; set; } //DBid field
        public string tipstaffRecordID { get; set; } //DBid field
        public string tipstaffUniqueRecordID { get; set; }
        public string tipstaffRecordType { get; set; } //for controller

        //public int partyID { get; set; } //DBid field

        public string partyID { get; set; } //DBid field
        public string PartyType { get; set; } //child or respondent
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
    }

}