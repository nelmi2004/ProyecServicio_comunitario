using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Profile>>>> GetProfiles()
        {
            var profiles = await _profileService.GetAll();
            return ApiResponse<IEnumerable<Profile>>.SuccessResponse(profiles, "Perfiles obtenidos correctamente");
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Profile>>> GetProfile(int id)
        {
            var profile = await _profileService.GetById(id);

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
            try
            {
                var created = await _profileService.Create(profile);
                return CreatedAtAction(nameof(GetProfile), new { id = created.Id },
                    ApiResponse<Profile>.SuccessResponse(created, "Perfil creado correctamente", 201));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Profile>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<Profile>.ErrorResponse("Error al crear el perfil", ex.Message, 500));
            }
        }

        // PUT: api/Profiles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Profile>>> PutProfile(int id, Profile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest(ApiResponse<Profile>.ErrorResponse("Error de coincidencia", "El ID no coincide.", 400));
            }

            try
            {
                var updated = await _profileService.Update(id, profile);
                return Ok(ApiResponse<Profile>.SuccessResponse(updated, "Perfil actualizado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Profile>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<Profile>.ErrorResponse("Perfil no encontrado", null, 404));
            }
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProfile(int id)
        {
            try
            {
                var deleted = await _profileService.Delete(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Perfil no encontrado", null, 404));
                }

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Perfil eliminado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("No se puede eliminar", ex.Message, 400));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("Error al eliminar el perfil", ex.Message, 500));
            }
        }
    }
}

