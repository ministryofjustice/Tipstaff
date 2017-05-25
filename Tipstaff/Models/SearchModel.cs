using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace Tipstaff.Models
{
    public enum SearchType
    {
        Name,
        DateOfBirth,
        RecordNumber
    }
    public class SearchModel
    {
        Regex rxNumeric = new Regex(@"^[0-9]+$");
        Regex rxText = new Regex(@"^[a-z,A-Z]+$");

        public string searchSource { get; set; }
        public int search { get; private set; }
        public bool isValid { get; private set; }
        public string errorMessage { get; private set; }
        public virtual TipstaffRecord tipstaffRecord { get; private set; }
        public string RecordType { get; private set; }
        public SearchType? searchType{ get; set; }
        public List<SearchResults.SearchResultRow> searchResults { get; set; }
        public SearchModel(string searchRecord)
        {
            searchSource = searchRecord;
            try
            {
                DateTime output = DateTime.Parse(searchSource);
                searchType = SearchType.DateOfBirth;
                //if (output.ToString("d/M/yy") == searchSource.ToString())
                if (searchSource.Count(c => c == '/')==2)
                {
                    searchResults = SearchResults.getMatches(output);
                }
                else
                {
                    searchResults = SearchResults.getMatches(output.Month, output.Day);
                }
                isValid = true;
                return;
            }
            catch
            {
                isValid = false;
                searchType = null;
            }
            try
            {
                if (rxText.IsMatch(searchSource))
                {
                    searchType = SearchType.Name;
                    isValid = true;
                    searchResults = SearchResults.getMatches(searchSource);
                    return;
                }
            }
            catch
            {
                isValid = false;
            }
            try
            {
                if (rxNumeric.IsMatch(searchSource))
                {
                    search = Int32.Parse(searchSource);
                    searchType = SearchType.RecordNumber;
                    isValid = true;
                }
            }
            catch
            {
                errorMessage=string.Format("Your search text '{0}' could not be converted into the TRxxxxxx format",searchSource);
                isValid = false;
                searchType = null;
                return;
            }
            using (TipstaffDB db = new TipstaffDB())
            {
                tipstaffRecord = db.TipstaffRecord.Find(search);
            }
            if (tipstaffRecord==null && search>0)
            {
                errorMessage = string.Format("No record could be found that matched '{0}' ", string.Format("TR{0}", search.ToString("D6")));
                isValid = false;
                searchType = null;
                return;
            }
            try
            {
                RecordType = genericFunctions.TypeOfTipstaffRecord(tipstaffRecord);
            }
            catch
            {
                errorMessage = string.Format("Type of record could not be determined");
                isValid = false;
                searchType = null;
                return;
            }
        }

    }
}