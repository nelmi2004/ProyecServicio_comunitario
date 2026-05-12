using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
using System.Security.Cryptography.X509Certificates;

namespace ProyecServicio_comunitario.Services
{
    public class PersonalGrupoService
    {
            public readonly AngelDbContext _context;
    
            public PersonalGrupoService(AngelDbContext context)
            {
                _context = context;
            }


        public async Task<IEnumerable<object>> GetAll() {
            return await _context.Grupos.Select(g => new { grupoId = g.Id, IdPersonales = g.Personals.Select(p => p.Id).ToList() }).ToListAsync();
        }

        public async Task<object> GetById(int personalId, int grupoId) {
           var result = await _context.Grupos.Include(g => g.Personals.Where(p => p.Id == personalId)).FirstOrDefaultAsync(g => g.Id == grupoId);
            if (result == null || result.Personals.Count == 0) return null;
            return new { grupoId = result.Id, IdPersonales = result.Personals.Select(p => p.Id).ToList() };
        }

        public async Task<bool> Create(int personalId, int grupoId)
        {
            //validamos que el personal y el grupo existan
            var personal = await _context.Personals.FindAsync(personalId);
            var grupo = await _context.Grupos.FindAsync(grupoId);
            if(personal == null || grupo == null) return false;
            //Agregamos el personala al grupo.
            grupo.Personals.Add(personal);
            //Guardamos los cambios en la base de datos.
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int personalId, int grupoId)
        {
            //Validamos que la relacion exista.
            var grupo = await _context.Grupos.Include(g => g.Personals.Where(p=> p.Id == personalId)).FirstOrDefaultAsync(g => g.Id == grupoId);
            //validamos que la relacion exista
            if (grupo == null || grupo.Personals.Count == 0) return false;
            //Eliminamos el personal del grupo.
            grupo.Personals.Remove(grupo.Personals.First());
            //Guardamos los cambios en la base de datos.
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
