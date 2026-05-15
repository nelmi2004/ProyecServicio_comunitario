using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class MenuService
    {
        private readonly AngelDbContext _context;

        public MenuService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _context.Menus
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Menu?> GetById(int id)
        {
            return await _context.Menus
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Menu> Create(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> Update(int id, Menu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu));
            }

            // Validación movida desde MenusController
            // Si el cliente envía un ID distinto al de la ruta, se considera error.
            if (menu.Id != 0 && menu.Id != id)
            {
                throw new ArgumentException("El ID de la URL no coincide con el ID del objeto.", nameof(menu));
            }

            menu.Id = id;
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }


        public async Task<bool> Delete(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return false;

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

