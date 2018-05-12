using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.Data;

namespace MrAng_Invoice.Models
{
    public class Customer_PrivateInfo
    {
 
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Customer ID: ")]
        public String customer_id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Company Name: ")]
        public string company_name { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Company Registration No: ")]
        public string company_reg_no { get; set; }


        [Required]
        [StringLength(100)]
        [Display(Name = "Payment Term: ")]
        public string payment_term { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Company GST No: ")]
        public string company_gst_no { get; set; }

        [Required]
        [StringLength(140)]
        [Display(Name = "Company Address: ")]
        public string company_address { get; set; }

        [StringLength(30)]
        [Display(Name = "State: ")]
        public string state { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Company Fax No: ")]
        public string company_fax_no { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Company Tel No: ")]
        public string company_tel_no { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Person Incharge 1: ")]
        public string company_person_incharge1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Department 1: ")]
        public string company_department1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Position 1: ")]
        public string company_position1 { get; set; }

        [StringLength(100)]
        [Display(Name = "Company Email 1: ")]
        public string company_email1 { get; set; }

        [StringLength(20)]
        [Display(Name = "Company Tel No 1: ")]
        public string company_tel_no1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Person Incharge 2: ")]
        public string company_person_incharge2 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Department 2: ")]
        public string company_department2 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Position 2: ")]
        public string company_position2 { get; set; }

        [StringLength(100)]
        [Display(Name = "Company Email 2: ")]
        public string company_email2 { get; set; }

        [StringLength(20)]
        [Display(Name = "Company Tel No 2: ")]
        public string company_tel_no2 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Person Incharge 3: ")]
        public string company_person_incharge3 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Department 3: ")]
        public string company_department3 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Position 3: ")]
        public string company_position3 { get; set; }

        [StringLength(100)]
        [Display(Name = "Company Email 3: ")]
        public string company_email3 { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Tel No 3: ")]
        public string company_tel_no3 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime created_date { get; set; }

        [StringLength(10)]
        [Display(Name = "Status")]
        public string status { get; set; }
    }
}