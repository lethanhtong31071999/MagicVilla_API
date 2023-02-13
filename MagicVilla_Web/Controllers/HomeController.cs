using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVillaService _villaService;

        public HomeController(ILogger<HomeController> logger, IVillaService villaService)
        {
            _logger = logger;
            _villaService = villaService;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _villaService.GetAllAsync<APIResponse>();
            if(res != null && res.IsSuccess)
            {
                return View(JsonConvert.DeserializeObject<List<VillaDTO>>(JsonConvert.SerializeObject(res.Result)));
            }
            return View(new List<VillaDTO>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}