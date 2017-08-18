using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "VendorUserName is Required")]
        public string VendorUserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Please confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int TemplateId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        


    }
}