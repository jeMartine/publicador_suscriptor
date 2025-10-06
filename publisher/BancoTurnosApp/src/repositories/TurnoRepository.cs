using BancoTurnosApp.src.Data;
using BancoTurnosApp.src.models;
using BancoTurnosApp.src.repositories;
using Microsoft.EntityFrameworkCore;

namespace BancoTurnosApp.src.Repositories
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly AppDbContext _context;

        public TurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Turno>> GetAllAsync()
        {
            return await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .ToListAsync();
        }

        public async Task<Turno?> GetByCodigoAsync(string codigo)
        {
            return await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .FirstOrDefaultAsync(t => t.Codigo == codigo);
        }

        public async Task<Turno?> GetByIdAsync(string id)
        {
            return await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .FirstOrDefaultAsync(t => t.Id == id);

        }

        public async Task<IEnumerable<Turno>> GetByEstadoAsync(EstadoTurno estado)
        {
            return await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .Where(t => t.Estado == estado)
                .ToListAsync();
        }

        public async Task<Turno> AddAsync(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<Turno> UpdateAsync(Turno turno)
        {
            _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _context.Turnos.AnyAsync(t => t.Codigo == codigo);
        }

        public async Task<int> ContarTurnosPorServicioHoyAsync(string codigoServicio)
        {
            var hoy = DateTime.Today;
            return await _context.Turnos
                .Where(t => t.Servicio.Codigo == codigoServicio 
                         && t.FechaCreacion.Date == hoy)
                .CountAsync();
        }
    }
}