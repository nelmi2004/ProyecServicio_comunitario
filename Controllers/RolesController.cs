using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // Asegúrate de usar tu namespace real
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AngelDbContext _context;

        public RolesController(AngelDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Role>>>> GetRoles()
        {
            // Retorna la lista de roles sin incluir la lista de usuarios para evitar ciclos
            var roles = await _context.Roles.ToListAsync();
            return ApiResponse<IEnumerable<Role>>.SuccessResponse(roles, "Roles obtenidos correctamente");
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Role>>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound(ApiResponse<Role>.ErrorResponse("Rol no encontrado", null, 404));
            }

            return ApiResponse<Role>.SuccessResponse(role, "Rol obtenido correctamente");
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Role>>> PostRole(Role role)
        {
            // Validar si ya existe un rol con el mismo nombre
            bool exists = await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == role.Nombre.ToLower());

            if (exists)
            {
                return Conflict(ApiResponse<Role>.ErrorResponse("Conflicto de nombre", $"Ya existe un rol con el nombre '{role.Nombre}'.", 409));
            }

            if (RoleExists(role.Id))
            {
                return Conflict(ApiResponse<Role>.ErrorResponse("Conflicto de ID", $"Ya existe un rol con el ID '{role.Id}'.", 409));
            }

            _context.Roles.Add(role);

            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, 
                ApiResponse<Role>.SuccessResponse(role, "Rol creado correctamente", 201)   );
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Role>>> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest(ApiResponse<Role>.ErrorResponse("Error de coincidencia", "El ID no coincide.", 400));
            }

            // Validar que el nombre no choque con otro rol existente
            bool nameConflict = await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == role.Nombre.ToLower() && r.Id != id);

            if (nameConflict)
            {
                return Conflict(ApiResponse<Role>.ErrorResponse("Conflicto de nombre", "Otro rol ya tiene ese nombre.", 409));
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id)) return NotFound(ApiResponse<Role>.ErrorResponse("Rol no encontrado", null, 404));
                else throw;
            }

            return Ok(ApiResponse<Role>.SuccessResponse(role, "Rol actualizado correctamente"));
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound(ApiResponse<Role>.ErrorResponse("Rol no encontrado", null, 404));
            // Validar si hay usuarios asociados antes de borrar
            bool hasRoles = await _context.Users.AnyAsync(u => u.RoleId == id);
            if (hasRoles)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("No se puede eliminar", "No se puede eliminar el perfil porque tiene usuarios asociados.", 400));
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Rol eliminado correctamente"));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}