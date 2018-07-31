using System;
using Tipstaff.Models;
using System.Xml;
using System.Text;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff
{
    public static partial class genericFunctions
    {
        /// <summary>
        /// Send in a pluralised string descriptor and the number of records
        /// </summary>
        /// <param name="toAssess">Field descriptor in plural form</param>
        /// <param name="numberofRecords">Number of records (from database?)</param>
        /// <returns></returns>
        public static string SingulariseString(string toAssess, int numberofRecords)
        {
            string returnVal = toAssess;
            if (numberofRecords == 1)
            {
                PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));
                returnVal = ps.Singularize(toAssess);
            }
            return returnVal;
        }
        public static string TypeOfTipstaffRecord(TipstaffRecord obj)
        {
            return isTipstaffRecordChildAbduction(obj) ? "ChildAbduction" : "Warrant";
        }
        //public static string TypeOfTipstaffRecord(int id)
        //{
        //    using (TipstaffDB tempDB = new TipstaffDB())
        //    {
        //        TipstaffRecord obj = tempDB.TipstaffRecord.Find(id);
        //        return isTipstaffRecordChildAbduction(obj) ? "ChildAbduction" : "Warrant";
        //    }
        //}
        public static string TypeOfTipstaffRecord(string id)
        {
            ITipstaffRecordRepository _repository = new TipstaffRecordRepository(new DynamoAPI<Services.DynamoTables.TipstaffRecord>(), new AuditEventRepository(new DynamoAPI<Services.DynamoTables.AuditEvent>(), new GuidGenerator()));
            var t = _repository.GetEntityByHashKey(id);

            return t.Discriminator; 

        }

        public static bool isTipstaffRecordChildAbduction(TipstaffRecord obj)
        {
            return obj.Discriminator=="ChildAbduction";
        }

        public static byte[] ConvertToBytes(XmlDocument doc)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] docAsBytes = encoding.GetBytes(doc.OuterXml);
            return docAsBytes;
        }
        public static string GetLowestError(Exception ex)
        {
            while (ex.Message.Contains("inner ex"))
            {
                return GetLowestError(ex.InnerException);
            };
            return ex.Message;
        }

    }
}