using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    //http://localhost:portnumber/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRespository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext
            , IRegionRepository regionRespository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRespository = regionRespository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGION
        //GET: http://localhost:portnumber/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //get data from database- domain models
                var regionsDomain = await regionRespository.GetAllAsync();

                //return DTOs
                logger.LogInformation($"Finished GetAllRegions request with data:{JsonSerializer.Serialize(regionsDomain)}");
                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            
        }


        //GET SINGLE REGION BY ID
        //GET: http://localhost:portnumber/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //get domain models from database
            var regionDomain = await regionRespository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

           

            //return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        //POST TO CREATE NEW RREGION
        //POST: http://localhost:portnumber/regions/
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Convert DTO to Domain Model
                var RegionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create new Region
                RegionDomainModel = await regionRespository.CreateAsync(RegionDomainModel);

                //convert Domain Model back to DTO:
                var regionDto = mapper.Map<RegionDto>(RegionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
           
            
        }

        //update region
        //PUT: http://localhost:portnumber/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                //convert DTO to Domain Model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                //check if region exist 
                regionDomainModel = await regionRespository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }


                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            
                
        }

        //DELETE SINGLE REGION BY ID
        //DELETE: http://localhost:portnumber/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //get domain models from database
            var regionDomainModel = await regionRespository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
