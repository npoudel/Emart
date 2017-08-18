using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emart.Models
{
    public class VendorCategory
    {
        [Key]
        public int VendorCatId { get; set; }
        public string VendorCategoryName { get; set; }
        public string VendorId { get; set; }
    }
}