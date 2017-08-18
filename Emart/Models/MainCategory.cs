using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Emart.Models
{
    public class MainCategory
    {
        [Key]
        public int MainCatID { get; set; }
        public string CatagoryName { get; set; }
    }
}