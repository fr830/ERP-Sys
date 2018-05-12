using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;
using WebMatrix.Data;

namespace MrAng_Invoice.Controllers
{
    public class Customer_PrivateInfoController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
        // GET: Customer_PrivateInfo
        public ActionResult Index(string customerID , string companyName)
        {

            var customer_PrivateInfo = from m in db.Customer_PrivateInfo
                                    select m;

            ViewBag.companyName = DropDownList.CreateCompanyList();


            ViewBag.companyID = DropDownList.CreateCompanyID();

            if (!String.IsNullOrEmpty(customerID))
            {
                customer_PrivateInfo = customer_PrivateInfo.Where(s => s.customer_id.Contains(customerID));
            }

            if (!string.IsNullOrEmpty(companyName))
            {
                customer_PrivateInfo = customer_PrivateInfo.Where(x => x.company_name == companyName);
            }
            if (!SAPermissionChecker.isSuperAdmin(Session["permission"].ToString()))
            {
                String myCompanyName = LoginFunction.getCompanyByUsername(Session["username"].ToString());
                customer_PrivateInfo = customer_PrivateInfo.Where(x => x.company_name == myCompanyName);
            }
            return View(customer_PrivateInfo);

        }

        // GET: Customer_PrivateInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_PrivateInfo customer_PrivateInfo = db.Customer_PrivateInfo.Find(id);
            if (customer_PrivateInfo == null)
            {
                return HttpNotFound();
            }
            return View(customer_PrivateInfo);
        }

        // GET: Customer_PrivateInfo/Create
        public ActionResult Create()
        {

            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'CUSTID'");
            var customer_id = "MTL" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            Customer_PrivateInfo customer_PrivateInfo = new Customer_PrivateInfo();
            customer_PrivateInfo.customer_id = customer_id;
            return View(customer_PrivateInfo);
        }

        // POST: Customer_PrivateInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customer_id,company_name,company_reg_no,payment_term,company_gst_no,company_address,company_fax_no,company_tel_no,company_person_incharge1,company_department1,company_position1,company_email1,company_tel_no1,company_person_incharge2,company_department2,company_position2,company_email2,company_tel_no2,company_person_incharge3,company_department3,company_position3,company_email3,company_tel_no3,state")] Customer_PrivateInfo customer_PrivateInfo)
        {
            if (ModelState.IsValid)
            {

                CreateCompanyProfile(customer_PrivateInfo);
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'CUSTID'");
                return RedirectToAction("Index");
            }
            

            return View(customer_PrivateInfo);
        }

        // GET: Customer_PrivateInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_PrivateInfo customer_PrivateInfo = db.Customer_PrivateInfo.Find(id);
            if (customer_PrivateInfo == null)
            {
                return HttpNotFound();
            }
            return View(customer_PrivateInfo);
        }

        // POST: Customer_PrivateInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,customer_id,company_name,company_reg_no,payment_term,company_gst_no,company_address,company_fax_no,company_tel_no,company_person_incharge1,company_department1,company_position1,company_email1,company_tel_no1,company_person_incharge2,company_department2,company_position2,company_email2,company_tel_no2,company_person_incharge3,company_department3,company_position3,company_email3,company_tel_no3,created_date,status,state")] Customer_PrivateInfo customer_PrivateInfo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(customer_PrivateInfo).State = EntityState.Modified;
                }
                catch
                {
                    db.Entry(customer_PrivateInfo).State = EntityState.Added;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer_PrivateInfo);
        }

        // GET: Customer_PrivateInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_PrivateInfo customer_PrivateInfo = db.Customer_PrivateInfo.Find(id);
            if (customer_PrivateInfo == null)
            {
                return HttpNotFound();
            }
            return View(customer_PrivateInfo);
        }

        // POST: Customer_PrivateInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Customer_PrivateInfo customer_PrivateInfo = db.Customer_PrivateInfo.Find(id);
            db.Customer_PrivateInfo.Remove(customer_PrivateInfo);
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

        public void CreateCompanyProfile(Customer_PrivateInfo customer_PrivateInfo)
        {
            customer_PrivateInfo.created_date = DateTime.Now;
            customer_PrivateInfo.status = "Active";
            db.Customer_PrivateInfo.Add(customer_PrivateInfo);
            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                System.IO.File.AppendAllText("D:/MVC_ERROR.log", ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }

        }

        public ActionResult customerReport(int? id, string dateOfReport, bool genReport, bool isChargeable)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_PrivateInfo customerInfo = db.Customer_PrivateInfo.Find(id);
            ViewBag.genReport = false;
            ViewBag.isChargeable = isChargeable;
            if (genReport)
            {
                ViewBag.genReport = genReport;
                WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
                if (String.IsNullOrEmpty(dateOfReport))
                {
                    dateOfReport = DateTime.Now.ToString("MM/yyyy");
                }
                ViewBag.dateOfReport = dateOfReport;

                if (isChargeable)
                {
                    var result = db2.Query("SELECT * FROM Customer_WorkSheet WHERE client_served=@0 AND FORMAT(working_date,'MM/yyyy')=@1 AND approval_status !='Amended' AND isChargeable = 1", customerInfo.company_name, dateOfReport);
                    ViewBag.result = result;

                }
                else
                {
                    var result = db2.Query("SELECT * FROM Customer_WorkSheet WHERE client_served=@0 AND FORMAT(working_date,'MM/yyyy')=@1 AND approval_status !='Amended'", customerInfo.company_name, dateOfReport);
                    ViewBag.result = result;
                }


            }
            if (customerInfo == null)
            {
                return HttpNotFound();
            }
            return View(customerInfo);
        }

        public ActionResult downloadCustomerReportPDF(int? id, string reportDate, bool isChargeable)
        {
            Customer_PrivateInfo customer_privateinfo = db.Customer_PrivateInfo.Find(id);
            ViewBag.dateOfReport = reportDate;
            ViewBag.isChargeable = isChargeable;
            return View(customer_privateinfo);
        }

        public ActionResult downloadCustomerReportExcel(int? id, string reportDate, bool isChargeable)
        {
            Customer_PrivateInfo customer_privateinfo = db.Customer_PrivateInfo.Find(id);
            ViewBag.dateOfReport = reportDate;
            ViewBag.isChargeable = isChargeable;
            return View(customer_privateinfo);
        }

    }
}
