using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class CentrosSaludService
    {
        public readonly AngelDbContext _context;

        public CentrosSaludService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CentrosSalud>> GetAll() {
            return await _context.CentrosSaluds.ToListAsync();
        }

        public async Task<CentrosSalud> GetById(int id)
        {
            CentrosSalud centro = await _context.CentrosSaluds.FindAsync(id);
            return centro;
        }

        public async Task<CentrosSalud> Create(CentrosSalud centrosSalud)
        {
            _context.CentrosSaluds.Add(centrosSalud);
            await _context.SaveChangesAsync();
            return centrosSalud;
        }
        public async Task<CentrosSalud> Update(int id,CentrosSalud centrosSalud) {
            centrosSalud.Id = id;
            _context.CentrosSaluds.Update(centrosSalud);
            await _context.SaveChangesAsync();
            return centrosSalud;
        }

        public async Task<CentrosSalud> PartialUpdate(int id, CentrosSalud centrosSalud) {
            //buscamos el centro
            CentrosSalud centro = await _context.CentrosSaluds.FindAsync(id);
            if (centro == null) return null;
            //de existir, se actualizan los datos que se hayan enviado
            if (centrosSalud.Nombre != null) centro.Nombre = centrosSalud.Nombre;
            if(centrosSalud.Tipo != null) centro.Tipo = centrosSalud.Tipo;
            if (centrosSalud.Direccion != null) centro.Direccion = centrosSalud.Direccion;
            _context.CentrosSaluds.Update(centro);
            await _context.SaveChangesAsync();
            return centro;

        }

        public async Task<bool> Delete(int id) {
            var centro = await _context.CentrosSaluds.FirstOrDefaultAsync(c => c.Id == id);
            if (centro == null) return false;
            _context.CentrosSaluds.Remove(centro);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
