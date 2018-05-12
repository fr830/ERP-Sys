using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class Leave_Application
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Leave Applied From: ")]
        [Required]
        public DateTime leave_applied_from { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Leave Applied To: ")]
        [Required]
        public DateTime leave_applied_to { get; set; }

        [Required]
        [Display(Name = "No Days Applied: ")]
        public double no_days_applied { get; set; }

        [Display(Name = "Status: ")]
        public String status { get; set; }

        [Required]
        [Display(Name = "Type Of Leave: ")]
        public String type_of_leave { get; set; }

        [Display(Name = "Reason: ")]
        public String reason{ get; set; }

        [Display(Name = "Submitted By: ")]
        public String submitted_by { get; set; }

        [Display(Name = "Submitted Date: ")]
        public DateTime submitted_date { get; set; }

        [Display(Name = "Approval By: ")]
        public String approved_by { get; set; }

        [Display(Name = "Approval Date: ")]
        public DateTime approval_date { get; set; }

        [Display(Name = "Approval Reason: ")]
        public String approval_reason { get; set; }

        public String username_submitted { get; set; }

        [Display(Name = "Ticket ID: ")]
        public String leave_ticket_id { get; set; }
    }
}