using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System;
using System.Web.UI;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class PassportController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ICloudWatchLogger _logger;

        public PassportController(ICloudWatchLogger logger)
        {
            _logger = logger;
        }
        
        public PartialViewResult ListPassportsByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListDocumentsByTipstaffRecord model = new ListDocumentsByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Documents = w.Documents.OrderByDescending(d => d.createdOn).ToXPagedList<Document>(page ?? 1, 8);
            return PartialView("_ListDocumentsByRecord", model);
        }

     }
}
