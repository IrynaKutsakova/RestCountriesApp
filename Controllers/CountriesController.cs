using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestCountriesApp.Models;

namespace RestCountriesApp.Controllers
{
    /// <summary>
    /// Controller responsible for providing country-related endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly string _countriesApiEndpoint;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor to initialize the CountriesController with required configurations.
        /// </summary>
        /// <param name="configuration">App's configuration to fetch necessary settings.</param>
        /// <param name="httpClient">HttpClient object</param>
        public CountriesController(IConfiguration configuration, HttpClient httpClient)
        {
            _countriesApiEndpoint = configuration.GetValue<string>("ApiUrls:RESTCountriesAPI");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves countries based on provided filters.
        /// </summary>
        /// <param name="param1">Filter by country name.</param>
        /// <param name="number">Filter by minimum population.</param>
        /// <param name="param3">Filter by region.</param>
        /// <param name="sortOrder">Sorts countries by their common name. Default is 'ascend'. Accepts 'ascend' or 'descend'.</param>
        /// <param name="limit">Limits the number of returned countries to the specified number.</param>
        /// <returns>Returns a filtered list of countries.</returns>
        // GET: api/countries?param1=value1&number=1&param3=value3&sortOrder=value
        // If you call /api/countries? param1 = Canada, it will return countries with names that contain "Canada".
        // If you call /api/countries? number = 50000000, it will return countries with a population of 50 million or more.
        // If you call /api/countries? param3 = Asia, it will return countries in the Asia region.
        // If you call /api/countries?sortOrder=descend to get countries sorted in descending order by their name/common
        // If you call /api/countries?limit=15 it will return the first 15 records after applying the other filters.
        [HttpGet]
        public async Task<IActionResult> FetchCountries(string? param1 = null, int? number = null,
            string? param3 = null, string? sortOrder = "ascend", int? limit = null)
        {
            var response = await _httpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            // existing filters...
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

            // Sort countries by name
            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                countries = SortCountriesByName(countries, sortOrder);
            }

            if (limit.HasValue)
            {
                countries = Paginate(countries, limit.Value); // Use the Paginate function here
            }

            return Ok(countries);
        }

        /// <summary>
        /// Searches countries by common name.
        /// </summary>
        /// <param name="name">Country name to search for.</param>
        /// <returns>Returns a list of countries that match the provided name.</returns>
        // GET: api/countries/search?name=your_search_term
        [HttpGet("search")]
        public async Task<IActionResult> SearchByCommonName(string name)
        {
            var response = await _httpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            // Filtering countries by name/common that contains the provided string (case-insensitive)
            var filteredCountries = countries.Where(c =>
                c.Name.Common.Contains(name, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(filteredCountries);
        }

        /// <summary>
        /// Retrieves countries with a population less than the provided value in millions.
        /// </summary>
        /// <param name="value">Value in millions to filter countries by population.</param>
        /// <returns>Returns a list of countries with a population less than the provided value.</returns>
        // GET: api/countries/populationLessThanMillion?value=10
        [HttpGet("populationLessThanMillion")]
        public async Task<IActionResult> FilterByPopulationLessThanMillion(int value)
        {
            var response = await _httpClient.GetStringAsync(_countriesApiEndpoint);
            var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(response);

            int populationThreshold = value * 1000000; // Convert the provided number into an actual population count.
            var filteredCountries = countries.Where(c => c.Population < populationThreshold).ToList();

            return Ok(filteredCountries);
        }

        /// <summary>
        /// Sorts countries by their common name.
        /// </summary>
        /// <param name="countries">List of countries to be sorted.</param>
        /// <param name="sortOrder">Order to sort by. Accepts 'ascend' or 'descend'.</param>
        /// <returns>Returns a sorted list of countries.</returns>
        public List<CountryInfo> SortCountriesByName(List<CountryInfo> countries, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("ascend", StringComparison.OrdinalIgnoreCase))
            {
                return countries.OrderBy(c => c.Name.Common).ToList();
            }
            else if (sortOrder.Equals("descend", StringComparison.OrdinalIgnoreCase))
            {
                return countries.OrderByDescending(c => c.Name.Common).ToList();
            }
            else
            {
                throw new ArgumentException("Invalid sortOrder provided");
            }
        }

        /// <summary>
        /// Paginates the provided list of countries.
        /// </summary>
        /// <param name="countries">List of countries to be paginated.</param>
        /// <param name="limit">Number of countries to be returned.</param>
        /// <returns>Returns a paginated list of countries.</returns>
        private List<CountryInfo> Paginate(List<CountryInfo> countries, int limit)
        {
            return countries.Take(limit).ToList();
        }
    }
}