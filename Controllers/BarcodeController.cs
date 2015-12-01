using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Text;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    public class BarcodeController : Controller
    {
        //
        // GET: /Barcode/
        public ActionResult GenerateBarCode(string text)
        {
            BarcodeLib.Barcode bob = new BarcodeLib.Barcode();
            bob.IncludeLabel = false;
            bob.Encode(BarcodeLib.TYPE.CODE128B ,text, Color.Black, Color.White,150,30);
            var returnStream = new MemoryStream();
            bob.SaveImage(returnStream, BarcodeLib.SaveTypes.PNG);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
            //<img src="<%: Url.Action("GenerateBarCode", "Barcode", new { text = Model.text }) %> //use on page
        }
    }
}
