using System.Text.Json;
using CSharpBasicSKA2.Controllers;
using CSharpBasicSKA2.Models;
using Polly;

namespace CSharpBasicSKA2.Proxies;

public class IpProxy
{
    private readonly string url = "https://api.ipify.org?format=json";

    private readonly HttpClient _httpClient;

    private readonly ILogger _logger;

    public IpProxy(HttpClient httpClient, ILogger<WhereAmIController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetIp()
    {
        var asyncRetryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .RetryAsync(3,
                onRetry: (exception, retryCount) =>
                {
                    _logger.LogError("Polly Retry Times:{RetryCount}, {ExceptionMessage}", retryCount,
                        exception.Exception.Message);
                });

        var responseMessage = await asyncRetryPolicy.ExecuteAsync(async () => await _httpClient.GetAsync(url));
        if (!responseMessage.IsSuccessStatusCode)
            throw new Exception($"{nameof(IpProxy)}\\{nameof(GetIp)} request failed");

        var responseString = await responseMessage.Content.ReadAsStringAsync();

        var ipModel = JsonSerializer.Deserialize<IpResponseModel>(responseString);
        _logger.LogInformation($"{nameof(IpProxy)}\\{nameof(GetIp)} success");

        return ipModel.Ip;
    }
}