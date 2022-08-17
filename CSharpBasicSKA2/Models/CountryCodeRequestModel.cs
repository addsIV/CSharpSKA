using System.Text.Json.Serialization;

namespace CSharpBasicSKA2.Models;

public class CountryCodeRequestModel
{
    [JsonPropertyName("query")]
    public string Query { get; set; }
}