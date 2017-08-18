using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emart.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string ProductId { get; set; }
        public string Value { get; set; }
    }
}