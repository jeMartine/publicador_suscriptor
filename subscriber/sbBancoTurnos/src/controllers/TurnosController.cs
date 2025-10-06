using Microsoft.AspNetCore.Mvc;
using sbBancoTurnos.src.models;
using sbBancoTurnos.src.services;

namespace sbBancoTurnos.src.controllers
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

        // POST: api/turnos
        [HttpPost]
        public async Task<ActionResult<Turno>> Create([FromBody] CrearTurnoRequest request)
        {
            try
            {
                var nuevoTurno = await _turnoService.CrearTurnoConClienteAsync(request.Cliente, request.CodigoServicio);
                return CreatedAtAction(nameof(GetByCodigo), new { codigo = nuevoTurno.Codigo }, nuevoTurno);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/turnos/{codigo}/estado
        [HttpPut("{codigo}/estado")]
        public async Task<ActionResult<Turno>> CambiarEstado(string codigo, [FromBody] CambiarEstadoRequest request)
        {
            try
            {
                var turno = await _turnoService.CambiarEstadoTurnoAsync(codigo, request.NuevoEstado);
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

    // Clases auxiliares para recibir JSON
    public class CrearTurnoRequest
    {
        public required ClienteRequest Cliente { get; set; }
        public required string CodigoServicio { get; set; }
    }

    public class ClienteRequest
    {
        public required string Cedula { get; set; }
        public required string Nombre { get; set; }
        public required bool RequiereAsistencia { get; set; }
    }

    public class CambiarEstadoRequest
    {
        public required EstadoTurno NuevoEstado { get; set; }
    }
}