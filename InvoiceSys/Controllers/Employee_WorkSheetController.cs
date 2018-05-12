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
    public class Employee_WorkSheetController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: Employee_WorkSheet
        public ActionResult Index(string status, string customerSearch, DateTime? working_date)
        {
            ViewBag.company = DropDownList.CreateCompanyList();
            var employeeWorkSheet = from m in db.Employee_WorkSheet
                                    where m.approval_status != "Amended"
                                    select m;

            if (String.IsNullOrEmpty(status))
            {
                employeeWorkSheet = employeeWorkSheet.Where(s => s.approval_status.Contains("Pending"));
            }
            if (!String.IsNullOrEmpty(status) && status != "All")
            {
                employeeWorkSheet = employeeWorkSheet.Where(s => s.approval_status.Contains(status));
            }

            if (!string.IsNullOrEmpty(customerSearch))
            {
                employeeWorkSheet = employeeWorkSheet.Where(x => x.client_served == customerSearch);
            }

            if (working_date != null)
            {
                employeeWorkSheet = employeeWorkSheet.Where(c => c.working_date.Month == working_date.Value.Month);
                employeeWorkSheet = employeeWorkSheet.Where(c => c.working_date.Year == working_date.Value.Year);
            }

            if (!SAPermissionChecker.isSuperAdmin(Session["permission"].ToString()))
            {
                String companyName = LoginFunction.getCompanyByUsername(Session["username"].ToString());
                employeeWorkSheet = employeeWorkSheet.Where(x => x.client_served == companyName);
            }
            return View(employeeWorkSheet);
        }

        // GET: Employee_WorkSheet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_WorkSheet employee_WorkSheet = db.Employee_WorkSheet.Find(id);
            if (employee_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(employee_WorkSheet);
        }

        // GET: Employee_WorkSheet/Create
        public ActionResult Create()
        {
            ViewBag.company = DropDownList.CreateCompanyList();
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'TASKID'");
            var ticket_ID = "T" + (DateTime.Now.Year%100).ToString() + running_no.ToString().PadLeft(4, '0');
            Employee_WorkSheet employee_WorkSheet = new Employee_WorkSheet();
            employee_WorkSheet.ticket_ID = ticket_ID;
            return View(employee_WorkSheet);
        }

        // POST: Employee_WorkSheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "worksheet_ID,employee_ID,time_in,time_out,client_served,submission_date,approval_date,approval_status,total_working_hour,job_location,job_description,followup_by,requested_by,working_date,remarks,ticket_id,employee_name,isChargeable")] Employee_WorkSheet employee_WorkSheet)
        {
            ViewBag.company = DropDownList.CreateCompanyList();
            if (ModelState.IsValid)
            {
                CreateTaskList(employee_WorkSheet);
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'TASKID'");
                return RedirectToAction("Index");
            }

            return View(employee_WorkSheet);
        }

        // GET: Employee_WorkSheet/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.company=DropDownList.CreateCompanyList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_WorkSheet employee_WorkSheet = db.Employee_WorkSheet.Find(id);
            if (employee_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(employee_WorkSheet);
        }

        // POST: Employee_WorkSheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "worksheet_ID,employee_ID,time_in,time_out,client_served,submission_date,approval_date,approval_status,total_working_hour,job_location,job_description,followup_by,requested_by,working_date,remarks,ticket_id,employee_name,isChargeable")] Employee_WorkSheet employee_WorkSheet)
        {
            ViewBag.company = DropDownList.CreateCompanyList();
            if (ModelState.IsValid)
            {
                Employee_WorkSheet ews = db.Employee_WorkSheet.Find(employee_WorkSheet.worksheet_ID);
                ews.approval_status = "Amended";
                db.Entry(ews).State = EntityState.Modified;
                db.SaveChanges();
                CreateTaskList(employee_WorkSheet);
                return RedirectToAction("Index");
            }

            return View(employee_WorkSheet);

        }

        // GET: Employee_WorkSheet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_WorkSheet employee_WorkSheet = db.Employee_WorkSheet.Find(id);
            if (employee_WorkSheet == null)
            {
                return HttpNotFound();
            }
            return View(employee_WorkSheet);
        }

        // POST: Employee_WorkSheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee_WorkSheet employee_WorkSheet = db.Employee_WorkSheet.Find(id);
            db.Employee_WorkSheet.Remove(employee_WorkSheet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ApproveWorkSheet(int? id)
        {
            Employee_WorkSheet ews = db.Employee_WorkSheet.Find(id);
            ews.approval_status = "Approve";
            ews.approval_date = DateTime.Now;
            db.Entry(ews).State = EntityState.Modified;
            db.SaveChanges();
            db2.Execute("INSERT INTO [InvoiceSys].[dbo].Customer_WorkSheet ([employee_ID],[employee_worksheet_ID],[working_date],[time_in],[time_out],[total_working_hour],[client_served],[submission_date],[approval_date],[approval_status],[job_location],[job_description],[followup_by],[requested_by],[remarks],[ticket_id],[working_date_type],[employee_name],[isChargeable]) SELECT[employee_ID],[worksheet_ID],[working_date],[time_in],[time_out],[total_working_hour],[client_served], submission_date,[approval_date],'Pending',[job_location],[job_description],[followup_by],[requested_by],[remarks],[ticket_id],[working_date_type],[employee_name],[isChargeable] FROM [InvoiceSys].[dbo].[Employee_WorkSheet] WHERE worksheet_ID = @0", id);
            return RedirectToAction("Index");
        }

        public ActionResult RejectWorkSheet(int? id)
        {
            Employee_WorkSheet ews = db.Employee_WorkSheet.Find(id);
            ews.approval_status = "Reject";
            ews.approval_date = DateTime.Now;
            db.Entry(ews).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void CreateTaskList(Employee_WorkSheet employee_WorkSheet)
        {
            employee_WorkSheet.submission_date = DateTime.Now;
            employee_WorkSheet.approval_status = "Pending";
            employee_WorkSheet.working_date_type = CheckHolidayTable.isHoliday(employee_WorkSheet.client_served, employee_WorkSheet.working_date);
            db.Employee_WorkSheet.Add(employee_WorkSheet);

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

        public ActionResult EmployeeSearch_PopUp(String username, String name)
        {
            ViewBag.usernameList = DropDownList.CreateEnginnerUsernameList();
            ViewBag.nameList = DropDownList.CreateEngineerNameList();

            var employeeList = from m in db.AccountInfo
                               where m.user_type == "Engineer" &&
                               m.status == "Active"
                               select m;
            if (!String.IsNullOrEmpty(username))
            {
                employeeList = employeeList.Where(s => s.username == username);
            }
            if (!String.IsNullOrEmpty(name))
            {
                employeeList = employeeList.Where(s => s.user_full_name == name);
            }
            return View(employeeList);
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
