using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class HerramientasEquipoService
    {
        public readonly AngelDbContext _context;
        public HerramientasEquipoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HerramientasEquipo>> GetAll() {
            return await _context.HerramientasEquipos.ToListAsync();
        }

        public async Task<HerramientasEquipo> GetById(int id) {
            HerramientasEquipo? herramientasEquipo = await _context.HerramientasEquipos.FirstOrDefaultAsync(p=> p.Id == id);
            return herramientasEquipo;

        }
        public async Task<HerramientasEquipo> Create(HerramientasEquipo herramientasEquipo) {
            _context.HerramientasEquipos.Add(herramientasEquipo);
            await _context.SaveChangesAsync();
            return herramientasEquipo;
        }

        public async Task<HerramientasEquipo> Update(int id, HerramientasEquipo herramientasEquipos) {
            herramientasEquipos.Id = id;
            _context.HerramientasEquipos.Update(herramientasEquipos);
            await _context.SaveChangesAsync();
            return herramientasEquipos;
        }

        public async Task<HerramientasEquipo> PartialUpdate(int id, HerramientasEquipo herramientas) {
            // buscamos el elemento en la base de datos
            HerramientasEquipo herramientasEquipo = await _context.HerramientasEquipos.FindAsync(id);
            // actualizamos los campos que sean diferentes de null
            if (herramientasEquipo == null) {
                return null;
            }
                if (herramientas.Nombre != null) herramientasEquipo.Nombre = herramientas.Nombre;
                if (herramientas.Descripcion != null) herramientasEquipo.Descripcion = herramientas.Descripcion;
                if (herramientas.CantidadTotal != -1) herramientasEquipo.CantidadTotal = herramientas.CantidadTotal;
                _context.HerramientasEquipos.Update(herramientasEquipo);
                await _context.SaveChangesAsync();
                return herramientasEquipo;
        }

        public async Task<bool> Delete(int id) {
            //Buscamos el elemento
            HerramientasEquipo? herramientasEquipo = await _context.HerramientasEquipos.FirstOrDefaultAsync(p => p.Id == id);
            if (herramientasEquipo == null) return false;
            //eliminamos si lo encontramos
            _context.HerramientasEquipos.Remove(herramientasEquipo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
