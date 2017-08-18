using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models
{
    public class S_likes
    {
        [Key]
        public int likeID { get; set; }

        public int PostId { get; set; }

        public int userId { get; set; }
         

        public DateTime LikeDate { get; set; } 
      

    }
}