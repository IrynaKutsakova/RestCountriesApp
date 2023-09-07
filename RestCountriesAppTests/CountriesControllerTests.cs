using Microsoft.AspNetCore.Mvc;
using RestCountriesApp.Models;
using Xunit;

namespace RestCountriesAppTests
{
    /// <summary>
    /// Unit tests for the CountriesController using xUnit.
    /// </summary>
    public class CountriesControllerTests : CountriesControllerTestsBase
    {
        // Sample mock response representing the countries' data.
        private const string MockApiResponse = @"
        [
            {
                'Name': {
                    'Common': 'Canada',
                    'Official': 'Canada'
                },
                'Capital': ['Ottawa'],
                'Cioc': 'CAN',
                'Currencies': {
                    'CAD': {
                        'Code': 'CAD',
                        'Name': 'Canadian dollar',
                        'Symbol': '$'
                    }
                },
                'Demonym': 'Canadian',
                'Flags': {
                    'Png': 'https://path-to-canadian-flag.png',
                    'Svg': 'https://path-to-canadian-flag.svg'
                },
                'Id': 'CAN',
                'Independent': true,
                'Landlocked': 'No',
                'Languages': {
                    'en': 'English',
                    'fr': 'French'
                },
                'Maps': {
                    'Google': 'https://google-map-link-to-canada'
                },
                'Population': 37590000,
                'Region': 'Americas',
                'Subregion': 'North America',
                'Tld': ['.ca'],
                'Translations': {
                    'fr': {
                        'SomeKey': 'Translation-in-French'
                    }
                },
                'UnMember': false
            },
            {
                'Name': {
                    'Common': 'India',
                    'Official': 'Republic of India'
                },
                'Capital': ['New Delhi'],
                'Cioc': 'IND',
                'Currencies': {
                    'INR': {
                        'Code': 'INR',
                        'Name': 'Indian rupee',
                        'Symbol': '₹'
                    }
                },
                'Demonym': 'Indian',
                'Flags': {
                    'Png': 'https://path-to-indian-flag.png',
                    'Svg': 'https://path-to-indian-flag.svg'
                },
                'Id': 'IND',
                'Independent': true,
                'Landlocked': 'No',
                'Languages': {
                    'en': 'English',
                    'hi': 'Hindi'
                },
                'Maps': {
                    'Google': 'https://google-map-link-to-india'
                },
                'Population': 1380004385,
                'Region': 'Asia',
                'Subregion': 'South Asia',
                'Tld': ['.in'],
                'Translations': {
                    'fr': {
                        'SomeKey': 'Translation-in-French'
                    }
                },
                'UnMember': false
            },
            {
                'Name': {
                    'Common': 'Australia',
                    'Official': 'Commonwealth of Australia'
                },
                'Capital': ['Canberra'],
                'Cioc': 'AUS',
                'Currencies': {
                    'AUD': {
                        'Code': 'AUD',
                        'Name': 'Australian dollar',
                        'Symbol': '$'
                    }
                },
                'Demonym': 'Australian',
                'Flags': {
                    'Png': 'https://path-to-australian-flag.png',
                    'Svg': 'https://path-to-australian-flag.svg'
                },
                'Id': 'AUS',
                'Independent': true,
                'Landlocked': 'No',
                'Languages': {
                    'en': 'English'
                },
                'Maps': {
                    'Google': 'https://google-map-link-to-australia'
                },
                'Population': 25200000,
                'Region': 'Oceania',
                'Subregion': 'Australia and New Zealand',
                'Tld': ['.au'],
                'Translations': {
                    'fr': {
                        'SomeKey': 'Translation-in-French'
                    }
                },
                'UnMember': false
            }
        ]";

        /// <summary>
        /// Initializes a new instance of the CountriesControllerTests class.
        /// </summary>
        public CountriesControllerTests()
        {
            MockMessageHandler.SetResponseContent(MockApiResponse);
        }

        /// <summary>
        /// Tests the FetchCountries method of the CountriesController when provided with a name filter.
        /// </summary>
        [Fact]
        public async Task FetchCountries_WithNameFilter_ReturnsFilteredCountries()
        {
            // Arrange: Setup the expected API response and expected results
            const string expectedCountryName = "Canada";

            // Act: Call the controller's method with specific parameters
            var result = await Controller.FetchCountries(param1: expectedCountryName) as OkObjectResult;

            // Assert: Verify that the returned results match the expected outcome
            var countries = result?.Value as List<CountryInfo>;
            Assert.NotNull(countries);
            Assert.True(countries.All(c =>
                c.Name.Common.Contains(expectedCountryName) || c.Name.Official.Contains(expectedCountryName)));
        }

        /// <summary>
        /// Validates that the FetchCountries method filters countries by name.
        /// </summary>
        [Fact]
        public async Task FetchCountries_WithPopulationFilter_ReturnsFilteredCountries()
        {
            const int expectedPopulation = 50000000;

            var result = await Controller.FetchCountries(number: expectedPopulation) as OkObjectResult;

            var countries = result?.Value as List<CountryInfo>;
            Assert.NotNull(countries);
            Assert.True(countries.All(c => c.Population >= expectedPopulation));
        }

        /// <summary>
        /// Validates that the FetchCountries method filters countries by region.
        /// </summary>
        [Fact]
        public async Task FetchCountries_WithRegionFilter_ReturnsFilteredCountries()
        {
            const string expectedRegion = "Asia";

            var result = await Controller.FetchCountries(param3: expectedRegion) as OkObjectResult;

            var countries = result?.Value as List<CountryInfo>;
            Assert.NotNull(countries);
            Assert.True(countries.All(c => c.Region.Equals(expectedRegion, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Validates that the SearchByCommonName method returns countries matching a given name.
        /// </summary>
        [Fact]
        public async Task SearchByCommonName_WithValidName_ReturnsFilteredCountries()
        {
            const string searchName = "Canada";

            var result = await Controller.SearchByCommonName(searchName) as OkObjectResult;

            var countries = result?.Value as List<CountryInfo>;
            Assert.NotNull(countries);
            Assert.True(countries.All(c => c.Name.Common.Contains(searchName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Validates that the FilterByPopulationLessThanMillion method returns countries with populations below a certain threshold.
        /// </summary>
        [Fact]
        public async Task FilterByPopulationLessThanMillion_WithValidValue_ReturnsFilteredCountries()
        {
            const int valueInMillions = 10;
            const int expectedPopulation = valueInMillions * 1000000;

            var result = await Controller.FilterByPopulationLessThanMillion(valueInMillions) as OkObjectResult;

            var countries = result?.Value as List<CountryInfo>;
            Assert.NotNull(countries);
            Assert.True(countries.All(c => c.Population < expectedPopulation));
        }
    }
}