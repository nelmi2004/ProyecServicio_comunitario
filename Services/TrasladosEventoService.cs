using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    
    public class TrasladosEventoService
    {
        public readonly AngelDbContext _context;
        public TrasladosEventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrasladosEvento>> GetAll() => await _context.TrasladosEventos.ToListAsync();

        public async Task<TrasladosEvento> GetById(int id) {
            var trasladosEvento = await _context.TrasladosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (trasladosEvento == null) return null;
            return trasladosEvento;
        }

        public async Task<TrasladosEvento> Create(TrasladosEvento trasladosEvento) {
            _context.TrasladosEventos.Add(trasladosEvento);
            await _context.SaveChangesAsync();
            return trasladosEvento;
        }

        public async Task<TrasladosEvento> Update(int id, TrasladosEvento trasladosEvento) {
            trasladosEvento.Id = id;
            _context.TrasladosEventos.Update(trasladosEvento);
            await _context.SaveChangesAsync();
            return trasladosEvento;
        }

        public async Task<TrasladosEvento> PartialUpdate(int id, TrasladosEvento trasladosEvento)
        {
            var existingTrasladosEvento = await _context.TrasladosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (existingTrasladosEvento == null) return null;
            if (trasladosEvento.InvolucradoId != -1) existingTrasladosEvento.InvolucradoId = trasladosEvento.InvolucradoId;
            if (trasladosEvento.VehiculoId != -1) existingTrasladosEvento.VehiculoId = trasladosEvento.VehiculoId;
            if (trasladosEvento.CentroSaludId != -1) existingTrasladosEvento.CentroSaludId = trasladosEvento.CentroSaludId;
            if (trasladosEvento.Observaciones != null) existingTrasladosEvento.Observaciones = trasladosEvento.Observaciones;
            _context.TrasladosEventos.Update(existingTrasladosEvento);
            await _context.SaveChangesAsync();
            return existingTrasladosEvento;
        }

        public async Task<bool> Delete(int id) {
            var trasladosEvento = await _context.TrasladosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (trasladosEvento == null) return false;
            _context.TrasladosEventos.Remove(trasladosEvento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
