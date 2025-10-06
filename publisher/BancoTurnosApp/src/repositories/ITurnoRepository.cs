using BancoTurnosApp.src.models;

namespace BancoTurnosApp.src.repositories
{
    public interface ITurnoRepository
    {
        Task<IEnumerable<Turno>> GetAllAsync();
        Task<Turno?> GetByCodigoAsync(string codigo);
        Task<Turno?> GetByIdAsync(string id);
        Task<IEnumerable<Turno>> GetByEstadoAsync(EstadoTurno estado);
        Task<Turno> AddAsync(Turno turno);
        Task<Turno> UpdateAsync(Turno turno);
        Task<bool> ExistsByCodigoAsync(string codigo);
        Task<int> ContarTurnosPorServicioHoyAsync(string codigoServicio);
    }
}