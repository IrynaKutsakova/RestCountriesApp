# RestCountriesApp
RestCountriesApp is a Web API built with .NET Core, aimed at providing users with information on different countries. The application fetches its data from the RESTCountriesAPI, offering users various filters to narrow down their search. Whether you need to find countries based on names, population, or region, RestCountriesApp has got you covered. Additionally, the application offers sorting and pagination functionalities, enhancing the overall user experience.

# Running the Application Locally:
## Using Kestrel (Built-in server):
1. Ensure you have .NET Core SDK installed.
1. Clone this repository to your local machine.
1. Navigate to the project's root directory via your terminal or command prompt.
1. Execute the command: dotnet restore to restore the required packages.
1. After the restore completes, run: dotnet run.
1. The application will start and should be available at https://localhost:7104 or http://localhost:5042.
## Using IIS Express:
1. Ensure you have Visual Studio installed with IIS Express.
1. Clone or open this repository in Visual Studio.
1. Make sure IIS Express is selected as the debug option (next to the green play button on the toolbar).
1. Press the green play button or F5 to start debugging.
1. The application will launch using IIS Express and should be available at http://localhost:43811 and https://localhost:44305.

# Example Usage:
For Kestrel: https://localhost:7104/api/countries?param1=Canada
For IIS Express: http://localhost:43811/api/countries?param1=Canada
Here are some example URLs to test the developed endpoint:

1. Fetch all countries: https://localhost:7104/api/countries
1. Filter countries by name (e.g., "Canada"): https://localhost:7104/api/countries?param1=Canada
1. Filter countries by a minimum population of 50 million: https://localhost:7104/api/countries?number=50000000
1. Filter countries by region (e.g., "Asia"): https://localhost:7104/api/countries?param3=Asia
1. Fetch countries sorted in descending order by name: https://localhost:7104/api/countries?sortOrder=descend
1. Limit the number of returned countries to 15: https://localhost:7104/api/countries?limit=15
1. Search countries by common name (e.g., "India"): https://localhost:7104/api/countries/search?name=India
1. Retrieve countries with a population less than 10 million: https://localhost:7104/api/countries/populationLessThanMillion?value=10
1. Combine multiple filters, e.g., countries in Asia with a population of 50 million or more, sorted in descending order: https://localhost:7104/api/countries?param3=Asia&number=50000000&sortOrder=descend
1. Combine multiple filters and limit the results to 5 countries: https://localhost:7104/api/countries?param3=Asia&number=50000000&sortOrder=descend&limit=5
