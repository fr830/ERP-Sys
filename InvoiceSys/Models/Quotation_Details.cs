using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class Quotation_Details
    {
     [Key]
     public int quotation_id { get; set; }

     [StringLength(20)]
     [Display(Name ="Quotation No")]
     public String quotation_no { get; set; }

    [Display(Name ="Company Name")]
    [StringLength(100)]
     public String company_name { get; set; }

    [Display(Name ="Company Address")]
    [StringLength(500)]
     public String company_address { get; set; }

     [Display(Name = "Remarks")]
     [StringLength(700)]
     public String remarks { get; set; }

     [StringLength(20)]
     [Display(Name ="Status")]
     public String status { get; set; }

     [StringLength(20)]
    [Display(Name = "Show Customer?")]
    public String approval_status { get; set; }

    [Display(Name = "Approval Date")]
    public DateTime approval_date { get; set; }

    public String created_by { get; set; }

    [Display(Name = "Created Date")]
    public DateTime created_date { get; set; }

    [Display(Name = "Modified By")]
    public String modified_by { get; set; }

    [Display(Name = "Modified Date")]
    public DateTime modified_date { get; set; }

    [Display(Name ="Attention To")]
    public String attn_to { get; set; }

    [StringLength(100)]
   public String payment_term { get; set; }
    }
}