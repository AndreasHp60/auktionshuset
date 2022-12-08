using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Http.Features; 
 using System.Diagnostics; 

namespace Loadbalancer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadbalancerController : ControllerBase
{
 [HttpGet("version")] 
 public async Task<Dictionary<string,string>> GetVersion() 
 { 
     var properties = new Dictionary<string, string>(); 
     var assembly = typeof(Program).Assembly; 
 
        properties.Add("service", "Weather Forecast"); 
     var ver = FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion; 
        properties.Add("version", ver); 
 
     var hostName = System.Net.Dns.GetHostName(); 
     var ips = await System.Net.Dns.GetHostAddressesAsync(hostName); 
     var ipa = ips.First().MapToIPv4().ToString(); 
        properties.Add("ip-address", ipa); 
 
     return properties; 
 } 
}
