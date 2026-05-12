using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class AutopistaService
    {
        public readonly AngelDbContext _context;

        public AutopistaService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autopista>> GetAll()
        {
            return await _context.Autopistas.ToListAsync();
        }

        public async Task<Autopista?> GetById(int id)
        {
            // Usamos FindAsync que es más eficiente para buscar por ID (llave primaria)
            return await _context.Autopistas.FindAsync(id);
        }

        public async Task<Autopista> Create(Autopista autopista)
        {
            _context.Autopistas.Add(autopista);
            await _context.SaveChangesAsync();
            return autopista;
        }

        public async Task<Autopista> Update(int id, Autopista autopista)
        {
            // Asignamos el ID de la URL al objeto para asegurar consistencia
            autopista.Id = id;

            // Marcamos la entidad como modificada
            _context.Autopistas.Update(autopista);

            await _context.SaveChangesAsync();
            return autopista;
        }

        public async Task<bool> Delete(int id)
        {
            var autopista = await _context.Autopistas.FindAsync(id);

            if (autopista == null)
                return false;

            _context.Autopistas.Remove(autopista);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
