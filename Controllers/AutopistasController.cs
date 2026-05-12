using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutopistasController : Controller
    {
        public readonly AutopistaService _autopistaService;

        public AutopistasController(AutopistaService autopistaService)
        {
            _autopistaService = autopistaService;
        }

        /// <summary>
        /// Devuelve las autopistas disponibles en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Autopista>>>> GetAll()
        {
            try
            {
                var result = await _autopistaService.GetAll();
                return Ok(ApiResponse<IEnumerable<Autopista>>.SuccessResponse(result, "Autopistas obtenidas exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener autopistas", ex.Message, 400));
            }
        }

        /// <summary>
        /// Devuelve la informacion de una autopista por su id
        /// </summary>
        /// <param name="id">Identificador unico de la autopista</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Autopista>>> GetById(int id)
        {
            try
            {
                Autopista autopista = await _autopistaService.GetById(id);
                if (autopista == null)
                {
                    return NotFound(ApiResponse<Autopista>.ErrorResponse("Autopista no encontrada", statusCode: 404));
                }
                return Ok(ApiResponse<Autopista>.SuccessResponse(autopista, "Autopista encontrada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Autopista>.ErrorResponse("Error al obtener autopista", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea una nueva autopista en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Autopista Regional del Centro"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Autopista>>> Create(System.Text.Json.JsonElement autopista)
        {
            try
            {
                // Mapeo manual preventivo
                Autopista newAutopista = new Autopista()
                {
                    Nombre = autopista.GetProperty("Nombre").GetString()
                };

                var result = await _autopistaService.Create(newAutopista);
                return Ok(ApiResponse<Autopista>.SuccessResponse(result, "Autopista creada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Autopista>.ErrorResponse("Error al crear autopista", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza la informacion de una autopista
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Autopista Francisco Fajardo"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico de la autopista</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Autopista>>> Update(int id, System.Text.Json.JsonElement autopista)
        {
            try
            {
                Autopista updatedAutopista = new Autopista()
                {
                    Nombre = autopista.GetProperty("Nombre").GetString()
                };

                var result = await _autopistaService.Update(id, updatedAutopista);

                return Ok(ApiResponse<Autopista>.SuccessResponse(result, "Autopista actualizada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Autopista>.ErrorResponse("Error al actualizar autopista", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina una autopista de la base de datos.
        /// </summary>
        /// <param name="id">Identificador unico de la autopista</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _autopistaService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Autopista no encontrada", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Autopista eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar autopista", ex.Message, 400));
            }
        }
    }
}
