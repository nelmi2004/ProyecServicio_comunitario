using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class ServicioService
    {
        private readonly AngelDbContext _context;

        public ServicioService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAll()
        {
            return await _context.Servicios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Servicio?> GetById(Guid id)
        {
            return await _context.Servicios
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Servicio> Create(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<Servicio?> Update(Guid id, Servicio servicio)
        {
            var existing = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == id);
            if (existing == null) return null;

            // No actualizar claves
            existing.EventoId = servicio.EventoId;
            existing.NumeroReporte = servicio.NumeroReporte;
            existing.TextoNatural = servicio.TextoNatural;
            existing.UsuarioId = servicio.UsuarioId;
            existing.EstatusProcesamiento = servicio.EstatusProcesamiento;
            existing.FechaRegistro = servicio.FechaRegistro;

            _context.Servicios.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> Delete(Guid id)
        {
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == id);
            if (servicio == null) return false;

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

