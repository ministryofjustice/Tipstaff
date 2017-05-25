/*
 ASP.NET MvcPager control
 Copyright:2009-2010 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
*/
using System.Linq;
using System.Collections.Generic;

namespace Tipstaff
{
    public static class PageLinqExtensions
    {
        public static xPagedList<T> ToXPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new xPagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        public static xPagedList<T> ToXPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
        {

            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new xPagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }
}