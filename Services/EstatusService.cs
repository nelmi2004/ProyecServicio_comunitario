using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class EstatusService
    {
        public readonly AngelDbContext _context;

        public EstatusService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estatus>> GetAll() {
            return await _context.Estatuses.ToListAsync();
        }

        public async Task<Estatus> GetById(int id) {
            Estatus? estatus = await _context.Estatuses.FirstOrDefaultAsync(e => e.Id == id);
            return estatus;
            
        }
        public async Task<Estatus> Create (Estatus estatus)
        {
            _context.Estatuses.Add(estatus);
            await _context.SaveChangesAsync();
            return estatus;
        }
        public async Task<Estatus> Update(int id, Estatus estatus) {
            estatus.Id = id;
            _context.Estatuses.Update(estatus);
            await _context.SaveChangesAsync();
            return estatus;
        }

        public async Task<bool> Delete(int id) {
            var estatus = await _context.Estatuses.FirstOrDefaultAsync(e => e.Id == id);
            if (estatus == null) return false;
            _context.Estatuses.Remove(estatus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
