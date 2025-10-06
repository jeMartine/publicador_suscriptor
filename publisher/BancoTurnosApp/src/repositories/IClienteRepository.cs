
using BancoTurnosApp.src.models;

namespace BancoTurnosApp.src.repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByCedulaAsync(string cedula);
        Task<Cliente> AddAsync(Cliente cliente);
        Task<bool> ExistsByCedulaAsync(string cedula);
    }
}
