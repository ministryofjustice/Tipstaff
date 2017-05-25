using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Tipstaff
{
    public static partial class genericFunctions
    {
        /// <summary>
        /// Send in the number of records and a pluralised string descriptor
        /// </summary>
        /// <param name="numberofRecords">Number of records (from database?)</param>
        /// <param name="toAssess">Field descriptor in plural form</param>
        /// <returns></returns>
        public static string DisplayFieldDescriptorWithRecordCount(int numberofRecords, string toAssess)
        {
            string returnVal = toAssess;
            if (numberofRecords == 1)
            {
                PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));
                returnVal = ps.Singularize(toAssess);
            }
            return string.Format("{0} {1}", numberofRecords, returnVal);
        }
        /// <summary>
        /// Send in the number of records and a series of pluralised words, each word will be singularised if
        /// the record count = 1
            /// </summary>
        /// <param name="numberofRecords">Number of records (from database?)</param>
        /// <param name="toAssess">A comma delimited series of strings that will be assessed and singularised</param>
        /// <returns></returns>
        public static string DisplayFieldDescriptorWithRecordCount(int numberofRecords, params string[] toAssess)
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));
            List<string> output = new List<string>();
            output.Add(numberofRecords.ToString());
            foreach (var word in toAssess)
            {
                string returnVal = word;
                if (numberofRecords == 1)
                {
                    returnVal = ps.Singularize(returnVal);
                }
                output.Add(returnVal);
            }


            return string.Join(" ",output);
        }
    }
}