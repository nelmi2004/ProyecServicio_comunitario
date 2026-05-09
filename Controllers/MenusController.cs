using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // Asegúrate de usar tu namespace real
using ProyecServicio_comunitario.Models.Common; // Para ApiResponse<T>

namespace ProyecServicio_comunitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly AngelDbContext _context;

        public MenusController(AngelDbContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Menu>>>> GetMenus()
        {
           // Al usar .AsNoTracking() y no incluir tablas relacionadas, 
           // reduces la probabilidad de ciclos            var menus = await _context.Menus.AsNoTracking().ToListAsync();
            var menus = await _context.Menus.AsNoTracking().ToListAsync();
            return Ok(ApiResponse<IEnumerable<Menu>>.SuccessResponse(menus, "Menús obtenidos correctamente"));
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Menu>>> GetMenu(int id)
        {
            // Usamos AsNoTracking para mejorar rendimiento y evitar que EF intente 
            // gestionar el ciclo de relaciones al serializar.
            var menu = await _context.Menus
                .AsNoTracking() 
                .FirstOrDefaultAsync(m => m.Id == id);
        
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
  
            if (MenuExists(menu.Id))
            {
                return Conflict(ApiResponse<Menu>.ErrorResponse("Conflicto de ID", $"Ya existe un menú con el ID '{menu.Id}'.", 409));
            }

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenu), new { id = menu.Id }, 
                ApiResponse<Menu>.SuccessResponse(menu, "Menú creado correctamente", 201)   );
        }

        // PUT: api/Menus/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Menu>>> PutMenu(int id, Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest(ApiResponse<Menu>.ErrorResponse(
                    "Error de coincidencia", 
                    "El ID de la URL no coincide con el ID del objeto.", 
                    400));
            }
        
            // 1. Validar si existe el menú antes de intentar actualizar (sin rastreo)
            var existeMenu = await _context.Menus.AnyAsync(m => m.Id == id);
            if (!existeMenu)
            {
                return NotFound(ApiResponse<Menu>.ErrorResponse("Menú no encontrado", null, 404));
            }
        
            // 2. Marcar la entidad como modificada
            _context.Entry(menu).State = EntityState.Modified;
        
            try
            {
                await _context.SaveChangesAsync();
                
                // 3. Opcional: Desacoplar la entidad tras guardar para asegurar que el 
                // serializador JSON no vea el rastreador de EF.
                _context.Entry(menu).State = EntityState.Detached;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ApiResponse<Menu>.ErrorResponse(
                    "Error de concurrencia", 
                    ex.Message, 
                    500));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Menu>.ErrorResponse(
                    "Error al actualizar", 
                    ex.InnerException?.Message ?? ex.Message, 
                    500));
            }
        
            return Ok(ApiResponse<Menu>.SuccessResponse(menu, "Menú actualizado correctamente"));
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return NotFound(ApiResponse<Menu>.ErrorResponse("Menú no encontrado", null, 404));

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Menú eliminado correctamente"));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.AsNoTracking().Any(e => e.Id == id);
        }
    }
}