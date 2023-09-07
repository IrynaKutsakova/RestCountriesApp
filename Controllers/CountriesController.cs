using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using RestCountriesApp.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace RestCountriesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly string _countriesApiEndpoint;
        private static readonly HttpClient HttpClient = new HttpClient();

        public CountriesController(IConfiguration configuration)
        {
            _countriesApiEndpoint = configuration.GetValue<string>("ApiUrls:RESTCountriesAPI");
        }


        // GET: api/countries?param1=value1&number=1&param3=value3
        // If you call /api/countries? param1 = Canada, it will return countries with names that contain "Canada".
        // If you call /api/countries? number = 50000000, it will return countries with a population of 50 million or more.
        // If you call /api/countries? param3 = Asia, it will return countries in the Asia region.
        [HttpGet]
        public async Task<IActionResult> FetchCountries(string? param1 = null, int? number = null,
            string? param3 = null)
        {
            var response = await HttpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            if (!string.IsNullOrWhiteSpace(param1)) // This is for nameFilter
            {
                countries = countries.Where(c =>
                    c.Name.Common.Contains(param1, StringComparison.OrdinalIgnoreCase) ||
                    c.Name.Official.Contains(param1, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (number.HasValue) // This is for populationFilter
            {
                countries = countries.Where(c => c.Population >= number.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(param3)) // This is for regionFilter
            {
                countries = countries.Where(c =>
                    c.Region.Equals(param3, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return Ok(countries);
        }

        // GET: api/countries/search?name=your_search_term
        [HttpGet("search")]
        public async Task<IActionResult> SearchByCommonName(string name)
        {
            var response = await HttpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            // Filtering countries by name/common that contains the provided string (case-insensitive)
            var filteredCountries = countries.Where(c =>
                c.Name.Common.Contains(name, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(filteredCountries);
        }

        // GET: api/countries/populationLessThanMillion?value=10
        [HttpGet("populationLessThanMillion")]
        public async Task<IActionResult> FilterByPopulationLessThanMillion(int value)
        {
            var response = await HttpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            int populationThreshold = value * 1000000;  // Convert the provided number into an actual population count.
            var filteredCountries = countries.Where(c => c.Population < populationThreshold).ToList();

            return Ok(filteredCountries);
        }
    }
}