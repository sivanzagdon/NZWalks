using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions <NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Difficulty> Difficulties { get; set; } //group of object from type 
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data from Difficulties
            //Easy, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                { Id= Guid.Parse("df6fafeb-8d43-49ea-ba8a-1a6a1b4a966e"),
                  Name = "Easy",
                },
                new Difficulty()
                { Id=Guid.Parse("d687fb93-3311-429c-842c-43fe928dc40b"),
                  Name= "Medium",
                },
                new Difficulty()
                { Id=Guid.Parse("d4e7ddfc-8649-4931-9661-4c2c7a4dcfb5"),
                  Name= "Hard",
                },
             };

            //seed Difficulties to database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //seed data from Region
            var regions = new List<Region>()
             {
                new Region()
                {
                    Id = Guid.Parse("b7363190-7d77-4685-93af-d7f00052968a"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
             };
            modelBuilder.Entity<Region>().HasData(regions);

        }
    }
}
