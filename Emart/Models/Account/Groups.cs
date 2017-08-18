using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Emart.Models.Account
{
    public class Groups
    {
        [Key]
        public int Id { get; set; }

        public String GroupName { get; set; }

        public String Group_link { get; set; }
        public String Group_image { get; set; }
        public bool join { get; set; }
    }
}