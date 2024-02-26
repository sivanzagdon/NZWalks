using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository: IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null, string? SortBy = null, bool IsAscending = true,
                                                   int PageNumber = 1, int PageSize = 1000)
        {
            var walks= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(FilterOn)==false && string.IsNullOrWhiteSpace(FilterQuery) == false)
            {
                if(FilterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks=walks.Where(x=>x.Name.Contains(FilterQuery));
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(SortBy)==false)
            {
                if(SortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
                }
                else  if(SortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResults = (PageNumber - 1) * (PageSize);

            return await walks.Skip(skipResults).Take(PageSize).ToListAsync();   
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;


            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();

            return existingWalk;
        }

    }
}
