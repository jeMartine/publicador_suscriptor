

using BancoTurnosApp.src.models;
using BancoTurnosApp.src.repositories;

namespace BancoTurnosApp.src.services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente> RegistrarClienteAsync(Cliente clienteDto);
        Task<Cliente?> BuscarPorCedulaAsync(string cedula);
    }

    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Cliente?> BuscarPorCedulaAsync(string cedula)
        {
            return await _repo.GetByCedulaAsync(cedula);
        }

        public async Task<Cliente> RegistrarClienteAsync(Cliente clienteDto)
        {
            // Evitar duplicados por cédula
            if (await _repo.ExistsByCedulaAsync(clienteDto.Cedula))
                throw new InvalidOperationException("Ya existe un cliente con esta cédula.");

            var nuevoCliente = new Cliente
            {
                Id = Guid.NewGuid().ToString(),
                Cedula = clienteDto.Cedula,
                Nombre = clienteDto.Nombre,
                RequiereAsistencia = clienteDto.RequiereAsistencia
            };

            return await _repo.AddAsync(nuevoCliente);
        }
    }
}
