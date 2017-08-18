using Emart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emart.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public Vendor Vendor { get; set; }
        public List<VendorCategory> VendorCategory { get; set; }
        public List<Cart> Cart { get; set; }
        public List<Product> ProductList { get; set; }

    }
}