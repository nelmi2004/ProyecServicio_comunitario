using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoriaController : Controller
    {
        public readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        /// <summary>
        /// Devuelve las categorias disponibles en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Categoria>>>> GetAll()
        {
            try
            {
                try
                {
                    var result = await _categoriaService.GetAll();
                    return Ok(ApiResponse<IEnumerable<Categoria>>.SuccessResponse(result, "Categorías obtenidas exitosamente", 200));
                }
                catch (Exception ex)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener categorías", ex.Message, 400));
                }
            }
            catch (Exception ex) { 
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener categorías", ex.Message, 400));
            }
        }

        /// <summary>
        /// Devuelve la informacion de una categoria por su id
        /// </summary>
        /// <param name="id">Identificador unico de la categoria</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Categoria>>> GetById(int id)
        {
            try
            {
                Categoria categoria = await _categoriaService.GetById(id);
                if (categoria == null)
                {
                    return NotFound(ApiResponse<Categoria>.ErrorResponse("Categoría no encontrada", statusCode: 404));
                }
                return Ok(ApiResponse<Categoria>.SuccessResponse(categoria, "Categoría encontrada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Categoria>.ErrorResponse("Error al obtener categoría", ex.Message, 400));
            }
            
        }

        /// <summary>
        /// Crea una nueva categoria en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Derrapamiento por hidrocarburos"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Categoria>>> Create(System.Text.Json.JsonElement categoria)
        {
            //mapeo del json al modelo de categoria
            try
            {
                Categoria newCategoria = new Categoria()
                {
                    Nombre = categoria.GetProperty("Nombre").GetString(),
                };
                var result = await _categoriaService.Create(newCategoria);
                return Ok(ApiResponse<Categoria>.SuccessResponse(result, "Categoría creada exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<Categoria>.ErrorResponse("Error al crear categoría", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza la informacion de una categoria
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre":  "Derrapamiento por hidrocarburos destilados"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico de la categoria</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Categoria>>> Update(int id, System.Text.Json.JsonElement categoria)
        {
            //mapeo del json al modelo de categoria
                try
                {
                    Categoria updatedCategoria = new Categoria()
                    {
                        Nombre = categoria.GetProperty("Nombre").GetString(),
                    };
                    var result = await _categoriaService.Update(id,updatedCategoria);
                    
                    return Ok(ApiResponse<Categoria>.SuccessResponse(result, "Categoría actualizada exitosamente", 200));
                }
                catch (Exception ex)
                {
                    return BadRequest(ApiResponse<Categoria>.ErrorResponse("Error al actualizar categoría", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina una categoria de la base de datos.
        /// </summary>
        /// 
        /// <param name="id">Identificador unido de la categoria</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _categoriaService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Categoría no encontrada", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Categoría eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar categoría", ex.Message, 400));
            }

        }
    }

}
