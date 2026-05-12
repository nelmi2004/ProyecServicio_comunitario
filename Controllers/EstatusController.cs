using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;
using Microsoft.AspNetCore.Http;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EstatusController : Controller
    {
        public readonly EstatusService _estatusService;

        public EstatusController(EstatusService estatusService)
        {
            _estatusService = estatusService;
        }
        /// <summary>
        /// Devuelve los estatus disponibles en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Estatus>>>> GetAll()
        {
            try
            {
                var result = await _estatusService.GetAll();
                return Ok(ApiResponse<IEnumerable<Estatus>>.SuccessResponse(result, "Estatus obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener estatus", ex.Message, 400));
            }
        }

        /// <summary>
        /// Devuelve la informacion de un estatus por su id
        /// </summary>
        /// <param name="id">Identificador unico del estatus</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Estatus>>> GetById(int id)
        {
            try
            {
                Estatus estatus = await _estatusService.GetById(id);
                if (estatus == null)
                {
                    return NotFound(ApiResponse<Estatus>.ErrorResponse("Estatus no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<Estatus>.SuccessResponse(estatus, "Estatus encontrado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Estatus>.ErrorResponse("Error al obtener estatus", ex.Message, 400));
            }
            
        }

        /// <summary>
        /// Crea un nuevo estatus en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Activo"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Estatus>>> Create(System.Text.Json.JsonElement estatus)
        {
            try
            {
                Estatus newEstatus = new Estatus()
                {
                    Nombre = estatus.GetProperty("Nombre").GetString(),
                };
                var result = await _estatusService.Create(newEstatus);
                return Ok(ApiResponse<Estatus>.SuccessResponse(result, "Estatus creado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<Estatus>.ErrorResponse("Error al crear estatus", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza la informacion de un estatus
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre":  "Inactivo"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del estatus</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Estatus>>> Update(int id, System.Text.Json.JsonElement estatus)
        {
            try
            {
                Estatus updatedEstatus = new Estatus()
                {
                    Nombre = estatus.GetProperty("Nombre").GetString(),
                };
                var result = await _estatusService.Update(id,updatedEstatus);
                
                return Ok(ApiResponse<Estatus>.SuccessResponse(result, "Estatus actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Estatus>.ErrorResponse("Error al actualizar estatus", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un estatus de la base de datos.
        /// </summary>
        /// 
        /// <param name="id">Identificador unico del estatus</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _estatusService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Estatus no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Estatus eliminado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar estatus", ex.Message, 400));
            }
        }
    }
}
