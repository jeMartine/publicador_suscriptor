using Microsoft.AspNetCore.Mvc;
using sbBancoTurnos.src.models;
using sbBancoTurnos.src.services;

namespace sbBancoTurnos.src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/{cedula}
        [HttpGet("{cedula}")]
        public async Task<ActionResult<Cliente>> GetByCedula(string cedula)
        {
            var cliente = await _clienteService.BuscarPorCedulaAsync(cedula);
            if (cliente == null)
                return NotFound($"No se encontró cliente con cédula {cedula}");
            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] Cliente cliente)
        {
            try
            {
                var nuevoCliente = await _clienteService.RegistrarClienteAsync(cliente);
                return CreatedAtAction(nameof(GetByCedula), new { cedula = nuevoCliente.Cedula }, nuevoCliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
