using Emart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Emart.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string VendorId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Detail { get; set; }
        public string MainCategoryId { get; set; }
        public string VendorCategoryId { get; set; }
        public string ImagePath { get; set; }
        public string ActualImagePath { get; set; }
    }
}