using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.VillaDTO;
using MagicVilla_VillaAPI.Models.Dto.VillaNumberDTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepo _villaNumberRepo;
        private readonly IVillaRepo _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaNumberAPIController(IVillaNumberRepo villaNumberRepo, IMapper mapper, IVillaRepo villaRepo)
        {
            _villaNumberRepo = villaNumberRepo;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<VillaNumberDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                var villaNumberFromDba = await _villaNumberRepo.GetAll(includedProps: "villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberFromDba);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpGet("{villaNo}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaNumberDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> Get([FromRoute]int villaNo)
        {
            try
            {
                var villaNumberFromDba = await _villaNumberRepo.Get(x => x.VillaNo == villaNo, includedProps: "villa");
                if (villaNumberFromDba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumberFromDba);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Route("{villaNo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> Delete([FromRoute] int villaNo)
        {
            try
            {
                var villaNumberFromDba = await _villaNumberRepo.Get(x => x.VillaNo == villaNo);
                if (villaNumberFromDba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _villaNumberRepo.Remove(villaNumberFromDba);
                await _villaNumberRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaNumberDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> Create(VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                // Check null, valid VillaId, and VillaNo is Unique
                if (villaNumberCreateDTO == null ||
                    ((await _villaRepo.Get(x => x.Id == villaNumberCreateDTO.VillaId)) == null))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                bool isVillaNoUnique = await _villaNumberRepo.Get(x => x.VillaNo == villaNumberCreateDTO.VillaNo, isTracked: false) == null;
                if (!isVillaNoUnique)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "VillaNo must be unique!" };
                    return BadRequest(_response);
                }

                // Create
                var newVillaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
                newVillaNumber.CreateAt = DateTime.Now;
                await _villaNumberRepo.Create(newVillaNumber);
                await _villaNumberRepo.Save();

                // Response
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaNumberDTO>(newVillaNumber);
                return CreatedAtRoute("GetVillaNumber", new { villaNo = newVillaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut]
        [Route("{villaNo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> Update([FromRoute] int villaNo, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                // Check valid VillaId and UpdateDTO
                if (villaNumberUpdateDTO == null
                    || villaNo != villaNumberUpdateDTO.VillaNo
                    || ((await _villaRepo.Get(x => x.Id == villaNumberUpdateDTO.VillaId)) == null))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumberFromDba = await _villaNumberRepo.Get(x => x.VillaNo == villaNumberUpdateDTO.VillaNo, isTracked: false);
                if (villaNumberFromDba == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                villaNumberFromDba = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                _villaNumberRepo.Update(villaNumberFromDba);
                await _villaNumberRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch]
        [Route("{villaNo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> UpdatePartially([FromRoute] int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> patch)
        {
            try
            {
                var villaNumberFromDba = await _villaNumberRepo.Get(x => x.VillaNo == villaNo, isTracked: false);
                if (villaNumberFromDba == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                // Create DTO to update new fileds by PatchJson
                var villaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumberFromDba);
                patch.ApplyTo(villaNumberUpdateDTO);
                // Check valid VillaId and the new VillaNo = the previous VillaNo
                if (villaNo != villaNumberUpdateDTO.VillaNo
                    || (await _villaRepo.Get(x => x.Id == villaNumberUpdateDTO.VillaId)) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                villaNumberFromDba = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                _villaNumberRepo.Update(villaNumberFromDba);
                await _villaNumberRepo.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
