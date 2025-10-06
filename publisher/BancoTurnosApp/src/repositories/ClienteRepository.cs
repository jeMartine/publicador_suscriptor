// Puedo concectar mi cliente con la base de datos

using BancoTurnosApp.src.Data;
using BancoTurnosApp.src.models;
using BancoTurnosApp.src.repositories;
using Microsoft.EntityFrameworkCore;

namespace BancoTurnosApp.src.Repositories
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
