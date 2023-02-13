using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _service;
        private readonly IMapper _mapper;
        public VillaController(IVillaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET ALL
        public async Task<IActionResult> IndexVilla()
        {
            var result = new List<VillaDTO>();
            var res = await _service.GetAllAsync<APIResponse>();
            if (res != null || res.IsSuccess)
            {
                result = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result));

            }
            return View(result);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetVilla()
        {
            var res = await _service.GetAllAsync<APIResponse>();
            if (res != null || res.IsSuccess)
            {
                return View(JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(res.Result)));

            }
            TempData["error"] = "Something went wrong!";
            return View(new VillaDTO());
        }

        // CREATE
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString(SD.SessionToken);
                var res = await _service.CreateAsync<APIResponse>(model, token);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View();
        }

        // REMOVE
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var res = await _service.GetAsync<APIResponse>(id);
            if(res != null && res.IsSuccess)
            {
                return View(JsonConvert.DeserializeObject<VillaDTO>(JsonConvert.SerializeObject(res.Result)));
            }
            return NotFound(res);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(int id, bool trash = true)
        {
            if (id > 0)
            {
                var token = HttpContext.Session.GetString(SD.SessionToken);
                var res = await _service.DeleteAsync<APIResponse>(id, token);
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = $"Removed successfully item with id {id}";
                    return RedirectToAction(nameof(IndexVilla));
                }
                else
                {
                    TempData["error"] = $"Failed to remove item with id {id}";
                    return BadRequest(res);
                }
            }
            TempData["error"] = $"Id {id} is not valid!";
            return RedirectToAction(nameof(IndexVilla));
        }

        // UPDATE
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var res = await _service.GetAsync<APIResponse>(id);
            if(res != null && res.IsSuccess)
            {
                var villaDTO = JsonConvert.DeserializeObject<VillaDTO>(JsonConvert.SerializeObject(res.Result));
                return View(_mapper.Map<VillaUpdateDTO>(villaDTO));
            }
            return NotFound(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString(SD.SessionToken);
                var res = await _service.UpdateAsync<APIResponse>(villaUpdateDTO, token);
                if(res != null && res.IsSuccess)
                {
                    TempData["success"] = $"Updated successfully item with id {villaUpdateDTO.Id}";
                    return RedirectToAction(nameof(IndexVilla));
                }
                else
                {
                    TempData["error"] = $"Failed to remove item with id {villaUpdateDTO.Id}";
                    return BadRequest(res);
                }
            }
            TempData["error"] = $"Something went wrong!";
            return View();
        }
    }
}
