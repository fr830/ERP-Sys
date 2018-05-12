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
    public class Leave_ApplicationController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: Leave_Application
        public ActionResult Index(String leave_applied_from, String leave_applied_to, String status)
        {
            var leave_application = from m in db.Leave_Application
                                  where m.status != "Amended"
                                  select m;

            if (!String.IsNullOrEmpty(leave_applied_from))
            {
                DateTime leave_from = Convert.ToDateTime(leave_applied_from);
                leave_application = leave_application.Where(s => s.leave_applied_from >= leave_from);
            }
            if (!String.IsNullOrEmpty(leave_applied_to))
            {
                DateTime leave_to = Convert.ToDateTime(leave_applied_to);
                leave_application = leave_application.Where(s => s.leave_applied_to <= leave_to);
            }
            if (!String.IsNullOrEmpty(status))
            {
                leave_application = leave_application.Where(s => s.status == status);
            }

            return View(leave_application);
        }

        // GET: Leave_Application/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            if (leave_Application == null)
            {
                return HttpNotFound();
            }
            return View(leave_Application);
        }

        // GET: Leave_Application/Create
        public ActionResult Create()
        {
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'LEAVEID'");
            var leave_no = " MTLLEAVE-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            ViewBag.leave_no = leave_no;
            return View();
        }

        // POST: Leave_Application/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,leave_applied_from,leave_applied_to,no_days_applied,status,type_of_leave,reason,submitted_by,submitted_date,approved_by,approval_date,approval_reason,username_submitted,leave_ticket_id")] Leave_Application leave_Application)
        {
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'LEAVEID'");
            var leave_no = " MTLLEAVE-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            ViewBag.leave_no = leave_no;
            leave_Application.submitted_by = Session["name"].ToString();
            leave_Application.submitted_date = DateTime.Now;
            leave_Application.approval_date = DateTime.Now;
            leave_Application.status = "Pending";
            leave_Application.approved_by = "System";
            leave_Application.username_submitted = Session["username"].ToString();

            if (ModelState.IsValid && LeaveFunction.sufficientLeave(leave_Application.username_submitted,leave_Application.no_days_applied,leave_Application.type_of_leave))
            {
                db.Leave_Application.Add(leave_Application);
                db.SaveChanges();
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'LEAVEID'");
                return RedirectToAction("Index");
            }

            return View(leave_Application);
        }

        // GET: Leave_Application/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            if (leave_Application == null)
            {
                return HttpNotFound();
            }
            return View(leave_Application);
        }

        // POST: Leave_Application/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,leave_applied_from,leave_applied_to,no_days_applied,status,type_of_leave,reason,submitted_by,submitted_date,approved_by,approval_date,approval_reason,username_submitted,leave_ticket_id")] Leave_Application leave_Application)
        {
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'LEAVEID'");
            var leave_no = " MTLLEAVE-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            ViewBag.leave_no = leave_no;
            leave_Application.submitted_by = Session["name"].ToString();
            leave_Application.submitted_date = DateTime.Now;
            leave_Application.approval_date = DateTime.Now;
            leave_Application.status = "Pending";
            leave_Application.approved_by = "System";
            leave_Application.username_submitted = Session["username"].ToString();


            if (ModelState.IsValid && LeaveFunction.sufficientLeave(leave_Application.username_submitted, leave_Application.no_days_applied, leave_Application.type_of_leave))
            {
                db2.Execute("UPDATE Leave_Application SET status = 'Amended' WHERE ID = @0", leave_Application.ID);
                db.Leave_Application.Add(leave_Application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leave_Application);
        }

        // GET: Leave_Application/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            if (leave_Application == null)
            {
                return HttpNotFound();
            }
            return View(leave_Application);
        }

        // POST: Leave_Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            db.Leave_Application.Remove(leave_Application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult ApproveLeave(int id)
        {
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            leave_Application.status = "Approve";
            leave_Application.approval_date = DateTime.Now;
            leave_Application.approved_by = Session["username"].ToString();
            db.Entry(leave_Application).State = EntityState.Modified;
            db.SaveChanges();
            LeaveFunction.reduceLeave(id,leave_Application.no_days_applied,leave_Application.type_of_leave,leave_Application.username_submitted);
            return RedirectToAction("Index");
        }

        public ActionResult RejectLeave(int? leave_id, String remarks)
        {
            Leave_Application leave_Application = db.Leave_Application.Find(leave_id);
            leave_Application.status = "Reject";
            leave_Application.approved_by = Session["username"].ToString();
            leave_Application.approval_reason = remarks;
            leave_Application.approval_date = DateTime.Now;
            db.Entry(leave_Application).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult WithdrawLeave(int id)
        {
            Leave_Application leave_Application = db.Leave_Application.Find(id);
            leave_Application.status = "Withdraw";
            leave_Application.approved_by = Session["username"].ToString();
            leave_Application.approval_date = DateTime.Now;
            db.Entry(leave_Application).State = EntityState.Modified;
            db.SaveChanges();
            LeaveFunction.addLeave(id, leave_Application.no_days_applied, leave_Application.type_of_leave, leave_Application.username_submitted);
            return RedirectToAction("Index");
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
