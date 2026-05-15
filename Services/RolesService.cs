using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class RolesService
    {
        private readonly AngelDbContext _context;

        public RolesService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role?> GetById(int id)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> Create(Role role)
        {
            bool exists = await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == role.Nombre.ToLower());

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Ya existe un rol con el nombre '{role.Nombre}'.");
            }

            if (role.Id != 0 && await _context.Roles.AnyAsync(r => r.Id == role.Id))
            {
                throw new InvalidOperationException(
                    $"Ya existe un rol con el ID '{role.Id}'.");
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> Update(int id, Role role)
        {
            bool nameConflict = await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == role.Nombre.ToLower() && r.Id != id);

            if (nameConflict)
            {
                throw new InvalidOperationException("Otro rol ya tiene ese nombre.");
            }

            role.Id = id;
            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Roles.AnyAsync(r => r.Id == id))
                    throw new KeyNotFoundException("Rol no encontrado");

                throw;
            }

            return role;
        }

        public async Task<bool> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            bool hasRoles = await _context.Users.AnyAsync(u => u.RoleId == id);
            if (hasRoles)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar el perfil porque tiene usuarios asociados.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

