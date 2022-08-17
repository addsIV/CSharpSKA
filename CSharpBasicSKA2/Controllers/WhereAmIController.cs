using System.Net;
using CSharpBasicSKA2.Models;
using CSharpBasicSKA2.Proxies;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace CSharpBasicSKA2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhereAmIController : ControllerBase
{
    private readonly ILogger<WhereAmIController> _logger;

    private IpProxy _ipProxy;

    private CountryCodeProxy _countryCodeProxy;

    public WhereAmIController(ILogger<WhereAmIController> logger, IpProxy ipProxy, CountryCodeProxy countryCodeProxy)
    {
        _logger = logger;
        _ipProxy = ipProxy;
        _countryCodeProxy = countryCodeProxy;
    }

    [HttpGet(Name = "WhereAmI")]
    public async Task<WhereAmIModel> GetWhereAmI()
    {
        string ip = await _ipProxy.GetIp();
        string countryCode = await _countryCodeProxy.GetCountryCode(ip);
        
        return new WhereAmIModel()
       {
           Ip = ip,
           CountryCode = countryCode,
       };
    }
}