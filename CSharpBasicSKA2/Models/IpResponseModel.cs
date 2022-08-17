using System.Text.Json.Serialization;

namespace CSharpBasicSKA2.Models;

public class IpResponseModel
{
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
}