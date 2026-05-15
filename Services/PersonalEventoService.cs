using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
namespace ProyecServicio_comunitario.Services
{
    public class PersonalEventoService
    {
        public readonly AngelDbContext _context;

        public PersonalEventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonalEvento>> GetAll()
        {
            return await _context.PersonalEventos.ToListAsync();
        }

        public async Task<PersonalEvento?> GetById(Guid casoId, int personalId)
        {
            PersonalEvento? personalEvento = await _context.PersonalEventos.FirstOrDefaultAsync(pe => pe.CasoId == casoId && pe.PersonalId == personalId);
            return personalEvento;
        }

        public async Task<PersonalEvento> Create(PersonalEvento personalEvento)
        {
            _context.PersonalEventos.Add(personalEvento);
            await _context.SaveChangesAsync();
            return personalEvento;
        }

        public async Task<PersonalEvento?> Update(Guid casoId, int personalId, PersonalEvento personalEvento)
        {
            var existingPersonalEvento = await _context.PersonalEventos.FirstOrDefaultAsync(pe => pe.CasoId == casoId && pe.PersonalId == personalId);

            if (existingPersonalEvento == null)
            {
                return null;
            }

            // Actualizamos las propiedades del objeto existente
            existingPersonalEvento.RolEnSitio = personalEvento.RolEnSitio;
            // Las claves (CasoId, PersonalId) no se actualizan ya que son parte de la clave primaria

            _context.PersonalEventos.Update(existingPersonalEvento);
            await _context.SaveChangesAsync();
            return existingPersonalEvento;
        }

        

        public async Task<bool> Delete(Guid casoId, int personalId)
        {
            var personalEvento = await _context.PersonalEventos.FirstOrDefaultAsync(pe => pe.CasoId == casoId && pe.PersonalId == personalId);

            if (personalEvento == null)
            {
                return false;
            }

            _context.PersonalEventos.Remove(personalEvento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
