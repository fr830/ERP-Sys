using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace MrAng_Invoice.Models
{
    public class Employee_WorkSheet
    {
        [Display(Name = "Employee ID: ")]
        [Required]
        public String employee_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Key]
        [Display(Name = "WorkSheet ID: ")]
        public int worksheet_ID { get; set; }

        [Display(Name = "Ticket ID: ")]
        public String ticket_ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date: ")]
        public DateTime working_date { get; set; }

        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Time In: ")]
        public DateTime time_in { get; set; }

        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Time Out: ")]
        public DateTime time_out { get; set; }

        [Required]
        [Display(Name = "Work Hrs: ")]
        public int total_working_hour { get; set; }

        [Required]
        [Display(Name = "Client Served: ")]
        public String client_served { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Submission Date: ")]
        public DateTime submission_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Approval Date: ")]
        public DateTime approval_date { get; set; }

        [Display(Name = "Approval Status: ")]
        public String approval_status { get; set; }

        [Display(Name = "Location: ")]
        [Required]
        [StringLength(500, ErrorMessage = "Job Location cannot be longer than 500 characters.")]
        public String job_location { get; set; }

        [Display(Name = "Job Description: ")]
        [Required]
        [StringLength(500, ErrorMessage = "Job Description cannot be longer than 500 characters.")]
        public String job_description { get; set; }

        [Display(Name = "Follow Up By: ")]
        [Required]
        [StringLength(100, ErrorMessage = "Follow Up By cannot be longer than 100 characters.")]
        public String followup_by { get; set; }

        [Display(Name = "Resquested By: ")]
        [Required]
        [StringLength(100, ErrorMessage = "Requested By cannot be longer than 100 characters.")]
        public String requested_by { get; set; }

        [Display(Name = "Remarks: ")]
        [Required]
        [StringLength(500, ErrorMessage = "Remarks cannot be longer than 100 characters.")]
        public String remarks { get; set; }

        [Display(Name = "Working Date Type: ")]
        [StringLength(5, ErrorMessage = "Working Date Type cannot be longer than 5 characters.")]
        public String working_date_type { get; set; }

        [Display(Name = "Employee Name: ")]
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public String employee_name { get; set; }

        [Display(Name = "Chargeable?")]
        public bool isChargeable { get; set; }
    }

    public class InvoiceSysDBContext : DbContext
    {
        public DbSet<Employee_WorkSheet> Employee_WorkSheet { get; set; }
        public DbSet<Customer_WorkSheet> Customer_Worksheet { get; set; }
        public DbSet<AccountInfo> AccountInfo { get; set; }
        public DbSet<Customer_PrivateInfo> Customer_PrivateInfo { get; set; }
        public DbSet<Quotation_Details> Quotation_Details { get; set; }
        public DbSet<Invoice_Details> Invoice_Details { get; set; }
        public DbSet<UMS> UMS { get; set; }
        public DbSet<HolidayTable> HolidayTable { get; set; }
        public DbSet<Leave_Application> Leave_Application { get; set; }

        public System.Data.Entity.DbSet<MrAng_Invoice.Models.Employee_Management> Employee_Management { get; set; }
    }
}