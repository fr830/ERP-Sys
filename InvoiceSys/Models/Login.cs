using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrAng_Invoice.Models
{
    public class Login
    {
        [Required]
        public String username { get; set; }

        [Required]
        public String password { get; set; }
    }
}