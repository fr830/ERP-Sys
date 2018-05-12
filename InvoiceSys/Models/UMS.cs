using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class UMS
    {
        [Key]
        public int ums_id { get; set; }

        [StringLength(1000)]
        public String ums_value { get; set; }

        [StringLength(100)]
        [Display(Name ="Role Name")]
        [Required]
        public String role_name { get; set; }

        [StringLength(200)]
        public String created_by { get; set; }

        public DateTime created_date { get; set; }

        [StringLength(10)]
        public String status { get; set; }
    }
}