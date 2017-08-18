using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models
{
    public class S_frns
    {
        [Key]
        public int frnID { get; set; }
        public String S_userID { get; set; }
        public String s_frnID { get; set; }
        public bool Status { get; set; }


    }
}