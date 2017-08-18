using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Emart.Models;
using System.Data.Entity;
using Emart.Models.Account;

namespace Emart.DALS
{
    public class EmartDBContexts: DbContext
    {
        
        public DbSet<S_frns> S_frnss { get; set; }
        public DbSet<S_Post> S_Posts { get; set; }
        public DbSet<S_msgs> S_Msgs { get; set; }
        public DbSet<E_Users> E_Userss { get; set; }
        public DbSet<S_likes> S_Likess { get; set; }
        public DbSet<S_Comments> S_Commentss { get; set; }
        public DbSet<UserImageStore> UserImageStore { get; set; }
        public DbSet<Recommends> Recommends { get; set; }
        public DbSet<Groups> Groups { get; set; }

        public DbSet<UserPostImages> UserPostImages { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

    }
}