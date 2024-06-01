using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                //Get all regions from web  API
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7281/api/regions"); //Get All Regions
                httpResponseMessage.EnsureSuccessStatusCode(); //Ensure success status code

                //If successed-> response body is read as a string
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                //Log the exception

                throw;
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            //Create Client
            var client = httpClientFactory.CreateClient();

            //Create Request
            var httpRequestMassege = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7281/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

        
                var httpResponseMessage = await client.SendAsync(httpRequestMassege);
                httpResponseMessage.EnsureSuccessStatusCode();
                var response= await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
                if(response is not null)
                    return RedirectToAction("Index", "Regions");
    
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Create Client
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7281/api/Regions/{id.ToString()}");
            if(response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            //Create Client
            var client = httpClientFactory.CreateClient();
            var httpRequestMassege = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7281/api/Regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMassege);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                //Create Client
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7281/api/Regions/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch(Exception ex) 
            {
                //
            }
            return View("Edit");
        }

    }
}
