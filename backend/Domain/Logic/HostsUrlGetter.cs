using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Domain.Logic;

public static class HostsUrlGetter
{
    public static readonly string ApiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5051";
    public static readonly string ApplicationUrl = Environment.GetEnvironmentVariable("APPLICATION_URL") ?? "http://localhost:5011";
    public static readonly string PyServerUrl = Environment.GetEnvironmentVariable("PYSERVER_URL") ?? "http://localhost:5059";
}