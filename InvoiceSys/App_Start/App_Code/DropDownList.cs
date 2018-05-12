using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;

    public class DropDownList
    {
    private static InvoiceSysDBContext db = new InvoiceSysDBContext();
    

    public static List<String> CreateCompanyList()
    {
        var CompanyList = new List<String>();
        var CompanyQuery = from c in db.Customer_PrivateInfo
                           orderby c.company_name
                           select c.company_name;
        CompanyList.AddRange(CompanyQuery.Distinct());
        return CompanyList;
    }

    public static List<String> CompanyDropDownList()
    {
        var CompanyList = new List<String>();
        var CompanyQuery = from c in db.Customer_PrivateInfo
                           orderby c.company_name
                           select c.company_name;
        CompanyList.AddRange(CompanyQuery.Distinct());
        //ViewBag.company = CompanyList;
        return CompanyList;
    }

    public static List<String> CreateRoleList()
    {
        var roleList = new List<string>();

        var roleQuery = from d in db.UMS
                        orderby d.role_name
                        select d.role_name;

        roleList.AddRange(roleQuery.Distinct());
        return roleList;
        //ViewBag.account_role = new SelectList(roleList);
    }



    public static List<String> CreateCompanyID()
    {
        var CompanyIDList = new List<String>();
        var CompanyIDQuery = from c in db.Customer_PrivateInfo
                             orderby c.customer_id
                             select c.customer_id;
        CompanyIDList.AddRange(CompanyIDQuery.Distinct());
        return CompanyIDList;
    }

    public static List<String> CreateCreatorDropDownList()
    {
        var CreatorList = new List<String>();
        var CreatorQuery = from q in db.Invoice_Details
                           orderby q.created_by
                           select q.created_by;
        CreatorList.AddRange(CreatorQuery.Distinct());
        return CreatorList;
       // ViewBag.creator = CreatorList;
    }

    public static List<String> CreateTicketIDDropDownList()
    {
        var ticketIDList = new List<String>();
        var ticketIDQuery = from c in db.Customer_Worksheet
                            where c.approval_status == "Approve"
                            orderby c.ticket_id
                            select c.ticket_id;
        ticketIDList.AddRange(ticketIDQuery.Distinct());
        return ticketIDList;
        //ViewBag.ticketID = ticketIDList;
    }

    public static List<String> CreateQuotationNoDropDownList()
    {
        var QuotationNoList = new List<String>();
        var QuotationNoQuery = from c in db.Quotation_Details
                               where c.approval_status == "Approve" && c.status=="PO Issued"
                               orderby c.quotation_no
                               select c.quotation_no;
        QuotationNoList.AddRange(QuotationNoQuery.Distinct());
        return QuotationNoList;
        //ViewBag.quotaionNo = QuotationNoList;
    }
    public static List<String> CreateEnginnerUsernameList()
    {
        var UserNameList = new List<String>();
        var UserNameQuery = from c in db.AccountInfo
                               where c.user_type == "Engineer"
                               orderby c.username
                               select c.username;
        UserNameList.AddRange(UserNameQuery.Distinct());
        return UserNameList;

    }
    public static List<String> CreateEngineerNameList()
    {
        var NameList = new List<String>();
        var UserNameQuery = from c in db.AccountInfo
                            where c.user_type == "Engineer"
                            orderby c.user_full_name
                            select c.user_full_name;
        NameList.AddRange(UserNameQuery.Distinct());
        return NameList;

    }
    public static List<dynamic> CreateUserTypeList(WebMatrix.Data.Database db2)
    {
        var UserTypeList = db2.Query("SELECT DISTINCT user_type FROM tbl_userType");
        return UserTypeList.Cast<dynamic>().ToList();
        //ViewBag.quotaionNo = QuotationNoList;
    }

    public static List<String> CreateQuotationNoList()
    {
        var quoNoList = new List<String>();
        var quoNoQuery = from c in db.Quotation_Details
                            orderby c.quotation_no
                            select c.quotation_no;
        quoNoList.AddRange(quoNoQuery.Distinct());
        return quoNoList;

    }

    public static List<String> CreateInvoiceNoList()
    {
        var invNoList = new List<String>();
        var invNoQuery = from c in db.Invoice_Details
                         orderby c.invoice_no
                         select c.invoice_no;
        invNoList.AddRange(invNoQuery.Distinct());
        return invNoList;

    }

    public static List<String> CreateStateList()
    {
        List<String> stateList = new List<String> { "Selangor", "Kuala Lumpur", "Sarawak", "Johor" , "Pulau Pinang", "Sabah", "Perak", "Pahang", "Negeri Sembilan", "Kedah", "Melaka", "Kelantan", "Perlis","Labuan"};
        return stateList;
    }
}
