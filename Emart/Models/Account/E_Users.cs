using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Emart.Models
{
    public class E_Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Gender { get; set; }
        public String Profession { get; set; }
        public int PhoneNumber { get; set; }
        public String Address { get; set; }

        //public int UserImageStore_ImageID { get; set; }
        public int imgid { get; set; }
        //public  int Postimgid { get; set; }
        [NotMapped]
        public HttpPostedFileWrapper UserImage { get; set; }
     
        ///public HttpPostedFileWrapper ImageFile { get; set; }
        ////public HttpPostedFileWrapper ImageFile { get; set; }
    }
}