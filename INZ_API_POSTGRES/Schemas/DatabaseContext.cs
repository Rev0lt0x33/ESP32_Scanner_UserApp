
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INZ_API_POSTGRES.Schemas
{
    public class DatabaseContext : IdentityDbContext<Users>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public virtual DbSet<Data> Datas { get; set; }

        public virtual DbSet<Devices> Device { get; set; } 

    }
}
