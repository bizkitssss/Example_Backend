using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.Controllers.Api
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new IndexModel
            {
                IP = Dns.GetHostEntry(Dns.GetHostName())?.HostName ?? "",
                AssemblyName = AppSetting.AssemblyName,
                AssemblyVersion = AppSetting.AssemblyVersion,
                AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                DateModified = AppSetting.DateModified
            });
        }
    }
}
