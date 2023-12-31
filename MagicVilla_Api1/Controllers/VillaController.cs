﻿using AutoMapper;
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
    public class VillaController : Controller
    {

        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        private IMapper mapper;
        protected ApiResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper) 
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }    


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las Villas");

                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodo();

                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
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

        [HttpGet("id", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer villa con id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest; 
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.Obtener(x => x.Id == id);

                if (villa == null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }

                _response.Resultado= _mapper.Map<VillaDto>(villa);
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

        public async Task<ActionResult<ApiResponse>> CrearVilla([FromBody] VillaCreateDto CreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == CreateDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }

                Villa Modelo = _mapper.Map<Villa>(CreateDto);
                Modelo.FechaCreacion = DateTime.Now;
                Modelo.FechaActualizacion = DateTime.Now;

                await _villaRepo.Crear(Modelo);
                _response.Resultado = Modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetVilla", new { id = Modelo.Id },_response);

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

        public async Task<ActionResult> DeleteVilla(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Obtener(x => x.Id == id);

                if (villa == null)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

               await _villaRepo.Remover(villa);
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

        public async Task<IActionResult> UpdateVilla(int id,[FromBody] VillaUpdateDto UpdateDto)
        { 
            if (UpdateDto==null || id != UpdateDto.Id)
            {
                _response.IsExitoso=!false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
          
            Villa Modelo = _mapper.Map<Villa>(UpdateDto);

            await _villaRepo.Actualizar(Modelo);
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
