using sbBancoTurnos.src.models;
using sbBancoTurnos.src.repositories;

namespace sbBancoTurnos.src.services
{
    public interface IServicioService
    {
        Task<IEnumerable<Servicio>> GetAllServiciosAsync();
        Task<Servicio> RegistrarServicioAsync(Servicio servicioDto);
        Task<Servicio?> BuscarPorCodigoAsync(string codigo);
        Task<Servicio?> ActualizarServicioAsync(string codigo, Servicio servicioDto);
    }

    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _repo;

        public ServicioService(IServicioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Servicio>> GetAllServiciosAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Servicio?> BuscarPorCodigoAsync(string codigo)
        {
            return await _repo.GetByCodigoAsync(codigo);
        }

        public async Task<Servicio> RegistrarServicioAsync(Servicio servicioDto)
        {
            // Evitar duplicados por código
            if (await _repo.ExistsByCodigoAsync(servicioDto.Codigo))
                throw new InvalidOperationException("Ya existe un servicio con este código.");

            var nuevoServicio = new Servicio
            {
                id = Guid.NewGuid().ToString(),
                Codigo = servicioDto.Codigo,
                Nombre = servicioDto.Nombre,
                Descripcion = servicioDto.Descripcion
            };

            return await _repo.AddAsync(nuevoServicio);
        }

        public async Task<Servicio?> ActualizarServicioAsync(string codigo, Servicio servicioDto)
        {
            var servicioExistente = await _repo.GetByCodigoAsync(codigo);
            if (servicioExistente == null)
                return null;

            servicioExistente.Nombre = servicioDto.Nombre;
            servicioExistente.Descripcion = servicioDto.Descripcion;

            return await _repo.UpdateAsync(servicioExistente);
        }
    }
}