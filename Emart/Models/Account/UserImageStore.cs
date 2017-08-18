//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Emart.DALS;



namespace Emart.Models.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class UserImageStore
    {

        public UserImageStore()
        {
            this.E_User = new HashSet<E_Users>();

        }
        [Key]
        public int ImageID {get;set;}
        public  String ImageName { get; set; }
        public byte[] ImageByte { get; set; }

        public String ImagePath { get; set; }

        public Nullable<bool> ImageisDeleted { get; set; }

        public virtual ICollection<E_Users> E_User { get; set; }




    }
}