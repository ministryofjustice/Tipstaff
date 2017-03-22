﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Tipstaff.Models;
using System.Web.Security;
using System.Configuration;

namespace Tipstaff
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new Filters.UserActivityAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Audit", "{auditType}/Audit/{id}"
                                , new
                                {
                                    controller = "Audit",
                                    action = "Audit",
                                }
                                , new
                                {
                                    auditType=@"\D{1,20}",
                                    id=@"\d{1,6}"
                                });
            routes.MapRoute("GenerateDocumentForAddressee"
                    , "{controller}/{action}/{tipstaffRecordID}/{templateID}/{solicitorID}"
                    , new
                    {
                        controller = "Template"
                      ,
                        action = "Create4"
                    }
                    , new
                    {
                        tipstaffRecordID = @"\d{1,9}"
                      , templateID = @"\d{1,5}"
                      , solicitorID = @"\d{1,4}"
                    }
                    );
            routes.MapRoute("DeleteSolicitorRoute"
                                , "TipstaffRecordSolicitor/Delete/{tipstaffRecordID}/{solicitorID}"
                                , new
                                {
                                    controller = "TipstaffRecordSolicitor"
                                  ,
                                    action = "Delete"
                                }
                                , new
                                {
                                    tipstaffRecordID = @"\d{1,9}"
                                  ,
                                    solicitorID = @"\d{1,9}"
                                }
                                );
            routes.MapRoute("SolicitorDetailRoute"
                                , "Solicitor/Details/{solicitorID}/{tipstaffRecordID}"
                                , new
                                {
                                    controller = "Solicitor"
                                  ,
                                    action = "Details"
                                }
                                , new
                                {
                                    solicitorID = @"\d{1,9}"
                                  ,
                                    tipstaffRecordID = @"\d{1,9}"
                                }
                                );
            routes.MapRoute("SolicitorFirmDetailRoute"
                                , "SolicitorFirm/Details/{solicitorFirmID}/{tipstaffRecordID}"
                                , new
                                {
                                    controller = "SolicitorFirm"
                                  ,
                                    action = "Details"
                                }
                                , new
                                {
                                    solicitorFirmID = @"\d{1,9}"
                                  ,
                                    tipstaffRecordID = @"\d{1,9}"
                                }
                                );
            routes.MapRoute("SolicitorEditRoute"
                                , "Solicitor/Edit/{solicitorID}/{tipstaffRecordID}"
                                , new
                                {
                                    controller = "Solicitor"
                                  ,
                                    action = "Edit"
                                }
                                , new
                                {
                                    solicitorID = @"\d{1,9}"
                                  ,
                                    tipstaffRecordID = @"\d{1,9}"
                                }
                                );
            routes.MapRoute("SolicitorFirmEditRoute"
                                , "SolicitoFirmr/Edit/{solicitorFirmID}/{tipstaffRecordID}"
                                , new
                                {
                                    controller = "SolicitorFirm"
                                  ,
                                    action = "Edit"
                                }
                                , new
                                {
                                    solicitorFirmID = @"\d{1,9}"
                                  ,
                                    tipstaffRecordID = @"\d{1,9}"
                                }
                                );
            routes.MapRoute("GenerateDocument"
                                , "{controller}/{action}/{tipstaffRecordID}/{templateID}"
                                , new
                                {
                                    controller = "Template"
                                  ,
                                    action = "Create"
                                }
                                , new
                                {
                                    tipstaffRecordID = @"\d{1,9}"
                                  ,
                                    templateID = @"\d{1,5}"
                                }
                                );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
              "Root",
              "",
              new { controller = "Home", action = "Index", id = "" }
            );
        }
        private class HttpContextDataStore : ServiceLayer.IUnitOfWorkDataStore
        {
            public object this[string key]
            {
                get { return HttpContext.Current.Items[key]; }
                set { HttpContext.Current.Items[key] = value; }
            }
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebFormViewEngine());
            ViewEngines.Engines.Add(new RazorViewEngine());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Database.SetInitializer(new TipstaffDBInitializer());
            ServiceLayer.UnitOfWorkHelper.CurrentDataStore = new HttpContextDataStore();

            string appYear = ConfigurationManager.AppSettings["AppYear"];
            if (DateTime.Now.Year.ToString() != appYear.ToString()) {
                ConfigurationManager.AppSettings["AppYear"] = appYear += " - " + DateTime.Now.Year.ToString();
            }
            //ConfigurationManager.AppSettings["CurServer"] = ConfigurationManager.ConnectionStrings["TipstaffDB"].ConnectionString.Split(';').First().Split('=').Last();
        }
        protected void Application_AuthenticateRequest()
        {
            //if (HttpContext.Current.User != null) Membership.GetUser(true);
        }
    }
}