using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // Asegúrate de que este namespace coincida con tu proyecto
using ProyecServicio_comunitario.Services;
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetUsers()
        {
            var users = await _userService.GetAll();
            return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(users));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<User>>> GetUser(Guid id)
        {
            var user = await _userService.GetById(id);
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
            try
            {
                var created = await _userService.Create(user);
                return CreatedAtAction(nameof(GetUser), new { id = created.Id },
                    ApiResponse<User>.SuccessResponse(created, "Usuario creado exitosamente", 201));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<User>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<User>.ErrorResponse(
                    "Error al guardar en la base de datos",
                    ex.InnerException?.Message,
                    500));
            }
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

            try
            {
                var updated = await _userService.Update(id, user);
                return Ok(ApiResponse<User>.SuccessResponse(updated, "Usuario actualizado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<User>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(ApiResponse<User>.ErrorResponse("Usuario no encontrado", null, 404));
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
        {
            try
            {
                var deleted = await _userService.Delete(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse(
                        "No se pudo eliminar",
                        $"No existe un usuario con el ID: {id}",
                        404));
                }

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
    }
}