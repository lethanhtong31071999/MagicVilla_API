using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _service;
        public VillaController(IVillaService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _service.GetAllAsync<APIResponse>();
            var model = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result));
            return View();
        }
    }
}
