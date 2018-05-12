using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrAng_Invoice.Controllers
{
    public class HomeController : Controller
    {
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       /* public void DownloadUserManual()
        {
            
            var FilePath = db2.QueryValue("SELECT TOP 1 value FROM tbl_system_parameters WHERE name = 'user_manual_path'");
            //octet-stream = arbitrary binary data
            Response.AddHeader("Content-Type", "Application/octet-stream");
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(FilePath));
            Response.TransmitFile(FilePath);
            Response.End();
        } */
    }
    
}