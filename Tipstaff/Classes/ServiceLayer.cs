using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceLayer
{
    public interface IUnitOfWorkDataStore
    {
        object this[string key] { get; set; }
    }
    public static class UnitOfWorkHelper
    {
        public static IUnitOfWorkDataStore CurrentDataStore;
    }
}