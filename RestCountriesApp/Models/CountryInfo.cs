namespace RestCountriesApp.Models
{
    public class CountryInfo
    {
        public NameInfo Name { get; set; }

        public List<string> Capital { get; set; }
        public string Cioc { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public string Demonym { get; set; }
        public Flags Flags { get; set; }
        public string Id { get; set; } // Note: In the API, this is the country's alpha-3 code
        public bool Independent { get; set; }
        public string Landlocked { get; set; }
        public Dictionary<string, string> Languages { get; set; }
        public Dictionary<string, string> Maps { get; set; }

        public int Population { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public List<string> Tld { get; set; }
        public Dictionary<string, TranslationDetail> Translations { get; set; }
        public bool UnMember { get; set; }
    }

    public class NameInfo
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }

    public class Flags
    {
        public string Png { get; set; }
        public string Svg { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    public class Translation
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

    public class TranslationDetail
    {
        public string SomeKey { get; set; }
        // Add other properties based on the JSON structure
    }
}