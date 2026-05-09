using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // Asegúrate de que este namespace coincida con tu proyecto
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AngelDbContext _context;

        public UsersController(AngelDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetUsers()
        {
            // Muestra todo la inofrmacion de los usuarios, incluyendo su rol
            var users = await _context.Users.ToListAsync();
            return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(users));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
        
            if (user == null)
            {
                return NotFound(ApiResponse<User>.ErrorResponse(
                    "Usuario no encontrado", 
                    $"No existe un registro con el ID: {id}", 
                    404));
            }
        
            return Ok(ApiResponse<User>.SuccessResponse(user));
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> PostUser(User user)
        {
            // 1. Validar si el nombre de usuario ya existe
            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == user.Username.ToLower());
            
            if (usernameExists)
                {
                    return Conflict(ApiResponse<User>.ErrorResponse(
                        "Nombre de usuario en uso",
                        $"El nombre de usuario '{user.Username}' ya está en uso.",
                        409));
            }
        
            // 2. Validar si el correo electrónico ya existe
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
        
            if (emailExists)
            {
                return Conflict(ApiResponse<User>.ErrorResponse(
                    "Correo electrónico en uso",
                    $"El correo electrónico '{user.Email}' ya está registrado con otro usuario.",
                    409));
            }
        
            // 3. Preparación de datos por defecto
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            user.FechaCreacion = DateTime.UtcNow;
            user.EstaActivo ??= true; // Si viene nulo, lo ponemos en true
        
            _context.Users.Add(user);
        
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Captura errores inesperados de la base de datos
                return StatusCode(500, ApiResponse<User>.ErrorResponse(
                    "Error al guardar en la base de datos",
                    ex.InnerException?.Message,
                    500));
            }
        
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, 
                 ApiResponse<User>.SuccessResponse(user, "Usuario creado exitosamente", 201));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest(ApiResponse<User>.ErrorResponse(
                    "Error de coincidencia", 
                    "El ID de la URL no coincide con el ID del cuerpo de la solicitud.", 
                    400));
            }
        
            // 1. Validar que el nuevo Username no lo tenga OTRO usuario
            bool usernameConflict = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == user.Username.ToLower() && u.Id != id);
            
            if (usernameConflict)
            {
                return Conflict(ApiResponse<User>.ErrorResponse(
                    "Conflicto de Username", 
                    $"El nombre de usuario '{user.Username}' ya está siendo usado por otra cuenta.", 
                    409));
            }
        
            // 2. Validar que el nuevo Email no lo tenga OTRO usuario
            bool emailConflict = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower() && u.Id != id);
        
            if (emailConflict)
            {
                return Conflict(ApiResponse<User>.ErrorResponse(
                    "Conflicto de Email", 
                    $"El correo '{user.Email}' ya está registrado en otra cuenta.", 
                    409));
            }
        
            _context.Entry(user).State = EntityState.Modified;
        
            // Evitamos que se modifique la fecha de creación original en el update
            _context.Entry(user).Property(x => x.FechaCreacion).IsModified = false;
        
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(ApiResponse<User>.ErrorResponse("Usuario no encontrado", null, 404));
                }
                else throw;
            }
        
            return Ok(ApiResponse<User>.SuccessResponse(user, "Usuario actualizado correctamente"));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(
                    "No se pudo eliminar", 
                    $"No existe un usuario con el ID: {id}", 
                    404));
            }
        
            try 
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Usuario eliminado permanentemente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(
                    "Error interno", 
                    ex.InnerException?.Message ?? ex.Message, 
                    500));
            }
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        
    }
}