using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class EventoService
    {
        private readonly AngelDbContext _context;

        public EventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> GetAll()
        {
            return await _context.Eventos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Evento?> GetById(Guid id)
        {
            return await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evento> Create(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento> Update(Guid id, Evento evento)
        {
            evento.Id = id;
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<bool> Delete(Guid id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return false;

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

