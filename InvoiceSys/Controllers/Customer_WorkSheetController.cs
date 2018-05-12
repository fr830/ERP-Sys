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
    public class Customer_WorkSheetController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: Customer_WorkSheet
        public ActionResult Index(string status, string customerSearch, DateTime? working_date)
        {
            ViewBag.company = DropDownList.CreateCompanyList();
            var companyWorkSheet = from m in db.Customer_Worksheet
                                   where m.approval_status != "Amended"
                                   select m;

            if (String.IsNullOrEmpty(status))
            {
                companyWorkSheet = companyWorkSheet.Where(s => s.approval_status.Contains("Pending"));
            }

            if (!String.IsNullOrEmpty(status) && status != "All")
            {
                companyWorkSheet = companyWorkSheet.Where(s => s.approval_status.Contains(status));
            }

            if (!string.IsNullOrEmpty(customerSearch))
            {
                companyWorkSheet = companyWorkSheet.Where(x => x.client_served == customerSearch);
            }
            if (working_date != null)
            {
                companyWorkSheet = companyWorkSheet.Where(c => c.working_date.Month == working_date.Value.Month);
                companyWorkSheet = companyWorkSheet.Where(c => c.working_date.Year == working_date.Value.Year);
            }

            if (!SAPermissionChecker.isSuperAdmin(Session["permission"].ToString()))
            {
                String companyName = LoginFunction.getCompanyByUsername(Session["username"].ToString());
                companyWorkSheet = companyWorkSheet.Where(x => x.client_served == companyName);
            }
                return View(companyWorkSheet);
        }

        // GET: Customer_WorkSheet/Details/5
        public ActionResult Details(int? id, string needLayout)
        {
            if (!String.IsNullOrEmpty(needLayout))
            {
                ViewBag.layout = "No";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_WorkSheet customer_WorkSheet = db.Customer_Worksheet.Find(id);
            if (customer_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(customer_WorkSheet);
        }

        // GET: Customer_WorkSheet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer_WorkSheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customer_worksheet_ID,ticket_id,employee_ID,employee_worksheet_ID,working_date,time_in,time_out,total_working_hour,client_served,submission_date,approval_date,approval_status,job_location,job_description,followup_by,requested_by,remarks,employee_name,isChargeable")] Customer_WorkSheet customer_WorkSheet)
        {
            if (ModelState.IsValid)
            {
                db.Customer_Worksheet.Add(customer_WorkSheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer_WorkSheet);
        }

        // GET: Customer_WorkSheet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_WorkSheet customer_WorkSheet = db.Customer_Worksheet.Find(id);
            if (customer_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(customer_WorkSheet);
        }

        // POST: Customer_WorkSheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "customer_worksheet_ID,ticket_id,employee_ID,employee_worksheet_ID,working_date,time_in,time_out,total_working_hour,client_served,submission_date,approval_date,approval_status,job_location,job_description,followup_by,requested_by,remarks,employee_name,isChargeable")] Customer_WorkSheet customer_WorkSheet)
        {
            if (ModelState.IsValid)
            {
                Customer_WorkSheet cws = db.Customer_Worksheet.Find(customer_WorkSheet.customer_worksheet_ID);
                cws.approval_status = "Amended";
                db.Entry(cws).State = EntityState.Modified;
                db.SaveChanges();
                CreateTaskList(customer_WorkSheet);
                return RedirectToAction("Index");
            }
            return View(customer_WorkSheet);
        }

        // GET: Customer_WorkSheet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_WorkSheet customer_WorkSheet = db.Customer_Worksheet.Find(id);
            if (customer_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(customer_WorkSheet);
        }

        // POST: Customer_WorkSheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer_WorkSheet customer_WorkSheet = db.Customer_Worksheet.Find(id);
            db.Customer_Worksheet.Remove(customer_WorkSheet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ApproveWorkSheet(int? id)
        {
            Customer_WorkSheet cws = db.Customer_Worksheet.Find(id);
            cws.approval_status = "Approve";
            cws.approval_date = DateTime.Now;
            db.Entry(cws).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RejectWorkSheet(int? id)
        {
            Customer_WorkSheet cws = db.Customer_Worksheet.Find(id);
            cws.approval_status = "Reject";
            cws.approval_date = DateTime.Now;
            db.Entry(cws).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CompareEmployeeVersion(String ticket_id)
        {
            var employee_worksheet = db2.QuerySingle("SELECT TOP 1 * FROM Employee_WorkSheet WHERE ticket_id = @0 AND approval_status !='Amended' ORDER BY worksheet_id DESC",ticket_id);
            var customer_worksheet = db2.QuerySingle("SELECT TOP 1 * FROM Customer_WorkSheet WHERE ticket_id = @0 AND approval_status !='Amended' ORDER BY customer_worksheet_id DESC", ticket_id);
            ViewBag.employee_worksheet = employee_worksheet;
            ViewBag.customer_worksheet = customer_worksheet;
            return View();
        }


        public void CreateTaskList(Customer_WorkSheet customer_worksheet)
        {
            customer_worksheet.submission_date = DateTime.Now;
            customer_worksheet.approval_status = "Pending";
            customer_worksheet.working_date_type = CheckHolidayTable.isHoliday(customer_worksheet.client_served, customer_worksheet.working_date);
            db.Customer_Worksheet.Add(customer_worksheet);
            db.SaveChanges();
        }

        public String CheckWorkingDayType(DateTime date)
        {
            var result = db2.QueryValue("SELECT isHoliday FROM HolidayTableDetails WHERE holidayDate = @0 AND year=@1", date.ToString("dd/MM"), date.Year);
            var result2 = "false";

            if (result == false || result == null)
            {
                ViewBag.result = "False";
                result2 = "false";
            }
            else
            {
                ViewBag.result = "True";
                result2 = "true";
            }
            return result2;
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
