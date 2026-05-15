using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Services;
using ProyecServicio_comunitario.Models; // Asegúrate de usar tu namespace real
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
public class MenusController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenusController(MenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menus
        [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Menu>>>> GetMenus()
        {
            var menus = await _menuService.GetAll();
            return Ok(ApiResponse<IEnumerable<Menu>>.SuccessResponse(menus, "Menús obtenidos correctamente"));
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Menu>>> GetMenu(int id)
        {
            var menu = await _menuService.GetById(id);
            if (menu == null)
            {
                return NotFound(ApiResponse<Menu>.ErrorResponse(
                    "Menú no encontrado",
                    $"No existe un registro con el ID: {id}",
                    404));
            }
        
            return Ok(ApiResponse<Menu>.SuccessResponse(menu, "Menú obtenido correctamente"));
        }

        // POST: api/Menus
        [HttpPost]
    public async Task<ActionResult<ApiResponse<Menu>>> PostMenu(Menu menu)
        {
            
            var created = await _menuService.Create(menu);
            return CreatedAtAction(nameof(GetMenu), new { id = created.Id }, 
                ApiResponse<Menu>.SuccessResponse(created, "Menú creado correctamente", 201));
        }

        // PUT: api/Menus/5
        [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Menu>>> PutMenu(int id, Menu menu)
        {
            try
            {
                var updated = await _menuService.Update(id, menu);
                return Ok(ApiResponse<Menu>.SuccessResponse(updated, "Menú actualizado correctamente"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<Menu>.ErrorResponse(
                    "Error de validación",
                    ex.Message,
                    400));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Menu>.ErrorResponse(
                    "Error de coincidencia",
                    ex.Message,
                    400));
            }

            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Menu>.ErrorResponse("Conflicto", ex.Message, 409));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _menuService.GetById(id) == null)
                {
                    return NotFound(ApiResponse<Menu>.ErrorResponse("Menú no encontrado", null, 404));
                }
                else
                {
                    throw;
                }
            }
        }


        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteMenu(int id)
        {
            try
            {
                var deleted = await _menuService.Delete(id);
                if (!deleted) return NotFound(ApiResponse<Menu>.ErrorResponse("Menú no encontrado", null, 404));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Menú eliminado correctamente"));
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