using System.Text.Json.Serialization;

namespace CSharpBasicSKA2.Models;

public class CountryCodeResponseModel
{
    [JsonPropertyName("countryCode")] 
    public string CountryCode { get; set; }
}