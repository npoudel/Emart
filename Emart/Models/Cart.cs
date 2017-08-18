using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emart.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string ProductId { get; set; }
        public string userId { get; set; }
        public string VendorId { get; set; }
        public string Status { get; set; }
        public string Quantity { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string ImagePath { get; set; }
    }
}