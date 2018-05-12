using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;

namespace MrAng_Invoice.Controllers
{
    public class Employee_ManagementController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();

        // GET: Employee_Management
        public ActionResult Index(String employeeUsername, String employeeName)
        {
            ViewBag.employeeName = DropDownList.CreateEngineerNameList();
            ViewBag.employeeUsername = DropDownList.CreateEnginnerUsernameList();


            var engineerList = from m in db.AccountInfo
                               where m.user_type == "Engineer"
                               select m;

            if (!String.IsNullOrEmpty(employeeName))
            {
                engineerList = engineerList.Where(s => s.user_full_name == employeeName);
            }
            if (!String.IsNullOrEmpty(employeeUsername))
            {
                engineerList = engineerList.Where(s => s.username == employeeUsername);
            }
            return View(engineerList);
        }

        // GET: Employee_Management/Details/5
        public ActionResult Details(int? id, string dateOfReport, bool genReport)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            ViewBag.genReport = false;
            if (genReport)
            {
                ViewBag.genReport = genReport;

                WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
                if (String.IsNullOrEmpty(dateOfReport))
                {
                    dateOfReport = DateTime.Now.ToString("MM/yyyy");
                }
                ViewBag.dateOfReport = dateOfReport;

                var result = db2.Query("SELECT * FROM Employee_WorkSheet WHERE employee_id=@0 AND FORMAT(working_date,'MM/yyyy')=@1 AND approval_status !='Amended'", accountInfo.employee_id, dateOfReport);
                ViewBag.result = result;
            }
            if (accountInfo == null)
            {
                return HttpNotFound();
            }
            return View(accountInfo);
        }


        public ActionResult downloadEmployeeReportPDF(int? id, string reportDate)
        {
            AccountInfo engineer_privateInfo = db.AccountInfo.Find(id);
            ViewBag.dateOfReport = reportDate;
            return View(engineer_privateInfo);
        }

        public ActionResult downloadEmployeeReportExcel(int? id, string reportDate)
        {
            AccountInfo engineer_privateInfo = db.AccountInfo.Find(id);
            ViewBag.dateOfReport = reportDate;
            return View(engineer_privateInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
