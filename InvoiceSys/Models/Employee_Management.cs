using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class Employee_Management
    {
        [Key]
        public String employee_id { get; set; }
        public String username { get; set; }
        public String employee_name { get; set; }
        public String user_contact_no { get; set; }

    }
}