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
    public class HolidayTablesController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: HolidayTables
        public ActionResult Index()
        {
            return View(db.HolidayTable.ToList());
        }

        // GET: HolidayTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTable holidayTable = db.HolidayTable.Find(id);
            if (holidayTable == null)
            {
                return HttpNotFound();
            }
            return View(holidayTable);
        }

        // GET: HolidayTables/Create
        public ActionResult Create()
        {
            HolidayTable holidayTable = new HolidayTable();
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'HOLIID'");
            var holiday_id = "HOLI" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            holidayTable.referenceKey = holiday_id;
            return View(holidayTable);
        }

        // POST: HolidayTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,state,referenceKey,year")] HolidayTable holidayTable, String[] holidayDate, String year)
        {
            if (ModelState.IsValid)
            {
                db.HolidayTable.Add(holidayTable);
                db.SaveChanges();
                var begin = DateTime.Parse(DateTime.Now.Year.ToString() + "-01-01");
                var end = DateTime.Parse(DateTime.Now.Year.ToString() + "-12-31");
                for (DateTime date = begin; date < end; date = date.AddDays(1))
                {
                    db2.Execute("INSERT INTO HolidayTableDetails VALUES(@0,@1,@2,@3,@4)",date.ToString("dd/MM"),year,holidayTable.state,0,holidayTable.referenceKey);
                }
                for(int i=0; i < holidayDate.Length; i++)
                {
                    db2.Execute("UPDATE HolidayTableDetails SET isHoliday = 1 WHERE holidayDate=@0 AND state=@1 AND year=@2",holidayDate[i],holidayTable.state,year);
                }
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'HOLIID'");
                return RedirectToAction("Index");
            }

            return View(holidayTable);
        }

        // GET: HolidayTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTable holidayTable = db.HolidayTable.Find(id);
            var holidayResult = db2.Query("SELECT holidayDate FROM HolidayTableDetails WHERE referenceKey=@0 AND isHoliday=1",holidayTable.referenceKey);
            List<String> holidayDateList = new List<String>();
            foreach(var result in holidayResult)
            {
                holidayDateList.Add(result.holidayDate);
            }
            ViewBag.holidayResult = holidayDateList;
            if (holidayTable == null)
            {
                return HttpNotFound();
            }

            return View(holidayTable);
        }

        // POST: HolidayTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,state,referenceKey,year")] HolidayTable holidayTable, String[] holidayDate, String year)
        {
            var holidayResult = db2.Query("SELECT holidayDate FROM HolidayTableDetails WHERE referenceKey=@0 AND isHoliday=1", holidayTable.referenceKey);
            ViewBag.holidayResult = holidayResult.Cast<dynamic>().ToList();
            if (ModelState.IsValid)
            {
                db.Entry(holidayTable).State = EntityState.Modified;
                db.SaveChanges();
                db2.Execute("UPDATE HolidayTableDetails SET state = @0, year = @1 WHERE referenceKey = @2", holidayTable.state, year, holidayTable.referenceKey);
                db2.Execute("UPDATE HolidayTableDetails SET isHoliday = 0 WHERE state=@0 AND year=@1", holidayTable.state, year);
                for (int i = 0; i < holidayDate.Length; i++)
                {
                    db2.Execute("UPDATE HolidayTableDetails SET isHoliday = 1 WHERE holidayDate=@0 AND state=@1 AND year=@2", holidayDate[i], holidayTable.state, year);
                }
                return RedirectToAction("Index");
            }

            return View(holidayTable);
        }

        // GET: HolidayTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayTable holidayTable = db.HolidayTable.Find(id);
            if (holidayTable == null)
            {
                return HttpNotFound();
            }
            return View(holidayTable);
        }

        // POST: HolidayTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolidayTable holidayTable = db.HolidayTable.Find(id);
            db.HolidayTable.Remove(holidayTable);
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
