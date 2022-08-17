using System.Text.Json.Serialization;

namespace CSharpBasicSKA2.Models;

public class WhereAmIModel
{
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
    
    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }
}