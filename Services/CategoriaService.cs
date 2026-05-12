using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class CategoriaService
    {
        public readonly AngelDbContext _context;

        public CategoriaService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAll() {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria> GetById(int id) {
            Categoria? categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            return categoria;
        }
        public async Task<Categoria> Create (Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
        public async Task<Categoria> Update(int id, Categoria categoria) {
            categoria.Id = id;
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> Delete(int id) {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null) return false;
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
