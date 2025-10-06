using Microsoft.AspNetCore.Mvc;
using BancoTurnosApp.src.models;
using BancoTurnosApp.src.services;

namespace BancoTurnosApp.src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        public TurnosController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        // GET: api/turnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> GetAll()
        {
            var turnos = await _turnoService.GetAllTurnosAsync();
            return Ok(turnos);
        }

        // GET: api/turnos/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Turno>> GetByCodigo(string codigo)
        {
            var turno = await _turnoService.BuscarPorCodigoAsync(codigo);
            if (turno == null)
                return NotFound($"No se encontró turno con código {codigo}");
            return Ok(turno);
        }

        // GET: api/turnos/estado/{estado}
        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<Turno>>> GetByEstado(EstadoTurno estado)
        {
            var turnos = await _turnoService.ObtenerTurnosPorEstadoAsync(estado);
            return Ok(turnos);
        }

        // POST: api/turnos?cedulaCliente=123&codigoServicio=TG
        [HttpPost]
        public async Task<ActionResult<Turno>> Create([FromQuery] string cedulaCliente, [FromQuery] string codigoServicio)
        {
            try
            {
                var nuevoTurno = await _turnoService.CrearTurnoAsync(cedulaCliente, codigoServicio);
                return CreatedAtAction(nameof(GetByCodigo), new { codigo = nuevoTurno.Codigo }, nuevoTurno);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/turnos/{codigo}/estado?nuevoEstado=Atendiendo
        [HttpPut("{codigo}/estado")]
        public async Task<ActionResult<Turno>> CambiarEstado(string codigo, [FromQuery] EstadoTurno nuevoEstado)
        {
            try
            {
                var turno = await _turnoService.CambiarEstadoTurnoAsync(codigo, nuevoEstado);
                if (turno == null)
                    return NotFound($"No se encontró turno con código {codigo}");
                return Ok(turno);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}