using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVillas()
        {
            _logger.LogInformation("Get all villas");
            var villaDba = _db.Villas.AsEnumerable();
            return Ok(_mapper.Map<List<VillaDTO>>(villaDba));
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> GetVilla([FromRoute] int id)
        {
            if (id == 0)
            {
                _logger.LogError($"Get villa error with Id: {id}");
                return BadRequest("Id is not 0");
            }
            var villaDba = await _db.Villas.FirstOrDefaultAsync<Villa>(x => x.Id == id);
            if (villaDba == null)
            {
                _logger.LogError($"Not found villa Id: {id}");
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villaDba));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaCreateDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<IActionResult> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            if (villaCreateDTO == null) return BadRequest();
            var isUnique = _db.Villas.FirstOrDefault(x => x.Name == villaCreateDTO.Name) == null;
            if (!isUnique)
            {
                ModelState.AddModelError("Villa_Name", "Have already existed!");
                return BadRequest(ModelState);
            }
            var newVilla = _mapper.Map<Villa>(villaCreateDTO);
            newVilla.CreatedAt = DateTime.Now;
            await _db.Villas.AddAsync(newVilla);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, newVilla);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        async public Task<IActionResult> DeleteVilla([FromRoute] int id)
        {
            if (id == 0) return BadRequest();
            var villaDba = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villaDba == null) return NotFound();
            _db.Villas.Remove(villaDba);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "PutVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        async public Task<IActionResult> UpdateVilla([FromRoute] int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            if (villaUpdateDTO == null || id != villaUpdateDTO.Id) return BadRequest();
            var isUnique = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Name == villaUpdateDTO.Name) == null;
            if (!isUnique)
            {
                ModelState.AddModelError("Villa_Name", "Have already existed!");
                return BadRequest(ModelState);
            }
            var villaDba = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (villaDba == null) return NotFound();
            villaDba = _mapper.Map<Villa>(villaUpdateDTO);
            villaDba.UpdatedAt = DateTime.UtcNow;
            _db.Villas.Update(villaDba);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "PatchVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> UpdatePartialVilla([FromRoute] int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {
            var villaDba = _db.Villas.AsNoTracking().FirstOrDefault(x => id == x.Id);
            if (villaDba == null) return NotFound();
            var villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villaDba);
            patch.ApplyTo(villaUpdateDTO);
            var isUnique = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Name == villaUpdateDTO.Name) == null;
            if (!isUnique)
            {
                ModelState.AddModelError("Villa_Name", "Have already existed!");
                return BadRequest(ModelState);
            }
            villaDba = _mapper.Map<Villa>(villaUpdateDTO);
            // Check Update At
            villaDba.UpdatedAt = DateTime.UtcNow;
            _db.Villas.Update(villaDba);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
