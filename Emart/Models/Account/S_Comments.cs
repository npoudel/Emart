using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Emart.Models
{
    public class S_Comments
    {
        [Key]

        public int comID { get; set; }
        public int Post_id { get; set; }
        public int userID { get; set; }
        public String Coment { get; set; }
        public DateTime Com_time { get; set; }



    }
}