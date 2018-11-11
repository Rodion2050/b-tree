using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Kursach2.utils
{
    public class AppDbContext : DbContext
    {
        public AppDbContext():
            base("AppDB")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}
