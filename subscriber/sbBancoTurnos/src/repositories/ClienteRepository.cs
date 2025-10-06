// Puedo concectar mi cliente con la base de datos

using sbBancoTurnos.src.Data;
using sbBancoTurnos.src.models;
using Microsoft.EntityFrameworkCore;

namespace sbBancoTurnos.src.repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> GetByCedulaAsync(string cedula)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> ExistsByCedulaAsync(string cedula)
        {
            return await _context.Clientes.AnyAsync(c => c.Cedula == cedula);
        }
    }
}
