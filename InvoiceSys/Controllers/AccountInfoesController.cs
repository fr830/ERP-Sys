using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;
using System.Text;
using System.Security.Cryptography;
using WebMatrix.Data;

namespace MrAng_Invoice.Controllers
{
    public class AccountInfoesController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: AccountInfoes
        public ActionResult Index(String companySearch)
        {
            ViewBag.companyName = DropDownList.CreateCompanyList();
            var accountInfo = from m in db.AccountInfo
                                   select m;
            /*var employeeOnLeave= db2.Query("SELECT a.username AS username FROM [dbo].[AccountInfoes] a INNER JOIN [dbo].[Leave_Application] b ON a.username = b.username_submitted WHERE FORMAT(CURRENT_TIMESTAMP, 'yyyyMMdd') >= FORMAT(b.leave_applied_from, 'yyyyMMdd')AND FORMAT(CURRENT_TIMESTAMP, 'yyyyMMdd') <= FORMAT(b.leave_applied_to, 'yyyyMMdd') AND b.status = 'Approve'");

            ViewBag.employeeOnLeave = employeeOnLeave;*/
            if (!string.IsNullOrEmpty(companySearch))
            {
                accountInfo = accountInfo.Where(x => x.company_name == companySearch);
            }

            return View(accountInfo);

        }

        // GET: AccountInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            if (accountInfo == null)
            {
                return HttpNotFound();
            }
            return View(accountInfo);
        }

        // GET: AccountInfoes/Create
        public ActionResult Create()
        {
            ViewBag.companyName = DropDownList.CreateCompanyList();
            ViewBag.account_role = DropDownList.CreateRoleList();
            ViewBag.userType = DropDownList.CreateUserTypeList(db2);

            return View();
        }

        // POST: AccountInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,user_full_name,user_contact_no,username,password,company_name,account_role,status,created_date,created_by,last_login,last_logout,user_type,annual_leave_bal,medical_leave_bal,email,remarks")] AccountInfo accountInfo)
        {
            ViewBag.companyName = DropDownList.CreateCompanyList();
            ViewBag.account_role = DropDownList.CreateRoleList();
            ViewBag.userType = DropDownList.CreateUserTypeList(db2);
            if (ModelState.IsValid)
            {
                CreateAccount(accountInfo);

                return RedirectToAction("Index");
            }

            return View(accountInfo);
        }

        // GET: AccountInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.companyName = DropDownList.CreateCompanyList();
            ViewBag.account_role = DropDownList.CreateRoleList();
            ViewBag.userType = DropDownList.CreateUserTypeList(db2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            if (accountInfo == null)
            {
                return HttpNotFound();
            }
            return View(accountInfo);
        }

        // POST: AccountInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,user_full_name,user_contact_no,username,password,company_name,account_role,status,created_date,created_by,last_login,last_logout,user_type,employee_id,annual_leave_bal,medical_leave_bal,email,remarks")] AccountInfo accountInfo)
        {
            ViewBag.companyName = DropDownList.CreateCompanyList();
            ViewBag.account_role = DropDownList.CreateRoleList();
            ViewBag.userType = DropDownList.CreateUserTypeList(db2);

            if (ModelState.IsValid)
            {

                db.Entry(accountInfo).State = EntityState.Modified;
                db.Entry(accountInfo).Property("password").IsModified = false;
                db.SaveChanges();
               

               // AccountInfo currentAccountInfo = db.AccountInfo.Find(accountInfo.ID);
                //accountInfo.password = currentAccountInfo.password;

                //db2.Execute("UPDATE [InvoiceSys].[dbo].[AccountInfoes] SET password = @0 WHERE id = @1", MD5Hash(accountInfo.password), accountInfo.ID);
                return RedirectToAction("Index");
            }
            return View(accountInfo);
        }

        // GET: AccountInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            if (accountInfo == null)
            {
                return HttpNotFound();
            }
            return View(accountInfo);
        }

        // POST: AccountInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            db.AccountInfo.Remove(accountInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(String password, String currentPassword)
        {
            LoginFunction lf = new LoginFunction();
            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(currentPassword))
            {
                if (lf.checkPassword(Session["username"].ToString(), currentPassword))
                {
                    db2.Execute("UPDATE AccountInfoes SET password=@0 WHERE username=@1", MD5Hash(password), Session["username"].ToString());
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.error_message = "Unable to change password, invalid password entered";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public String CheckUsername(String username)
        {
            var result= db2.QueryValue("SELECT username FROM AccountInfoes WHERE username=@0",username);
            var result2 = "false";
            if (String.IsNullOrEmpty(result))
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

        public void CreateAccount(AccountInfo accountInfo)
        {
            accountInfo.created_date = DateTime.Now;
            accountInfo.status = "Active";
            accountInfo.created_date = DateTime.Now;
            accountInfo.created_by = accountInfo.created_by == null ? Session["username"].ToString() : "";
            accountInfo.last_login = DateTime.Now;
            accountInfo.last_logout = DateTime.Now;
            accountInfo.tempPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'ACCID'");
            var employee_id = "MTL" + DateTime.Now.Year.ToString()+ running_no.ToString().PadLeft(2, '0');
            accountInfo.employee_id = employee_id;
            db.AccountInfo.Add(accountInfo);
           

            try
            {
                db.SaveChanges();
                db2.Execute("UPDATE [InvoiceSys].[dbo].[AccountInfoes] SET password = @0 WHERE id = @1", MD5Hash(accountInfo.password), accountInfo.ID);
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'ACCID'");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.IO.File.AppendAllText("D:/MVC_ERROR.log", ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }

        }



        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ActivateUser(int id)
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            accountInfo.status = "Active";
            accountInfo.reset_retry_count = 0;
            accountInfo.retry_count = 0;
            db.Entry(accountInfo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeactivateUser(int id)
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            AccountInfo accountInfo = db.AccountInfo.Find(id);
            accountInfo.status = "Inactive";
            db.Entry(accountInfo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {
                System.IO.File.AppendAllText("D:/MVC_ERROR.log", ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }

            return RedirectToAction("Index");
        }

 


    }
}
