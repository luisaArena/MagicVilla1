using AutoMapper;
using MagicVilla_Api1.Datos;
using MagicVilla_Api1.Modelos;
using MagicVilla_Api1.Modelos.DTO;
using MagicVilla_Api1.Modelos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_Api1.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class NumeroVillaController : Controller
    {

        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _NumeroRepo;
        private readonly IMapper _mapper;
        private IMapper mapper;
        protected ApiResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, 
                                                      INumeroVillaRepositorio numeroRepo, IMapper mapper) 
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _NumeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }    


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener los numeros Villas");

                IEnumerable<NumeroVilla> NumerovillaList = await _NumeroRepo.ObtenerTodo();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(NumerovillaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()};
            }

            return _response;
         
        }

        [HttpGet("id", Name ="GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer numero villa con id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest; 
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numerovilla = await _NumeroRepo.Obtener(x => x.VillaNo == id);

                if (numerovilla == null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }

                _response.Resultado= _mapper.Map<NumeroVillaDto>(numerovilla);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString()};    
            }

            return _response;
        }

        [HttpPost("id")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto CreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _NumeroRepo.Obtener(v => v.VillaNo == CreateDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El numero villa ya existe");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id == CreateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
                    return BadRequest(ModelState);
                }

                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }

                NumeroVilla Modelo = _mapper.Map<NumeroVilla>(CreateDto);
                Modelo.FechaCreacion = DateTime.Now;
                Modelo.FechaActualizacion = DateTime.Now;

                await _NumeroRepo.Crear(Modelo);
                _response.Resultado = Modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetNumeroVilla", new { id = Modelo.VillaNo },_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()};
            }

            return _response;
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteNumeroVilla(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Numerovilla = await _NumeroRepo.Obtener(x => x.VillaNo == id);

                if (Numerovilla == null)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

               await _NumeroRepo.Remover(Numerovilla);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);           
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateNumeroVilla(int id,[FromBody] NumeroVillaUpdateDto UpdateDto)
        { 
            if (UpdateDto==null || id != UpdateDto.VillaNo)
            {
                _response.IsExitoso=!false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Obtener(v => v.Id == UpdateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
                return BadRequest(ModelState);
            }

            NumeroVilla Modelo = _mapper.Map<NumeroVilla>(UpdateDto);

            await _NumeroRepo.Actualizar(Modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
             return Ok(_response);
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

            var villa = await _villaRepo.Obtener(x => x.Id == id, tracked:false);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
                       
            if(villa == null) return BadRequest();

            PatchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            Villa Modelo = _mapper.Map<Villa>(villaDto);

           
            await _villaRepo.Actualizar(Modelo);
            _response.StatusCode=HttpStatusCode.NoContent;
             return Ok(_response);
        }

    }
}
