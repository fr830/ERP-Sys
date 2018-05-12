using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrAng_Invoice.Models
{
    public class Customer_WorkSheet
    {
        [Display(Name = "Customer WorkSheet ID: ")]
        [StringLength(20, ErrorMessage = "Ticket ID cannot be longer than 20 characters.")]
        public String ticket_id { get; set; }

        [Display(Name = "Employee ID: ")]
        public String employee_ID { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Customer WorkSheet ID: ")]
        public int customer_worksheet_ID { get; set; }

        [Display(Name = "Employee WorkSheet ID: ")]
        public int employee_worksheet_ID { get; set; }

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
        [StringLength(500, ErrorMessage = "Remarks cannot be longer than 500 characters.")]
        public String remarks { get; set; }

        [Display(Name = "Working Date Type: ")]
        [StringLength(5, ErrorMessage = "Working Date Type cannot be longer than 5 characters.")]
        public String working_date_type { get; set; }

        [Display(Name = "Employee Name: ")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public String employee_name { get; set; }

        [Display(Name = "Chargeable?")]
        public bool isChargeable { get; set; }
    }
}