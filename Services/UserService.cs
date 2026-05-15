using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class UserService
    {
        private readonly AngelDbContext _context;

        public UserService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Create(User user)
        {
            // 1) Username duplicado
            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == user.Username.ToLower());

            if (usernameExists)
            {
                throw new InvalidOperationException(
                    $"El nombre de usuario '{user.Username}' ya está en uso.");
            }

            // 2) Email duplicado
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"El correo electrónico '{user.Email}' ya está registrado con otro usuario.");
            }

            // 3) Defaults
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.FechaCreacion = DateTime.UtcNow;
            user.EstaActivo ??= true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(Guid id, User user)
        {
            if (id != user.Id)
            {
                user.Id = id;
            }

            // 1) Username duplicado (otro usuario)
            bool usernameConflict = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == user.Username.ToLower() && u.Id != id);

            if (usernameConflict)
            {
                throw new InvalidOperationException(
                    $"El nombre de usuario '{user.Username}' ya está siendo usado por otra cuenta.");
            }

            // 2) Email duplicado (otro usuario)
            bool emailConflict = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower() && u.Id != id);

            if (emailConflict)
            {
                throw new InvalidOperationException(
                    $"El correo '{user.Email}' ya está registrado en otra cuenta.");
            }

            _context.Entry(user).State = EntityState.Modified;

            // Evitamos que se modifique la fecha de creación original en el update
            _context.Entry(user).Property(x => x.FechaCreacion).IsModified = false;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

