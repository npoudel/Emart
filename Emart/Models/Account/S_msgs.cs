using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models
{
    public class S_msgs
    {
        [Key]
        public int msgID { get; set; }
        public DateTime msgtime { get; set; }
        public int msg_SenderID { get; set; }
        public int msg_RecpientID { get; set; }
        public String msg_text { get; set; }
        public String msg_Subject { get; set; }
   
              
    }
}