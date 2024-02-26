using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;


namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // to map the `FullName` property from `UserDTO` to the `Name` property in `UserDomain
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto,Region>().ReverseMap();
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }
    }

    //public class UserDTO
    //{
    //    public string FullName { get; set; }
    //}
    //public class UserDomain
    //{
    //    public string Name { get; set; }
    //}

}
