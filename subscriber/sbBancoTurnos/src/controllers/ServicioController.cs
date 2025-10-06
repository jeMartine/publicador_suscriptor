using Microsoft.AspNetCore.Mvc;
using sbBancoTurnos.src.models;
using sbBancoTurnos.src.services;

namespace sbBancoTurnos.src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetAll()
        {
            var servicio = await _servicioService.GetAllServiciosAsync();
            return Ok(servicio);
        }

        // GET: api/servicios/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Servicio>> GetByCodigo(string codigo)
        {
            var servicio = await _servicioService.BuscarPorCodigoAsync(codigo);
            if (servicio == null)
                return NotFound($"No se encontró servicio con código {codigo}");
            return Ok(servicio);
        }

        // POST: api/servicios
        [HttpPost]
        public async Task<ActionResult<Servicio>> Create([FromBody] Servicio servicio)
        {
            try
            {
                var nuevoServicio = await _servicioService.RegistrarServicioAsync(servicio);
                return CreatedAtAction(nameof(GetByCodigo), new { codigo = nuevoServicio.Codigo }, nuevoServicio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/servicios/{codigo}
        [HttpPut("{codigo}")]
        public async Task<ActionResult<Servicio>> Update(string codigo, [FromBody] Servicio servicio)
        {
            try
            {
                var servicioActualizado = await _servicioService.ActualizarServicioAsync(codigo, servicio);
                if (servicioActualizado == null)
                    return NotFound($"No se encontró servicio con código {codigo}");
                return Ok(servicioActualizado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}