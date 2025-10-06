using BancoTurnosApp.src.models;
using BancoTurnosApp.src.repositories;

namespace BancoTurnosApp.src.services
{
    public interface ITurnoService
    {
        Task<IEnumerable<Turno>> GetAllTurnosAsync();
        Task<Turno> CrearTurnoAsync(string cedulaCliente, string codigoServicio);
        Task<Turno?> BuscarPorCodigoAsync(string codigo);
        Task<IEnumerable<Turno>> ObtenerTurnosPorEstadoAsync(EstadoTurno estado);
        Task<Turno?> CambiarEstadoTurnoAsync(string codigo, EstadoTurno nuevoEstado);
    }

    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _turnoRepo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IServicioRepository _servicioRepo;

        public TurnoService(
            ITurnoRepository turnoRepo,
            IClienteRepository clienteRepo,
            IServicioRepository servicioRepo)
        {
            _turnoRepo = turnoRepo;
            _clienteRepo = clienteRepo;
            _servicioRepo = servicioRepo;
        }

        public async Task<IEnumerable<Turno>> GetAllTurnosAsync()
        {
            return await _turnoRepo.GetAllAsync();
        }

        public async Task<Turno?> BuscarPorCodigoAsync(string codigo)
        {
            return await _turnoRepo.GetByCodigoAsync(codigo);
        }

        public async Task<IEnumerable<Turno>> ObtenerTurnosPorEstadoAsync(EstadoTurno estado)
        {
            return await _turnoRepo.GetByEstadoAsync(estado);
        }

        public async Task<Turno> CrearTurnoAsync(string cedulaCliente, string codigoServicio)
        {
            // Validar que exista el cliente
            var cliente = await _clienteRepo.GetByCedulaAsync(cedulaCliente);
            if (cliente == null)
                throw new InvalidOperationException($"No existe cliente con cédula {cedulaCliente}");

            // Validar que exista el servicio
            var servicio = await _servicioRepo.GetByCodigoAsync(codigoServicio);
            if (servicio == null)
                throw new InvalidOperationException($"No existe servicio con código {codigoServicio}");

            // Generar código del turno (ejemplo: TG01, SD02, TGA03)
            var consecutivo = await _turnoRepo.ContarTurnosPorServicioHoyAsync(servicio.Codigo) + 1;
            var prefijo = cliente.RequiereAsistencia ? $"{servicio.Codigo}A" : servicio.Codigo;
            var codigoTurno = $"{prefijo}{consecutivo:D2}";

            var nuevoTurno = new Turno
            {
                Id = Guid.NewGuid().ToString(),
                Codigo = codigoTurno,
                FechaCreacion = DateTime.Now,
                Cliente = cliente,
                Servicio = servicio,
                Estado = EstadoTurno.EnEspera
            };

            return await _turnoRepo.AddAsync(nuevoTurno);
        }

        public async Task<Turno?> CambiarEstadoTurnoAsync(string codigo, EstadoTurno nuevoEstado)
        {
            var turno = await _turnoRepo.GetByCodigoAsync(codigo);
            if (turno == null)
                return null;

            turno.Estado = nuevoEstado;
            return await _turnoRepo.UpdateAsync(turno);
        }
    }
}