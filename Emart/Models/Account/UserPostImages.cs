using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emart.Models.Account;

namespace Emart.Models.Account
{
    public class UserPostImages
    {


        public UserPostImages()
        {
            this.E_User= new HashSet<E_Users>();

        }
        [Key]
        public int ImageID { get; set; }
        public String ImageName { get; set; }
        public byte[] ImageByte { get; set; }

        public String ImagePath { get; set; }

        public Nullable<bool> ImageisDeleted { get; set; }

        public virtual ICollection<E_Users> E_User { get; set; }




    }
}
