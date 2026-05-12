using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class GrupoService
    {
        public readonly AngelDbContext _context;

        public GrupoService(AngelDbContext context)
        {
            _context = context;
        }

        //Devuelve todos los grupos>
        public async Task<IEnumerable<Grupo>> GetAll() { return await _context.Grupos.ToListAsync(); } 
        public async Task<Grupo> Create(Grupo grupo)
        { 
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<Grupo> GetById(int id) { 
            return await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);
        }


        
        public async Task<Grupo> Update(int id, Grupo grupo)
        {
            grupo.Id = id;
            _context.Grupos.Update(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<Grupo> PartialUpdate(int id, Grupo grupo) { 
            //Obtener el grupo con el id proporcionado
            var existingGrupo = await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);
            if (existingGrupo == null) return null;
            //Actualizar los campos que fueron enviados
            if (grupo.Descripcion != null) existingGrupo.Descripcion = grupo.Descripcion;
            if (grupo.Nombre != null) existingGrupo.Nombre = grupo.Nombre; 
            //Guardar los cambios
            _context.Grupos.Update(existingGrupo);
            await _context.SaveChangesAsync();
            return existingGrupo;
        }

        public async Task<bool> Delete(int id) { 
            var grupo = await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return false;
            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
