using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Models.Dto.VillaNumberDTO;
using MagicVilla_Web.Models.ViewModels;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> IndexVillaNumber()
        {
            var res = await _villaNumberService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(JsonConvert.SerializeObject(res.Result));
                return View(model);
            }
            return BadRequest(res);
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> CreateVillaNumber()
        {
            var res = await _villaService.GetAllAsync<APIResponse>();
            if(res != null && res.IsSuccess)
            {
                var viewModel = new VillaNumberCreateVM();
                var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(JsonConvert.SerializeObject(res.Result));
                if(villaList != null)
                {
                    viewModel.VillaList = villaList.Select(villa => new SelectListItem()
                    {
                        Value = villa.Id.ToString(),
                        Text = villa.Name,
                    });
                    return View(viewModel);
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber([FromForm] VillaNumberCreateVM villaNumberCreateVM)
        {
            if (ModelState.IsValid)
            {
                var res = await _villaNumberService.CreateAsync<APIResponse>(villaNumberCreateVM.VillaNumber);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            var villaNumberCreateDTO = villaNumberCreateVM.VillaNumber;
            var villaRes = await _villaService.GetAllAsync<APIResponse>();
            if (villaRes != null && villaRes.IsSuccess)
            {
                var viewModel = new VillaNumberCreateVM() { };
                var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(JsonConvert.SerializeObject(villaRes.Result));
                if (villaList != null && villaNumberCreateDTO != null)
                {
                    viewModel.VillaNumber = villaNumberCreateDTO;
                    viewModel.VillaList = villaList.Select(villa =>
                    {
                        bool isSelected = villaNumberCreateDTO.VillaId == villa.Id;
                        return new SelectListItem()
                        {
                            Value = villa.Id.ToString(),
                            Text = villa.Name,
                            Selected = isSelected,
                        };
                    });
                    return View(viewModel);
                }
            }
            return BadRequest();
        }

        // UPDATE
        [HttpGet]
        public async Task<IActionResult> UpdateVillaNumber([FromQuery] int villaNo)
        {
            var viewModel = new VillaNumberUpdateVM();
            var res = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (res != null && res.IsSuccess)
            {
                viewModel.VillaNumber = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(JsonConvert.SerializeObject(res.Result));
            }

            // Get villa select
            var villaRes = await _villaService.GetAllAsync<APIResponse>();
            if (villaRes != null && villaRes.IsSuccess)
            {
                var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(JsonConvert.SerializeObject(villaRes.Result));
                if (villaList != null)
                {
                    viewModel.VillaList = villaList.Select(villa => new SelectListItem()
                    {
                        Value = villa.Id.ToString(),
                        Text = villa.Name,
                        Selected = villa.Id == viewModel.VillaNumber.VillaId,
                    });
                    return View(viewModel);
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber([FromForm]VillaNumberUpdateVM villaNumberUpdateVM)
        {
            if (ModelState.IsValid)
            {
                var res = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberUpdateVM.VillaNumber);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            var villaRes = await _villaService.GetAllAsync<APIResponse>();
            if (villaRes != null && villaRes.IsSuccess)
            {
                var villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(JsonConvert.SerializeObject(villaRes.Result));
                if (villaList != null)
                {
                    villaNumberUpdateVM.VillaList = villaList.Select(villa => new SelectListItem()
                    {
                        Value = villa.Id.ToString(),
                        Text = villa.Name,
                        Selected = villa.Id == villaNumberUpdateVM.VillaNumber.VillaId,
                    });
                    return View(villaNumberUpdateVM);
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteVillaNumber([FromQuery] int villaNo)
        {
            var res = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (res != null && res.IsSuccess)
            {
                return View(JsonConvert.DeserializeObject<VillaNumberDTO>(JsonConvert.SerializeObject(res.Result)));
            }
          
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVillaNumber([FromForm] VillaNumberDTO villaNumberDTO)
        {
            var res = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberDTO.VillaNo);
            if (res != null && res.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return BadRequest(res);

        }
    }
}
