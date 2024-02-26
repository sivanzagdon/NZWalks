using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? FilterOn=null, string? FilterQuery=null,
                                     string? SortBy = null, bool IsAscending = true,
                                     int PageNumber = 1,  int PageSize = 1000);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);


    }
}
