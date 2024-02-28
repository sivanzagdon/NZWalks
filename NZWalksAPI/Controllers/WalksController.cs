using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //CREATE walk
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                //Convert DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

                //Convert Model Domain to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
           

        }

        //GET ALL walk
        //GET: /api/walks?FilterOn=Name&FilterQuery=Track&SortBy=Name&IsAscending=true&PageNumber=1&PageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery,
            [FromQuery] string? SortBy, [FromQuery] bool? IsAscending,
            [FromQuery] int PageNumber=1, [FromQuery] int PageSize=1000)
        {
            
                //get data from database- domain models
                var walksDomain = await walkRepository.GetAllAsync(FilterOn, FilterQuery, SortBy, IsAscending ?? true, PageNumber, PageSize);

                return Ok(mapper.Map<List<WalkDto>>(walksDomain));
        }

        //GET Walk By Id
        //GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //get domain models from database
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //return DTO back to client
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update Walk by Id
        //PUT: /api/Walks/
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)/////////
        {
            
                //convert DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                //check if region exist 
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
            
        }

        //DELETE SINGLE REGION BY ID
        //DELETE: http://localhost:portnumber/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //get domain models from database
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //return DTO back to client
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
