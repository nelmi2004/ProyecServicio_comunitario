using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RolesService _rolesService;

        public RolesController(RolesService rolesService)
        {
            _rolesService = rolesService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Role>>>> GetRoles()
        {
            var roles = await _rolesService.GetAll();
            return ApiResponse<IEnumerable<Role>>.SuccessResponse(roles, "Roles obtenidos correctamente");
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Role>>> GetRole(int id)
        {
            var role = await _rolesService.GetById(id);

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
            try
            {
                var created = await _rolesService.Create(role);
                return CreatedAtAction(nameof(GetRole), new { id = created.Id },
                    ApiResponse<Role>.SuccessResponse(created, "Rol creado correctamente", 201));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Role>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<Role>.ErrorResponse("Error al crear el rol", ex.Message, 500));
            }   
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Role>>> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest(ApiResponse<Role>.ErrorResponse("Error de coincidencia", "El ID no coincide.", 400));
            }

            try
            {
                var updated = await _rolesService.Update(id, role);
                return Ok(ApiResponse<Role>.SuccessResponse(updated, "Rol actualizado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Role>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<Role>.ErrorResponse("Rol no encontrado", null, 404));
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRole(int id)
        {
            try
            {
                var deleted = await _rolesService.Delete(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Rol no encontrado", null, 404));
                }

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Rol eliminado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("No se puede eliminar", ex.Message, 400));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("Error al eliminar el rol", ex.Message, 500));
            }
        }
    }
}

