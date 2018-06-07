using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Model
{
    public class MainContext : IdentityDbContext<Account>
    {
        public MainContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static MainContext Create()
        {
            return new MainContext();
        }

        public DbSet<ProfileInfo> Profiles { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ReadNews> ReadNews { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
