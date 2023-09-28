using AutoMapper;
using MagicVilla_Api1.Datos;
using MagicVilla_Api1.Modelos;
using MagicVilla_Api1.Modelos.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_Api1.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class VillaController : Controller
    {

        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private IMapper mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper) 
        {
            _logger = logger; 
            _db = db;
            _mapper = mapper;
        }    


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");

            IEnumerable<Villa> villaList = await _db.villas.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
         
        }

        [HttpGet("id", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Error al traer villa con id" + id);
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.villas.FirstOrDefaultAsync(x => x.Id == id);
                
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost("id")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto CreateDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _db.villas.FirstOrDefaultAsync(v=>v.Nombre.ToLower()== CreateDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }
                
            if(CreateDto == null)
            {
                return BadRequest(CreateDto);
            }

            Villa Modelo = _mapper.Map<Villa>(CreateDto);
                      
            await _db.villas.AddAsync(Modelo);  
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new {id = Modelo.Id}, Modelo);
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteVilla(int id) 
        { 
            var villa = await _db.villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _db.villas.Remove(villa);
            await _db.SaveChangesAsync();  

            return NoContent();
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateVilla(int id,[FromBody] VillaUpdateDto UpdateDto)
        { 
            if (UpdateDto==null || id != UpdateDto.Id)
            {
                return BadRequest();
            }

            //var villa = _db.villas.FirstOrDefault(x => x.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa Modelo = _mapper.Map<Villa>(UpdateDto);



            _db.villas.Update(Modelo);
            await _db.SaveChangesAsync();
            return NoContent() ;
        }

        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> PatchDto)
        {
            if (PatchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
                       
            if(villa == null) return BadRequest();

            PatchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            Villa Modelo = _mapper.Map<Villa>(villaDto);

           
            _db.villas.Update(Modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
