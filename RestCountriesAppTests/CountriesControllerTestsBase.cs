using Microsoft.Extensions.Configuration;
using RestCountriesApp.Controllers;

namespace RestCountriesAppTests
{
    public class CountriesControllerTestsBase
    {
        protected IConfiguration Configuration;
        protected MockHttpMessageHandler MockMessageHandler;
        protected HttpClient HttpClient;
        protected CountriesController Controller;

        public CountriesControllerTestsBase()
        {
            // Set up in-memory configuration data
            var inMemorySettings = new Dictionary<string, string> {
                {"ApiUrls:RESTCountriesAPI", "https://restcountries.com/v3.1/all"}
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Setup mock HttpClient to simulate API requests
            MockMessageHandler = new MockHttpMessageHandler("{}", System.Net.HttpStatusCode.OK); // you can adjust these parameters as needed
            HttpClient = new HttpClient(MockMessageHandler);

            // Instantiate the controller with the dependencies
            Controller = new CountriesController(Configuration, HttpClient);
        }
    }
}