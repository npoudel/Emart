using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Emart.Models;
using Emart.Models.Account;

namespace Emart.ViewModels
{
    public class SocialViewModel
    {
        //public string Username { get; set; }
        //public string Email { get; set; }
        //public string PostContent { get; set; }
        //public String PostTime { get; set; }
        //public int PostLike { get; set; }
        public E_Users E_users { get; set; }
        public S_frns S_frnss { get; set; }
        public S_Comments S_comments { get; set; }
        public S_likes S_likess { get; set; }
        public S_Post S_posts { get; set; }
        public S_msgs S_msgss { get; set; }
        public UserImageStore UserImageStore { get; set; }
    }
}