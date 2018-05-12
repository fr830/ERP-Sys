using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class HolidayTable
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "State")]
        public String state { get; set; }
        [Display(Name = "Reference Key")]
        public String referenceKey { get; set; }
        [Display(Name = "Year")]
        public String year { get; set; }
    }
}