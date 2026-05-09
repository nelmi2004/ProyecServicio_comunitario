using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // Asegúrate de usar tu namespace real
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly AngelDbContext _context;

        public ProfilesController(AngelDbContext context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Profile>>>> GetProfiles()
        {
            // Retorna la lista de perfiles sin incluir la lista de usuarios para evitar ciclos
            var profiles = await _context.Profiles.ToListAsync();
            return ApiResponse<IEnumerable<Profile>>.SuccessResponse(profiles, "Perfiles obtenidos correctamente");
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Profile>>> GetProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound(ApiResponse<Profile>.ErrorResponse("Perfil no encontrado", null, 404));
            }

            return ApiResponse<Profile>.SuccessResponse(profile, "Perfil obtenido correctamente");
        }

        // POST: api/Profiles
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Profile>>> PostProfile(Profile profile)
        {
            // Validar si ya existe un perfil con el mismo nombre
            bool exists = await _context.Profiles
                .AnyAsync(p => p.Nombre.ToLower() == profile.Nombre.ToLower());

            if (exists)
            {
                return Conflict(ApiResponse<Profile>.ErrorResponse("Conflicto de nombre", $"Ya existe un perfil con el nombre '{profile.Nombre}'.", 409));
            }

            if (ProfileExists(profile.Id))
            {
                return Conflict(ApiResponse<Profile>.ErrorResponse("Conflicto de ID", $"Ya existe un perfil con el ID '{profile.Id}'.", 409));
            }

            // Valores por defecto para permisos si vienen nulos
            profile.Read ??= false;
            profile.Create ??= false;
            profile.Update ??= false;
            profile.Delete ??= false;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfile), new { id = profile.Id }, 
                ApiResponse<Profile>.SuccessResponse(profile, "Perfil creado correctamente", 201)   );
        }

        // PUT: api/Profiles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Profile>>> PutProfile(int id, Profile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest(ApiResponse<Profile>.ErrorResponse("Error de coincidencia", "El ID no coincide.", 400));
            }

            // Validar que el nombre no choque con otro perfil existente
            bool nameConflict = await _context.Profiles
                .AnyAsync(p => p.Nombre.ToLower() == profile.Nombre.ToLower() && p.Id != id);

            if (nameConflict)
            {
                return Conflict(ApiResponse<Profile>.ErrorResponse("Conflicto de nombre", "Otro perfil ya tiene ese nombre.", 409));
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id)) return NotFound(ApiResponse<Profile>.ErrorResponse("Perfil no encontrado", null, 404));
                else throw;
            }

            return Ok(ApiResponse<Profile>.SuccessResponse(profile, "Perfil actualizado correctamente"));
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return NotFound(ApiResponse<Profile>.ErrorResponse("Perfil no encontrado", null, 404));
            // Validar si hay usuarios asociados antes de borrar
            bool hasProfiles = await _context.Users.AnyAsync(u => u.ProfileId == id);
            if (hasProfiles)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("No se puede eliminar", "No se puede eliminar el perfil porque tiene usuarios asociados.", 400));
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Perfil eliminado correctamente"));
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}