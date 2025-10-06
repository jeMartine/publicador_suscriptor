using BancoTurnosApp.src.Data;
using BancoTurnosApp.src.models;
using BancoTurnosApp.src.repositories;
using Microsoft.EntityFrameworkCore;

namespace BancoTurnosApp.src.Repositories
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly AppDbContext _context;

        public ServicioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAllAsync()
        {
            return await _context.Servicios.ToListAsync();
        }

        public async Task<Servicio?> GetByCodigoAsync(string codigo)
        {
            return await _context.Servicios.FirstOrDefaultAsync(s => s.Codigo == codigo);
        }

        public async Task<Servicio?> GetByIdAsync(string id)
        {
            return await _context.Servicios.FindAsync(id);
        }

        public async Task<Servicio> AddAsync(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<Servicio> UpdateAsync(Servicio servicio)
        {
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _context.Servicios.AnyAsync(s => s.Codigo == codigo);
        }
    }
}