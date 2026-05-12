using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Services
{
    public class PersonalService
    {
        public readonly AngelDbContext _context;

        public PersonalService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Personal>> GetAll()
        {
            return await _context.Personals.ToListAsync();
        }

        public async Task<Personal> GetById(int id) {
            Personal? Personal = await _context.Personals.FirstOrDefaultAsync(p => p.Id == id);
            return Personal;
        }
        public async Task<Personal> Create(Personal personal)
        {
            _context.Personals.Add(personal);
            await _context.SaveChangesAsync();
            return personal;
        }

        public async Task<Personal> Update(int id,Personal personal) { 
            personal.Id = id;
            _context.Personals.Update(personal);
            await _context.SaveChangesAsync();
            return personal;
        }

        public async Task<Personal> PartialUpdate(int id, Personal personal) {
            //Buscamos el miembro
            Personal? existingPersonal = await _context.Personals.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPersonal == null)
            {
                throw new Exception("Personal not found");
            }
            //actualizamos los campos que fueron enviados
            if (personal.Cedula != null) existingPersonal.Cedula = personal.Cedula;
            if (personal.Nombres != null) existingPersonal.Nombres = personal.Nombres;
            if (personal.Apellidos != null) existingPersonal.Apellidos = personal.Apellidos;
            if (personal.Telefono != null) existingPersonal.Telefono = personal.Telefono;
            if (personal.Cargo != null) existingPersonal.Cargo = personal.Cargo;
            if (personal.Activo.HasValue) existingPersonal.Activo = personal.Activo.Value;

            //Guardamos los cambios
            _context.Personals.Update(existingPersonal);
            await _context.SaveChangesAsync();
            return existingPersonal;
        }

        public async Task<bool> Delete(int id) {
            var personal = await _context.Personals.FirstOrDefaultAsync(p => p.Id == id);
            if (personal == null) return false;
            _context.Personals.Remove(personal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
