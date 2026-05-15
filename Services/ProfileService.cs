using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class ProfileService
    {
        private readonly AngelDbContext _context;

        public ProfileService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profile>> GetAll()
        {
            return await _context.Profiles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Profile?> GetById(int id)
        {
            return await _context.Profiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profile> Create(Profile profile)
        {
            // Reglas de negocio desde el controller
            bool exists = await _context.Profiles
                .AnyAsync(p => p.Nombre.ToLower() == profile.Nombre.ToLower());

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Ya existe un perfil con el nombre '{profile.Nombre}'.");
            }

            if (profile.Id != 0 && await _context.Profiles.AnyAsync(p => p.Id == profile.Id))
            {
                throw new InvalidOperationException(
                    $"Ya existe un perfil con el ID '{profile.Id}'.");
            }

            profile.Read ??= false;
            profile.Create ??= false;
            profile.Update ??= false;
            profile.Delete ??= false;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<Profile> Update(int id, Profile profile)
        {
            // Validar nombre contra conflictos (otro perfil)
            bool nameConflict = await _context.Profiles
                .AnyAsync(p => p.Nombre.ToLower() == profile.Nombre.ToLower() && p.Id != id);

            if (nameConflict)
            {
                throw new InvalidOperationException("Otro perfil ya tiene ese nombre.");
            }

            profile.Id = id;
            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Profiles.AnyAsync(p => p.Id == id))
                    throw new KeyNotFoundException("Perfil no encontrado");

                throw;
            }

            return profile;
        }

        public async Task<bool> Delete(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return false;

            bool hasProfiles = await _context.Users.AnyAsync(u => u.ProfileId == id);
            if (hasProfiles)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar el perfil porque tiene usuarios asociados.");
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

