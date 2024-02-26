using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; //IdentityDbContext , IdentityRole
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions <NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "d460df7d-7174-4c5a-9a50-ff20af05f5ca";
            var writerRoleId = "fa47208a-2db0-40e1-b0d5-15b183e4efdd";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id= readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id= writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                },
            };
            //seed roles to database
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
