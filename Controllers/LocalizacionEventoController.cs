using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalizacionEventoController : Controller
    {
        public readonly LocalizacionEventoService _localizacionEventoService;

        public LocalizacionEventoController(LocalizacionEventoService localizacionEventoService)
        {
            _localizacionEventoService = localizacionEventoService;
        }

        /// <summary>
        /// Obtiene todas las localizaciones de eventos registradas en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<LocalizacionEvento>>>> GetAll()
        {
            try
            {
                var result = await _localizacionEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<LocalizacionEvento>>.SuccessResponse(result, "Localizaciones de eventos obtenidas exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener localizaciones de eventos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene la información de una localización de evento por su ID de caso.
        /// </summary>
        /// <param name="casoId">Identificador único del caso. Ej: f778e00e-a225-4870-83ae-18ac050ec1c5</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LocalizacionEvento>>> GetById(Guid id)
        {
            try
            {
                var result = await _localizacionEventoService.GetById(id);
                if (result == null)
                {
                    return NotFound(ApiResponse<LocalizacionEvento>.ErrorResponse("Localización de evento no encontrada", "El ID de caso proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<LocalizacionEvento>.SuccessResponse(result, "Localización de evento obtenida exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LocalizacionEvento>.ErrorResponse("Error al obtener la localización del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea una nueva localización de evento en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "AutopistaId": 1,
        ///     "DireccionExacta": "Av. Principal, Cerca del puente",
        ///     "PuntoReferencia": "Torre ABC",
        ///     "Latitud": 10.4682,
        ///     "Longitud": -66.8647
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<LocalizacionEvento>>> Create([FromBody] System.Text.Json.JsonElement localizacionEventoJson)
        {
            try
            {
                LocalizacionEvento localizacionEvento = new LocalizacionEvento()
                {
                    CasoId = Guid.Parse(localizacionEventoJson.GetProperty("CasoId").GetString()!),
                    AutopistaId = localizacionEventoJson.GetProperty("AutopistaId").GetInt32(),
                    DireccionExacta = localizacionEventoJson.GetProperty("DireccionExacta").GetString(),
                    PuntoReferencia = localizacionEventoJson.GetProperty("PuntoReferencia").GetString(),
                    Latitud = localizacionEventoJson.GetProperty("Latitud").GetDecimal(),
                    Longitud = localizacionEventoJson.GetProperty("Longitud").GetDecimal()
                };

                var result = await _localizacionEventoService.Create(localizacionEvento);
                return Ok(ApiResponse<LocalizacionEvento>.SuccessResponse(result, "Localización de evento creada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LocalizacionEvento>.ErrorResponse("Error al crear la localización del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza todos los datos de una localización de evento.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "AutopistaId": 2,
        ///     "DireccionExacta": "Calle Nueva, Frente al parque",
        ///     "PuntoReferencia": "Farmacia Central",
        ///     "Latitud": 10.4700,
        ///     "Longitud": -66.8700
        /// }
        /// </remarks>
        /// <param name="id">Identificador único del caso.  Ej: f778e00e-a225-4870-83ae-18ac050ec1c5</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<LocalizacionEvento>>> Update(Guid id, [FromBody] System.Text.Json.JsonElement localizacionEventoJson)
        {
            try
            {
                //mapeamos el json a la entidad LocalizacionEvento
                LocalizacionEvento localizacionEvento = new LocalizacionEvento()
                {
                    // CasoId no se actualiza a través del body, se usa el de la ruta
                    AutopistaId = localizacionEventoJson.GetProperty("AutopistaId").GetInt32(),
                    DireccionExacta = localizacionEventoJson.GetProperty("DireccionExacta").GetString(),
                    PuntoReferencia = localizacionEventoJson.GetProperty("PuntoReferencia").GetString(),
                    Latitud = localizacionEventoJson.GetProperty("Latitud").GetDecimal(),
                    Longitud = localizacionEventoJson.GetProperty("Longitud").GetDecimal()
                };
                //Guardamos en la base de datos
                var result = await _localizacionEventoService.Update(id, localizacionEvento);
                return Ok(ApiResponse<LocalizacionEvento>.SuccessResponse(result, "Localización de evento actualizada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LocalizacionEvento>.ErrorResponse("Error al actualizar la localización del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los datos enviados de una localización de evento.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "PuntoReferencia": "Nueva Referencia",
        ///     "Latitud": 10.5000
        /// }
        /// </remarks>
        /// <param name="id">Identificador único del caso.   Ej: f778e00e-a225-4870-83ae-18ac050ec1c5</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<LocalizacionEvento>>> PartialUpdate(Guid id, [FromBody] System.Text.Json.JsonElement localizacionEventoJson)
        {
            try
            {
                LocalizacionEvento localizacionEvento = new LocalizacionEvento()
                {
                    AutopistaId = localizacionEventoJson.TryGetProperty("AutopistaId", out var autopistaIdProp)? autopistaIdProp.GetInt32() : (int?)null,
                    DireccionExacta = localizacionEventoJson.TryGetProperty("DireccionExacta", out var direccionExactaProp) ? direccionExactaProp.GetString() : null,
                    PuntoReferencia = localizacionEventoJson.TryGetProperty("PuntoReferencia", out var puntoReferenciaProp) ? puntoReferenciaProp.GetString() : null,
                    Latitud = localizacionEventoJson.TryGetProperty("Latitud", out var latitudProp) ? latitudProp.GetDecimal() : (decimal?)null,
                    Longitud = localizacionEventoJson.TryGetProperty("Longitud", out var longitudProp) ? longitudProp.GetDecimal() : (decimal?)null
                };

                var result = await _localizacionEventoService.PartialUpdate(id, localizacionEvento);
                if (result == null)
                {
                    return NotFound(ApiResponse<LocalizacionEvento>.ErrorResponse("Localización de evento no encontrada", "El ID de caso proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<LocalizacionEvento>.SuccessResponse(result, "Localización de evento actualizada parcialmente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LocalizacionEvento>.ErrorResponse("Error al actualizar parcialmente la localización del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina una localización de evento por su ID de caso.
        /// </summary>
        /// <param name="id">Identificador único del caso.   Ej: f778e00e-a225-4870-83ae-18ac050ec1c5</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await _localizacionEventoService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Localización de evento no encontrada", "El ID de caso proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Localización de evento eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar la localización del evento", ex.Message, 400));
            }
        }
    }
}
