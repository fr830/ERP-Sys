using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using WebMatrix.Data;



public class SendEmail
{


    public static void sendForgotPassword(string email, string username, string tempPassword)
    {
        Database db = Database.Open("InvoiceSysDBContext");
        MailMessage m = new MailMessage();
        SmtpClient sc = new SmtpClient();

            var resetLink = db.QuerySingle("SELECT value FROM tbl_system_parameters WHERE name='reset_pw_link'");
            var company_email = db.QueryValue("SELECT value FROM tbl_system_parameters WHERE name='company_email'");
            var company_email_password = db.QueryValue("SELECT value FROM tbl_system_parameters WHERE name='company_email_password'");


            m.From = new MailAddress(company_email, "ERP Forgot Password Assistance");
            m.To.Add(new MailAddress(email, "Sent To"));

            //m.CC.Add(new MailAddress("CC@yahoo.com", "Display name CC"));
            //similarly BCC
            m.Subject = "Temporary Password for ERP System";
            m.IsBodyHtml = true;

            var bodyMessage = "Dear <b>" + username + "</b> ,";
            bodyMessage += "</br></br>";
            bodyMessage += "Here is your tempory password, please login to <a href=\"" + resetLink.value + "\">here</a> to change your password";
            bodyMessage += "</br></br>";
            bodyMessage += "<b>Username:</b> "+username;
            bodyMessage += "</br></br>";
            bodyMessage += "<b>Password:</b> "+ tempPassword;
            bodyMessage += "</br></br>";
            bodyMessage += "Best Regards,";
            bodyMessage += "</br></br>";
            bodyMessage += "ERP System Customer Service";
            m.Body = bodyMessage;


            sc.Host = "smtp.gmail.com";
            sc.Port = 587;
            sc.Credentials = new System.Net.NetworkCredential(company_email, company_email_password);
            sc.EnableSsl = true;
            sc.Send(m);


    }
}
