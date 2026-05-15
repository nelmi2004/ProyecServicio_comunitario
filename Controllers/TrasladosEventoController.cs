using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrasladosEventoController : Controller
    {
        public readonly TrasladosEventoService _trasladosEventoService;

        public TrasladosEventoController(TrasladosEventoService trasladosEventoService)
        {
            _trasladosEventoService = trasladosEventoService;
        }


        /// <summary>
        /// Devuelve todos los registros de traslados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TrasladosEvento>>>> GetAll()
        {
            try
            {
                var result = await _trasladosEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<TrasladosEvento>>.SuccessResponse(result, "Traslados obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener traslados", ex.Message, 400));
            }
        }

        /// <summary>
        /// Devuelve un registro de traslado por su id
        /// </summary>
        /// <param name="id">Identificador unico del registro de traslado</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TrasladosEvento>>> GetById(int id) {
            try
            {
                var result = await _trasladosEventoService.GetById(id);
                if (result == null) { 
                    return NotFound(ApiResponse<TrasladosEvento>.ErrorResponse("Traslado no encontrado", "Traslado no encontrado", 404));
                }
                return Ok(ApiResponse<TrasladosEvento>.SuccessResponse(result, "Traslado obtenido exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<TrasladosEvento>.ErrorResponse("Error al obtener el registro de traslado", ex.Message, 400));
            }
        }


        /// <summary>
        /// Crea un nuevo Registro de traslado realizado para un evento en particular.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "InvolucradoId": 1,
        ///     "VehiculoId": 1,
        ///     "CentroSaludId": 1,
        ///     "Observaciones": "Paciente en estado critico, con contusion en la cabeza"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TrasladosEventoController>>> Create([FromBody] System.Text.Json.JsonElement trasladosEventoJson) {
            try {
                // mapeo del json al modelo de TrasladoEvento
                TrasladosEvento trasladosEvento = new TrasladosEvento()
                {
                    InvolucradoId = trasladosEventoJson.GetProperty("InvolucradoId").GetInt32(),
                    VehiculoId = trasladosEventoJson.GetProperty("VehiculoId").GetInt32(),
                    CentroSaludId = trasladosEventoJson.GetProperty("CentroSaludId").GetInt32(),
                    Observaciones = trasladosEventoJson.GetProperty("Observaciones").GetString(),
                };
                //guardamos en la base de datos
                var result = await _trasladosEventoService.Create(trasladosEvento);
                return Ok(ApiResponse<object>.SuccessResponse(result, "Traslado creado exitosamente", 200));
            }catch(Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al crear el registro de traslado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza un registro de traslado para un evento
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "InvolucradoId": 1,
        ///     "VehiculoId": 1,
        ///     "CentroSaludId": 1,
        ///     "Observaciones": "Paciente en estado critico, con contusion en la cabeza"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del registro de traslado</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TrasladosEvento>>> Update(int id, System.Text.Json.JsonElement trasladoEventojson) {

            try
            {
                //mapeo del json al modelo de TrasladoEvento
                TrasladosEvento trasladosEvento = new TrasladosEvento()
                {
                    InvolucradoId = trasladoEventojson.GetProperty("InvolucradoId").GetInt32(),
                    VehiculoId = trasladoEventojson.GetProperty("VehiculoId").GetInt32(),
                    CentroSaludId = trasladoEventojson.GetProperty("CentroSaludId").GetInt32(),
                    Observaciones = trasladoEventojson.GetProperty("Observaciones").GetString(),
                };
                //actualizacion en la base de datos y mapeo de la respuesta
                var result = await _trasladosEventoService.Update(id, trasladosEvento);
                return Ok(ApiResponse<object>.SuccessResponse(result, "Traslado actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al actualizar el registro de traslado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los datos enviados del registro de traslado para un evento
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///    
        ///     "VehiculoId": 1, 
        ///     "Observaciones": "Paciente en estado critico, con contusion en la cabeza"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del registro de traslado</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<TrasladosEvento>>> PartialUpdate(int id, System.Text.Json.JsonElement trasladoEventoJson)
        {
            try
            {
                //mapeo del json al modelo de TrasladoEvento
                TrasladosEvento trasladosEvento = new TrasladosEvento()
                {
                    InvolucradoId = trasladoEventoJson.TryGetProperty("InvolucradoId", out var involucradoId) ? involucradoId.GetInt32() : -1,
                    VehiculoId = trasladoEventoJson.TryGetProperty("VehiculoId", out var vehiculoId) ? vehiculoId.GetInt32() : -1,
                    CentroSaludId = trasladoEventoJson.TryGetProperty("CentroSaludId", out var centroSaludId) ? centroSaludId.GetInt32() : -1,
                    Observaciones = trasladoEventoJson.TryGetProperty("Observaciones", out var observaciones) ? observaciones.GetString() : null
                };
                //actualizacion en la base de datos y mapeo de la respuesta
                var result = await _trasladosEventoService.PartialUpdate(id, trasladosEvento);
                if (result == null) { 
                    return NotFound(ApiResponse<object>.ErrorResponse("Traslado no encontrado", "Traslado no encontrado", 404));   
                }
                return Ok(ApiResponse<object>.SuccessResponse(result, "Traslado actualizado exitosamente", 200));
            }catch(Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al actualizar el registro de traslado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un registro de traslado
        /// </summary>
        /// 
        /// <param name="id">Identificado  unico del registro de traslado.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) {
            try {
                var result = await _trasladosEventoService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Traslado no encontrado", "Traslado no encontrado", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Traslado eliminado exitosamente", 200));
            }catch(Exception ex) {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el registro de traslado", ex.Message, 400));
            }
        }
     }
}
