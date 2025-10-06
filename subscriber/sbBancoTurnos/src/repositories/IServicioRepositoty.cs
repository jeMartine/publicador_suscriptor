using sbBancoTurnos.src.models;

namespace sbBancoTurnos.src.repositories
{
    public interface IServicioRepository
    {
        Task<IEnumerable<Servicio>> GetAllAsync();
        Task<Servicio?> GetByCodigoAsync(string codigo);
        Task<Servicio?> GetByIdAsync(string id);
        Task<Servicio> AddAsync(Servicio servicio);
        Task<Servicio> UpdateAsync(Servicio servicio);
        Task<bool> ExistsByCodigoAsync(string codigo);
    }
}