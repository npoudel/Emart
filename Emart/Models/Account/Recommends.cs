using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models.Account
{
    public class Recommends
    {
        [Key]
        public int ID { get; set; }
        public String userid { get; set; }
        public String ProductName { get; set; }
        public int quantity { get; set; }
        public double? unitprice { get; set; }
        public string country { get; set; }
        //public String CategoryID
        //{
        //    get; set;
        //}
     }
}