using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Presenters
{
    public class SearchPresenter : ISearchPresenter
    {
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IChildPresenter _childPresenter;
        private readonly IRespondentPresenter _respondentPresenter;

        Regex rxNumeric = new Regex(@"^[0-9]+$");
        Regex rxText = new Regex(@"^[a-z,A-Z]+$");
        
        public SearchPresenter(ITipstaffRecordPresenter tipstaffRecordPresenter, 
                               IChildPresenter childPresenter, 
                               IRespondentPresenter respondentPresenter)
        {
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _childPresenter = childPresenter;
            _respondentPresenter = respondentPresenter;
        }

        public SearchModel GetSearchModel(string searchRecord)
        {
            var mdl = new SearchModel();
            mdl.searchSource = searchRecord;
            var searchResults = new List<SearchResultRow>();
            try
            {
                DateTime output = DateTime.Parse(mdl.searchSource);
                mdl.searchType = SearchType.DateOfBirth;
                //if (output.ToString("d/M/yy") == searchSource.ToString())
                if (mdl.searchSource.Count(c => c == '/') == 2)
                {
                    mdl.searchResults = GetMatches(output);
                }
                else
                {
                    mdl.searchResults = GetMatches(output.Month, output.Day);
                }
                mdl.isValid = true;
                return mdl;
            }
            catch
            {
                mdl.isValid = false;
                mdl.searchType = null;
            }
            try
            {
                if (rxText.IsMatch(mdl.searchSource))
                {
                    mdl.searchType = SearchType.Name;
                    mdl.isValid = true;
                    mdl.searchResults = GetMatches(mdl.searchSource);
                    return mdl;
                }
            }
            catch
            {
                mdl.isValid = false;
            }
            try
            {
                if (rxNumeric.IsMatch(mdl.searchSource))
                {
                    mdl.search = Int32.Parse(mdl.searchSource);
                    mdl.searchType = SearchType.RecordNumber;
                    mdl.isValid = true;
                }
            }
            catch
            {
                mdl.errorMessage = string.Format("Your search text '{0}' could not be converted into the TRxxxxxx format", mdl.searchSource);
                mdl.isValid = false;
                mdl.searchType = null;
                return mdl;
            }

            mdl.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(mdl.search.ToString());

            if (mdl.tipstaffRecord == null && mdl.search > 0)
            {
                mdl.errorMessage = string.Format("No record could be found that matched '{0}' ", string.Format("TR{0}", mdl.search.ToString("D6")));
                mdl.isValid = false;
                mdl.searchType = null;
                return mdl;
            }
            try
            {
                mdl.RecordType = genericFunctions.TypeOfTipstaffRecord(mdl.tipstaffRecord);
                return mdl;
            }
            catch
            {
                mdl.errorMessage = string.Format("Type of record could not be determined");
                mdl.isValid = false;
                mdl.searchType = null;
                return mdl;
            }
        }

        private List<SearchResultRow> GetMatches(int month, int day)
        {
            //TipstaffDB db = myDBContextHelper.CurrentContext;
            try
            {
                List<SearchResultRow> Results = new List<SearchResultRow>();
                var children = _childPresenter.GetAllChildren();
                var kids = children.Where(f => ((DateTime)f.dateOfBirth).Month == month || ((DateTime)f.dateOfBirth).Day == day);
                if (kids != null)
                {
                    foreach (Child child in kids)
                    {
                        Results.Add(new SearchResultRow
                        {
                            name = child.fullname,
                            partyID = child.childID,
                            tipstaffRecordID = child.tipstaffRecordID,
                            DateOfBirth = (DateTime)child.dateOfBirth,
                            PartyType = "Child",
                            tipstaffRecordType = "ChildAbduction",
                            tipstaffUniqueRecordID = child.childAbduction.UniqueRecordID
                        });
                    }
                }
                var respondents = _respondentPresenter.GetAll();
                var Resps = respondents.Where(f => ((DateTime)f.dateOfBirth).Month == month || ((DateTime)f.dateOfBirth).Day == day);
                if (Resps != null)
                {
                    foreach (Respondent resp in Resps)
                    {
                        Results.Add(new SearchResultRow
                        {
                            name = resp.fullname,
                            partyID = resp.respondentID.ToString(), //resp.respondentID,
                            tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is incorrect, as we have to modify respondent to handle TipstaffRecordID as string
                            DateOfBirth = (DateTime)resp.dateOfBirth,
                            PartyType = "Respondent",
                            tipstaffRecordType = genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID),
                            tipstaffUniqueRecordID = resp.tipstaffRecord.UniqueRecordID
                        });
                    }
                }
                return Results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return null;

            }
        }
        private List<SearchResultRow> GetMatches(DateTime DoB)
        {
            /////TipstaffDB db = myDBContextHelper.CurrentContext;
            try
            {
                List<SearchResultRow> Results = new List<SearchResultRow>();
                ////var kids = db.Children.Where(f => f.dateOfBirth == DoB);
                var children = _childPresenter.GetAllChildren();
                if (children != null)
                {
                    foreach (Child child in children)
                    {
                        string uniqID = "";
                        using (TipstaffDB tempDB = new TipstaffDB())
                        {
                            TipstaffRecord TR = tempDB.TipstaffRecord.Find(child.tipstaffRecordID);
                            uniqID = TR.UniqueRecordID;
                        };

                        Results.Add(new SearchResultRow
                        {
                            name = child.fullname,
                            partyID = child.childID,
                            tipstaffRecordID = child.tipstaffRecordID,
                            DateOfBirth = (DateTime)child.dateOfBirth,
                            PartyType = "Child",
                            tipstaffRecordType = "ChildAbduction",
                            tipstaffUniqueRecordID = uniqID
                        });
                    }
                }
                var respondents = _respondentPresenter.GetAll();
                var Resps = respondents.Where(f => f.dateOfBirth == DoB);
                if (Resps != null)
                {
                    foreach (Respondent resp in Resps)
                    {
                        string uniqID = "";
                        using (TipstaffDB tempDB = new TipstaffDB())
                        {
                            TipstaffRecord TR = tempDB.TipstaffRecord.Find(resp.tipstaffRecordID);
                            uniqID = TR.UniqueRecordID;
                        };
                        Results.Add(new SearchResultRow
                        {
                            name = resp.fullname,
                            partyID = resp.respondentID.ToString(), //this is not correct, as respondentid will have to change to be string when migrated to nosql
                            tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is not correct, we will have to change respondent.tipstaffrecordid to String when migrating to NoSQL
                            DateOfBirth = (DateTime)resp.dateOfBirth,
                            PartyType = "Respondent",
                            tipstaffRecordType = genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID),
                            tipstaffUniqueRecordID = uniqID
                        });
                    }
                }
                return Results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return null;

            }
        }
        private List<SearchResultRow> GetMatches(string Name)
        {
            TipstaffDB db = myDBContextHelper.CurrentContext;
            try
            {
                List<SearchResultRow> Results = new List<SearchResultRow>();
                var kids = db.Children.Where(f => f.nameFirst.Contains(Name) || f.nameMiddle.Contains(Name) || f.nameLast.Contains(Name));
                if (kids != null)
                {
                    foreach (Child child in kids)
                    {
                        string uniqID = "";
                        using (TipstaffDB tempDB = new TipstaffDB())
                        {
                            TipstaffRecord TR = tempDB.TipstaffRecord.Find(child.tipstaffRecordID);
                            uniqID = TR.UniqueRecordID;
                        };
                        Results.Add(new SearchResultRow
                        {
                            name = child.fullname,
                            partyID = child.childID,
                            tipstaffRecordID = child.tipstaffRecordID,
                            DateOfBirth = (DateTime)child.dateOfBirth,
                            PartyType = "Child",
                            tipstaffRecordType = "ChildAbduction",
                            tipstaffUniqueRecordID = uniqID
                        });
                    }
                }
                var Resps = db.Respondents.Where(f => f.nameFirst.Contains(Name) || f.nameMiddle.Contains(Name) || f.nameLast.Contains(Name));
                System.Diagnostics.Debug.Print(Resps.Count().ToString());
                if (Resps != null)
                {
                    foreach (Respondent resp in Resps)
                    {
                        string typePart2 = "";
                        string uniqID = "";
                        using (TipstaffDB tempDB = new TipstaffDB())
                        {
                            if (genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID) == "Warrant")
                            {
                                Warrant temp = tempDB.Warrants.Find(resp.tipstaffRecordID);
                                typePart2 = string.Format(" ({0})", temp.division.Detail);
                            }
                            TipstaffRecord TR = tempDB.TipstaffRecord.Find(resp.tipstaffRecordID);
                            uniqID = TR.UniqueRecordID;
                        };
                        Results.Add(new SearchResultRow
                        {
                            name = resp.fullname,
                            partyID = resp.respondentID.ToString(), //this is not correct, as respondentid will have to change to be string when migrated to nosql
                            tipstaffRecordID = resp.tipstaffRecordID.ToString(), //this is not correct, we will have to change respondent.tipstaffrecordid to String when migrating to NoSQL
                            DateOfBirth = resp.dateOfBirth,
                            PartyType = "Respondent",
                            tipstaffRecordType = string.Format("{0}{1}", genericFunctions.TypeOfTipstaffRecord(resp.tipstaffRecordID)
                                                                        , typePart2),
                            tipstaffUniqueRecordID = uniqID
                        });
                    }
                }
                return Results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                //return null;
                throw new NotUploaded(ex.Message);
            }
        }
    }
}