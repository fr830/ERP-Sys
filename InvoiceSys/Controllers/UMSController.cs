using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;

namespace MrAng_Invoice.Views
{
    public class UMSController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
        // GET: UMS
        public ActionResult Index()
        {
            return View(db.UMS.ToList());
        }

        // GET: UMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMS uMS = db.UMS.Find(id);
            if (uMS == null)
            {
                return HttpNotFound();
            }
            return View(uMS);
        }

        // GET: UMS/Create
        public ActionResult Create()
        {
            var result = db2.Query("SELECT * FROM tbl_module");
            ViewBag.module = result.ToList<dynamic>();
            return View();
        }

        // POST: UMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ums_id,ums_value,created_by,created_date,status,role_name")] UMS uMS, string[] permission)
        {
            var result = db2.Query("SELECT * FROM tbl_module");
            ViewBag.module = result.ToList<dynamic>();
            if (ModelState.IsValid)
            {
                try
                {
                    for (int i = 0; i < permission.Length; i++)
                    {
                        uMS.ums_value += permission[i] + ",";
                    }
                }
                catch
                {

                }

                uMS.created_by = uMS.created_by == null ? Session["name"].ToString() : "";
                uMS.created_date = DateTime.Now;
                uMS.status = "Active";
                db.UMS.Add(uMS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uMS);
        }

        // GET: UMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = db2.Query("SELECT  * FROM tbl_module");
            ViewBag.module = result.ToList<dynamic>();
            UMS uMS = db.UMS.Find(id);
            if (uMS == null)
            {
                return HttpNotFound();
            }
            return View(uMS);
        }

        // POST: UMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ums_id,ums_value,created_by,created_date,status,role_name")] UMS uMS, string[] permission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    for (int i = 0; i < permission.Length; i++)
                    {
                        uMS.ums_value += permission[i] + ",";
                    }
                }
                catch
                {

                }
                uMS.created_by = uMS.created_by == null ? Session["name"].ToString() : "";
                uMS.created_date = DateTime.Now;
                uMS.status = "Active";
                db.Entry(uMS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uMS);
        }

        // GET: UMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMS uMS = db.UMS.Find(id);
            if (uMS == null)
            {
                return HttpNotFound();
            }
            return View(uMS);
        }

        // POST: UMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UMS uMS = db.UMS.Find(id);
            db.UMS.Remove(uMS);
            db.SaveChanges();
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
