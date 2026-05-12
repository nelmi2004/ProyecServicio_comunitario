using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class OrganismoService
    {
        public readonly AngelDbContext _context;

        public OrganismoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organismo>> GetAll()
        {
            return await _context.Organismos.ToListAsync();
        }

        public async Task<Organismo?> GetById(int id)
        {
            return await _context.Organismos.FindAsync(id);
        }

        public async Task<Organismo> Create(Organismo organismo)
        {
            _context.Organismos.Add(organismo);
            await _context.SaveChangesAsync();
            return organismo;
        }

        public async Task<Organismo> Update(int id, Organismo organismo)
        {
            organismo.Id = id;
            _context.Organismos.Update(organismo);
            await _context.SaveChangesAsync();
            return organismo;
        }

        public async Task<bool> Delete(int id)
        {
            var organismo = await _context.Organismos.FindAsync(id);

            if (organismo == null)
                return false;

            _context.Organismos.Remove(organismo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
