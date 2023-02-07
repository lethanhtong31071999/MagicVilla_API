using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Repository.IRepository;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IVillaRepo _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepo villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Get all villas");
                var villaDba = await _villaRepo.GetAll();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaDba);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<ActionResult<APIResponse>> GetVilla([FromRoute] int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError($"Get villa error with Id: {id}");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaDba = await _villaRepo.Get(x => x.Id == id);
                if (villaDba == null)
                {
                    _logger.LogError($"Not found villa Id: {id}");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDTO>(villaDba);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                if (villaCreateDTO == null) return BadRequest();
                var isUnique = await _villaRepo.Get(x => x.Name == villaCreateDTO.Name, isTracked: false) == null;
                if (!isUnique)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var newVilla = _mapper.Map<Villa>(villaCreateDTO);
                newVilla.CreatedAt = DateTime.Now;
                await _villaRepo.Create(newVilla);
                await _villaRepo.Save();
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = newVilla;
                return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        async public Task<ActionResult<APIResponse>> DeleteVilla([FromRoute] int id)
        {
            try
            {
                if (id == 0) return BadRequest();
                var villaDba = await _villaRepo.Get(x => x.Id == id);
                if (villaDba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _villaRepo.Remove(villaDba);
                await _villaRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "PutVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        async public Task<ActionResult<APIResponse>> UpdateVilla([FromRoute] int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.Id) return BadRequest();
                var isUnique = await _villaRepo.Get(x => x.Name == villaUpdateDTO.Name, isTracked: false) == null;
                if (!isUnique)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaDba = await _villaRepo.Get(x => x.Id == id, isTracked: false);
                if (villaDba == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                villaDba = _mapper.Map<Villa>(villaUpdateDTO);
                villaDba.UpdatedAt = DateTime.UtcNow;
                _villaRepo.Update(villaDba);
                await _villaRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "PatchVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<ActionResult<APIResponse>> UpdatePartialVilla([FromRoute] int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {
            try
            {
                var villaDba = await _villaRepo.Get(x => x.Id == id, isTracked: false);
                if (villaDba == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villaDba);
                patch.ApplyTo(villaUpdateDTO);
                var isUnique = await _villaRepo.Get(x => x.Name == villaUpdateDTO.Name && x.Id != villaUpdateDTO.Id, isTracked: false) == null;
                if (!isUnique)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                villaDba = _mapper.Map<Villa>(villaUpdateDTO);
                villaDba.UpdatedAt = DateTime.UtcNow;
                _villaRepo.Update(villaDba);
                await _villaRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
