using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class LocalizacionEventoService
    {
        public readonly AngelDbContext _context;

        public LocalizacionEventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalizacionEvento>> GetAll()
        {
            return await _context.LocalizacionEventos.ToListAsync();
        }

        public async Task<LocalizacionEvento?> GetById(Guid eventoId)
        {
            LocalizacionEvento? localizacionEvento = await _context.LocalizacionEventos.FirstOrDefaultAsync(l => l.EventoId == eventoId);
            return localizacionEvento;
        }

        public async Task<LocalizacionEvento> Create(LocalizacionEvento localizacionEvento)
        {
            _context.LocalizacionEventos.Add(localizacionEvento);
            await _context.SaveChangesAsync();
            return localizacionEvento;
        }

        public async Task<LocalizacionEvento?> Update(Guid eventoId, LocalizacionEvento localizacionEvento)
        {
            localizacionEvento.EventoId = eventoId;
            _context.LocalizacionEventos.Update(localizacionEvento);
            await _context.SaveChangesAsync();
            return localizacionEvento;
        }

        public async Task<LocalizacionEvento?> PartialUpdate(Guid eventoId, LocalizacionEvento localizacionEvento)
        {
            var existingLocalizacionEvento = await _context.LocalizacionEventos.FirstOrDefaultAsync(l => l.EventoId == eventoId);
            if (existingLocalizacionEvento == null)
            {
                return null;
            }

            if (localizacionEvento.AutopistaId.HasValue)
            {
                existingLocalizacionEvento.AutopistaId = localizacionEvento.AutopistaId;
            }
            if (localizacionEvento.DireccionExacta != null)
            {
                existingLocalizacionEvento.DireccionExacta = localizacionEvento.DireccionExacta;
            }
            if (localizacionEvento.PuntoReferencia != null)
            {
                existingLocalizacionEvento.PuntoReferencia = localizacionEvento.PuntoReferencia;
            }
            if (localizacionEvento.Latitud.HasValue)
            {
                existingLocalizacionEvento.Latitud = localizacionEvento.Latitud;
            }
            if (localizacionEvento.Longitud.HasValue)
            {
                existingLocalizacionEvento.Longitud = localizacionEvento.Longitud;
            }

            _context.LocalizacionEventos.Update(existingLocalizacionEvento);
            await _context.SaveChangesAsync();
            return existingLocalizacionEvento;
        }

        public async Task<bool> Delete(Guid eventoId)
        {
            var localizacionEvento = await _context.LocalizacionEventos.FirstOrDefaultAsync(l => l.EventoId == eventoId);
            if (localizacionEvento == null)
            {
                return false;
            }

            _context.LocalizacionEventos.Remove(localizacionEvento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
