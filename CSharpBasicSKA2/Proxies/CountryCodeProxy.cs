using System.Text.Json;
using CSharpBasicSKA2.Controllers;
using CSharpBasicSKA2.Models;
using Polly;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace CSharpBasicSKA2.Proxies;

public class CountryCodeProxy
{
    private readonly string url = "http://ip-api.com/batch";

    private readonly HttpClient _httpClient;
    
    private readonly ILogger _logger;

    private readonly List<CountryCodeRequestModel> _requestBody;

    public CountryCodeProxy(HttpClient httpClient, ILogger<WhereAmIController> logger, List<CountryCodeRequestModel> requestBody)
    {
        _httpClient = httpClient;
        _logger = logger;
        _requestBody = requestBody;
    }
    public async Task<string> GetCountryCode(string ip)
    {
        _requestBody.Add(new CountryCodeRequestModel
        {
            Query = ip,
        });

        var asyncRetryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .RetryAsync(3,
                onRetry: (exception, retryCount) =>
                {
                    _logger.LogError("Polly Retry Times:{RetryCount}, {ExceptionMessage}", retryCount,
                        exception.Exception.Message);
                });
        
        var responseMessage = await asyncRetryPolicy.ExecuteAsync(async () =>  await _httpClient.PostAsync(
            url,
            JsonContent.Create(
                _requestBody,
                MediaTypeHeaderValue.Parse("application/json"),
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }
            )));

        if (!responseMessage.IsSuccessStatusCode)
            throw new Exception($"{nameof(CountryCodeProxy)}\\{nameof(GetCountryCode)} request failed");
        
        var responseString = await responseMessage.Content.ReadAsStringAsync();

        var countryCodeResponseModel = JsonSerializer.Deserialize<List<CountryCodeResponseModel>>(responseString);
        _logger.LogInformation($"{nameof(CountryCodeProxy)}\\{nameof(GetCountryCode)} success");
        
        return countryCodeResponseModel[0].CountryCode;
    }
}