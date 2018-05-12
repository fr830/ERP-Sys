using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace MrAng_Invoice.Controllers
{
    public class LoginController : Controller
    {
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");
        // GET: Login
        public ActionResult Index(String username, String password)
        {
            LoginFunction lf = new LoginFunction();
            ViewBag.message = "";
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
            if (!lf.checkUsername(username))
            { // Check Validity of username
                ViewBag.message = "Invalid Username, does not exist!";
            }
            else if (!lf.checkUserRetryCount(username))
            { // Check user retry count
                ViewBag.message = "Exceeded Retry Count,Account Locked! Please Contact System Administrator";
            }
            else if (!lf.checkUserResetRetryCount(username))
            { // Check user reset retry count
                ViewBag.message = "There is a suspicious activity in this account! Please Contact System Administrator";
            }
                else if (!lf.checkPassword(username, password))
            { // Check username and password match or not
                ViewBag.message = "Either Password or Username is Wrong!";
                lf.increaseRetryCount(username);
            }
            else if (!lf.checkUserStatus(username, password))
            { // Check user active or not
                ViewBag.message = "Account Locked! Please Contact System Administrator";
            }
            else
            {
                    assignSession(username);
                    return RedirectToAction("Index", "Home");

            }
            }
            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ForgotPassword(String username)
        {
            dynamic isExist = "";
            LoginFunction lf = new LoginFunction();
            if (!String.IsNullOrEmpty(username))
            {
                isExist = db2.QuerySingle("SELECT username, email, tempPassword FROM AccountInfoes WHERE username=@0", username);
            }
            if(isExist == null)
            {
                ViewBag.message = "Username does not exist!";
                return View();
            }
            else
            {
                    if (!String.IsNullOrEmpty(username))
                    {
                        if (lf.checkUserResetRetryCount(username))
                        {
                            try
                            {
                                SendEmail.sendForgotPassword(isExist.email, username, isExist.tempPassword);
                            }
                            catch (Exception ex)
                            {
                                System.IO.File.AppendAllText("D:/Test.log", ex.ToString() + "\r\n");
                            }

                            return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.message = "There is a suspicious activity in this account! Please Contact System Administrator";
                        return View();
                    }
                }
                



            }
            return View();
        }

        public ActionResult ResetPassword(string username,string tempPassword, string newPassword)
        {
            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(tempPassword) && !String.IsNullOrEmpty(newPassword))
            {
                LoginFunction lf = new LoginFunction();
                if (lf.checkUsername(username))
                {
                    if (lf.checkUserResetRetryCount(username))
                    {
                        var result = db2.QuerySingle("SELECT username FROM AccountInfoes WHERE tempPassword=@0 AND username=@1", tempPassword, username);

                        if (result != null)
                        {
                            db2.Execute("UPDATE AccountInfoes SET password = @0, tempPassword = @1, status='Active', reset_retry_count=0, retry_count = 0 WHERE username=@2", MD5Hash(newPassword), Guid.NewGuid().ToString("N").Substring(0, 8), username);
                            assignSession(username);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            lf.increaseResetRetryCount(username);
                            ViewBag.message = "Invalid temporary password!";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.message = "There is a suspicious activity in this account! Please Contact System Administrator";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Username does not exist!";
                    return View();
                }



            }
            return View();
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

        public void assignSession(string username)
        {
            LoginFunction lf = new LoginFunction();
            Session["username"] = username;
            Session["name"] = LoginFunction.getNamebyUsername(username);
            Session["role"] = LoginFunction.getRoleByUsername(username);
            Session["permission"] = db2.QueryValue("SELECT TOP 1 ums_value FROM UMS WHERE role_name=@0", Session["role"].ToString());
            Session["company_name"] = db2.QueryValue("SELECT TOP 1 value from tbl_system_parameters WHERE name = 'company_name'");
            lf.ResetRetryCount(username);
        }

    }
}
