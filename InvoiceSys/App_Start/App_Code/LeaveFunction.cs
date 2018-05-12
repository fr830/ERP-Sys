using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;
using Microsoft.VisualBasic;
using System.Security.Cryptography;


    public class LeaveFunction
    {

        public static bool sufficientLeave(string username, double no_leave_applied, string type_of_leave)
        {
            if(type_of_leave == "Annual Leave")
            {
                type_of_leave = "annual_leave";
            }
            else
            {
                type_of_leave = "medical_leave";
            }
            Database db = Database.Open("InvoiceSysDBContext");
            bool sufficientLeave = false;
            var sql = "SELECT TOP 1 " + type_of_leave + "_bal FROM AccountInfoes WHERE username = @0";
            var leaveLeft = db.QueryValue(sql, username);
            double leaveDeduction = leaveLeft - no_leave_applied;
            if (leaveDeduction > 0)
            {
                sufficientLeave = true;
            }
            return sufficientLeave;
        }

        public static bool pendingApprovalLeave(string username)
        {
            Database db = Database.Open("InvoiceSysDBContext");
            bool hasPendingLeave = false;
            var sql = "SELECT COUNT(*) FROM tbl_leaveApplication WHERE submitted_by = @0 AND status = 'Pending Approval' ";
            var pendingApprovalLeave = db.QueryValue(sql, LoginFunction.getNamebyUsername(username));
            if (pendingApprovalLeave > 0)
            {
                hasPendingLeave = true;
            }
            return hasPendingLeave;
        }

        public static void reduceLeave(int id, double no_days_applied, string type_of_leave, string username)
        {
        if (type_of_leave == "Annual Leave")
        {
            type_of_leave = "annual_leave";
        }
        else
        {
            type_of_leave = "medical_leave";
        }
        Database db = Database.Open("InvoiceSysDBContext");
        var sql = "";
            if (type_of_leave == "annual_leave")
            {
                sql = "SELECT TOP 1 a." + type_of_leave + "_bal AS leave_bal, a.username AS username FROM AccountInfoes a INNER JOIN Leave_Application b on a.username = b.username_submitted WHERE b.ID = @0";
            }
            else
            {
                sql = "SELECT TOP 1 a." + type_of_leave + "_bal AS leave_bal, a.username AS username FROM AccountInfoes a INNER JOIN Leave_Application b on a.username = b.username_submitted WHERE type_of_leave = 'Medical leave' AND username_submitted='" + username + "' ORDER BY b.ID DESC";
            }

            var result = db.QuerySingle(sql, id);
            sql = "UPDATE AccountInfoes SET " + type_of_leave + "_bal = @0 WHERE username=@1";
            db.Execute(sql, (result.leave_bal - no_days_applied), result.username);
        }

        public static void addLeave(int? id, double no_days_applied, string type_of_leave, string username)
        {
        if (type_of_leave == "Annual Leave")
        {
            type_of_leave = "annual_leave";
        }
        else
        {
            type_of_leave = "medical_leave";
        }
        Database db = Database.Open("InvoiceSysDBContext");
        var sql = "";
        if (type_of_leave == "annual_leave")
        {
            sql = "SELECT TOP 1 a." + type_of_leave + "_bal AS leave_bal, a.username AS username FROM AccountInfoes a INNER JOIN Leave_Application b on a.username = b.username_submitted WHERE b.ID = @0";
        }
        else
        {
            sql = "SELECT TOP 1 a." + type_of_leave + "_bal AS leave_bal, a.username AS username FROM AccountInfoes a INNER JOIN Leave_Application b on a.username = b.username_submitted WHERE type_of_leave = 'Medical leave' AND username_submitted='" + username + "' ORDER BY b.ID DESC";
        }

        var result = db.QuerySingle(sql, id);
        sql = "UPDATE AccountInfoes SET " + type_of_leave + "_bal = @0 WHERE username=@1";
        db.Execute(sql, (result.leave_bal + no_days_applied), result.username);
        }





}
