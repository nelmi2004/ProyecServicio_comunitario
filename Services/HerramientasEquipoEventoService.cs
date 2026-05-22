using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
namespace ProyecServicio_comunitario.Services
{
    public class HerramientasEquipoEventoService
    {
        public readonly AngelDbContext _context;

        public HerramientasEquipoEventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HerramientasEquipoEvento>> GetAll()
        {
            return await _context.HerramientasEquipoEventos.ToListAsync();
        }

        public async Task<HerramientasEquipoEvento?> GetById(Guid EventoId, int herramientaId)
        {
            HerramientasEquipoEvento? herramientasEquipoEvento = await _context.HerramientasEquipoEventos
                .FirstOrDefaultAsync(he => he.EventoId == EventoId && he.HerramientaId == herramientaId);
            return herramientasEquipoEvento;
        }

        public async Task<HerramientasEquipoEvento> Create(HerramientasEquipoEvento herramientasEquipoEvento)
        {
            _context.HerramientasEquipoEventos.Add(herramientasEquipoEvento);
            await _context.SaveChangesAsync();
            return herramientasEquipoEvento;
        }

        public async Task<HerramientasEquipoEvento?> Update(Guid EventoId, int herramientaId, HerramientasEquipoEvento herramientasEquipoEvento)
        {
            var existingHerramientasEquipoEvento = await _context.HerramientasEquipoEventos
                .FirstOrDefaultAsync(he => he.EventoId == EventoId && he.HerramientaId == herramientaId);

            if (existingHerramientasEquipoEvento == null)
            {
                return null;
            }

            // Actualizamos las propiedades del objeto existente
            existingHerramientasEquipoEvento.CantidadUsada = herramientasEquipoEvento.CantidadUsada;
            // Las claves (EventoId, HerramientaId) no se actualizan ya que son parte de la clave primaria

            _context.HerramientasEquipoEventos.Update(existingHerramientasEquipoEvento);
            await _context.SaveChangesAsync();
            return existingHerramientasEquipoEvento;
        }

        

        public async Task<bool> Delete(Guid EventoId, int herramientaId)
        {
            var herramientasEquipoEvento = await _context.HerramientasEquipoEventos
                .FirstOrDefaultAsync(he => he.EventoId == EventoId && he.HerramientaId == herramientaId);

            if (herramientasEquipoEvento == null)
            {
                return false;
            }

            _context.HerramientasEquipoEventos.Remove(herramientasEquipoEvento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
