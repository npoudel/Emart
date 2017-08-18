using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Emart.Models
{
    public class S_Post
    {
        [Key]
        public int postID { get; set; }
        public int S_userID { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public String PostTime { get; set; }  
        public String  PostContent { get; set; }
        public int Post_like { get; set; }
        public int Postimgid { get; set; }
        [NotMapped]
        public HttpPostedFileWrapper PostImage { get; set; }
    }
}