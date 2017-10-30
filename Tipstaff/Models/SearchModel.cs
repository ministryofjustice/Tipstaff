using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        public string searchSource { get; set; }
        public int search { get; set; }
        public bool isValid { get;  set; }
        public string errorMessage { get; set; }
        public TipstaffRecord tipstaffRecord { get; set; }

        public string RecordType { get; set; }

        public SearchType? searchType{ get; set; }

        public List<SearchResultRow> searchResults { get; set; }

        //public SearchModel(string searchRecord)
        //{
        //    searchSource = searchRecord;
        //    try
        //    {
        //        DateTime output = DateTime.Parse(searchSource);
        //        searchType = SearchType.DateOfBirth;
        //        //if (output.ToString("d/M/yy") == searchSource.ToString())
        //        if (searchSource.Count(c => c == '/')==2)
        //        {
        //            searchResults = SearchResults.getMatches(output);
        //        }
        //        else
        //        {
        //            searchResults = SearchResults.getMatches(output.Month, output.Day);
        //        }
        //        isValid = true;
        //        return;
        //    }
        //    catch
        //    {
        //        isValid = false;
        //        searchType = null;
        //    }
        //    try
        //    {
        //        if (rxText.IsMatch(searchSource))
        //        {
        //            searchType = SearchType.Name;
        //            isValid = true;
        //            searchResults = SearchResults.getMatches(searchSource);
        //            return;
        //        }
        //    }
        //    catch
        //    {
        //        isValid = false;
        //    }
        //    try
        //    {
        //        if (rxNumeric.IsMatch(searchSource))
        //        {
        //            search = Int32.Parse(searchSource);
        //            searchType = SearchType.RecordNumber;
        //            isValid = true;
        //        }
        //    }
        //    catch
        //    {
        //        errorMessage=string.Format("Your search text '{0}' could not be converted into the TRxxxxxx format",searchSource);
        //        isValid = false;
        //        searchType = null;
        //        return;
        //    }
        //    using (TipstaffDB db = new TipstaffDB())
        //    {
        //        tipstaffRecord = db.TipstaffRecord.Find(search);
        //    }
        //    if (tipstaffRecord==null && search>0)
        //    {
        //        errorMessage = string.Format("No record could be found that matched '{0}' ", string.Format("TR{0}", search.ToString("D6")));
        //        isValid = false;
        //        searchType = null;
        //        return;
        //    }
        //    try
        //    {
        //        RecordType = genericFunctions.TypeOfTipstaffRecord(tipstaffRecord);
        //    }
        //    catch
        //    {
        //        errorMessage = string.Format("Type of record could not be determined");
        //        isValid = false;
        //        searchType = null;
        //        return;
        //    }
        //}

    }
}