using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class Result
    {
        public int ResultId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class ResultsList
    {
        public static List<Result> GetResultList()
        {
            return new List<Result>()
            {
                new Result() {  ResultId=1 ,  Detail = "Executed",         Active = 1   },
                new Result() {  ResultId=2 ,  Detail = "Suspended",        Active = 1   },
                new Result() {  ResultId=3 ,  Detail = "Discharged",       Active = 1   },
                new Result() {  ResultId=4 ,  Detail = "Expired",          Active = 1   },
                new Result() {  ResultId=5 ,  Detail = "Lodged in Prison", Active = 1   },
                new Result() {  ResultId=6 ,  Detail = "Arrested",         Active = 1   }
            };
        }

        public static Result GetResultByDetail(string c)
        {
            return GetResultList().Where(x => x.Detail == c).FirstOrDefault();
        }
    }
}