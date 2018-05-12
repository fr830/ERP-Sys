using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MrAng_Invoice.Models
{
    public class AccountInfo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "User's Full Name")]
        public String user_full_name { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "User's Contact No")]
        public String user_contact_no { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Username")]
        public String username { get; set; }


        [StringLength(30)]
        [Display(Name = "Employee ID")]
        public String employee_id { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{4,15}$", ErrorMessage = "Password must be min 4 char, max 15 char, contains at least 1 capital letter, at least 1 symbol, at least 1 number. Eg: Pass1234!")]
        [StringLength(100)]
        [Display(Name = "Password")]
        public String password { get; set; }

        [StringLength(8)]
        public String tempPassword { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public String email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Company Name")]
        public String company_name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Role")]
        public String account_role { get; set; }

        [Display(Name = "Status")]
        public String status { get; set; }

        [Display(Name = "Created Date")]
        public DateTime created_date { get; set; }

        [Display(Name = "Created By")]
        public String created_by { get; set; }

        [Display(Name = "Last Login")]
        public DateTime last_login { get; set; }

        [Display(Name = "Last Logout")]
        public DateTime last_logout { get; set; } 

        public int retry_count { get; set; }

        public int reset_retry_count { get; set; }

        [Display(Name = "Type of User")]
        public String user_type { get; set; }

        [Display(Name = "Annual Leave Balance")]
        public float annual_leave_bal { get; set; }

        [Display(Name = "Medical Leave Balance")]
        public float medical_leave_bal { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Remarks")]
        public String remarks { get; set; }


    }
}