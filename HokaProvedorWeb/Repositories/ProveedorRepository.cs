using HokaProvedorWeb.Data;
using HokaProvedorWeb.Data.Entities;
using HokaProvedorWeb.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _context;

        public ProveedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await _context.Provedores.ToListAsync();
        }
    }
}
