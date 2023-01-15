using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public VillaApiController(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        // This is for swagger
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET - GET VILLAS
        public ActionResult<IReadOnlyList<VillaOutputDto>> GetVillas()
        {
            return Ok(_mapper.Map<IReadOnlyList<VillaOutputDto>>(_context.Villas.ToList()));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        // // GET - GET VILLA
        public ActionResult<VillaOutputDto> GetVilla(int id)
        {
            Villa villa = _context.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null) return NotFound();

            return Ok(_mapper.Map<VillaOutputDto>(villa));
        }

        [HttpPost]
        // POST - CREATE VILLA
        public ActionResult<VillaOutputDto> CreateVilla([FromBody] VillaInputDto villaInputDto)
        {
            if (villaInputDto == null) return BadRequest(villaInputDto);

            // Check villa name is unique
            if (_context.Villas.FirstOrDefault(v => v.Name.ToLower() == villaInputDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
                //return BadRequest("Villa name already exists");
            }

            Villa villa = _mapper.Map<VillaInputDto, Villa>(villaInputDto);

            _context.Villas.Add(villa);

            _context.SaveChanges();

            // In the response the location points to the new created record, using the GetVilla(int id) endpoint.
            // That's why we pass the Id along
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _mapper.Map<Villa, VillaOutputDto>(villa));
        }

        [HttpDelete("{id:int}")]
        // DELETE - DELETE VILLA
        public IActionResult DeleteVilla(int id)
        {
            Villa villa = _context.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null) return NotFound();

            // Remove the item from the list
            _context.Villas.Remove(villa);

            _context.SaveChanges();

            return NoContent();

        }

        [HttpPut("{id:int}")]
        public ActionResult<VillaOutputDto> UpdateVilla(int id, [FromBody] VillaInputDto villaInputDto)
        {
            if (villaInputDto == null) return BadRequest();

            Villa villa = _mapper.Map<VillaInputDto, Villa>(villaInputDto);
            if(villa == null) return NotFound();

            // Update properties
            _context.Villas.Update(villa);

            _context.SaveChanges();

            return _mapper.Map<Villa, VillaOutputDto>(villa);
        }
    }
}