using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;
using System.Security.Cryptography;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApsEventoController : Controller
    {
        public readonly ApsEventoService _apsEventoService;

        public ApsEventoController(ApsEventoService apsEventoService)
        {
            _apsEventoService = apsEventoService;
        }

        /// <summary>
        /// Obtiene todos los ApsEventos de la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ApsEvento>>>> GetAll() {
            try
            {
                var result = await _apsEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<ApsEvento>>.SuccessResponse(result, "ApsEventos obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<ApsEvento>>.ErrorResponse("Error al obtener ApsEventos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene un ApsEvento de la base de datos por su id
        /// </summary>
        /// <param name="id">Identificador Unico del apsEvento</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ApsEvento>>> GetById(int id) 
        {
            try
            {
                var result = await _apsEventoService.GetById(id);
                if (result == null) {
                    return NotFound(ApiResponse<ApsEvento>.ErrorResponse("ApsEvento no encontrado", "El id proporcionado no se consigue en la base de datos", 404));
                }
                return Ok(ApiResponse<ApsEvento>.SuccessResponse(result, "ApsEvento obtenido exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al obtener ApsEvento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea un nuevo ApsEvento en la base de datos
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso: 
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "InvolucradoId": 2,
        ///     "DetalleAtencion": "Paciente en estado critico, con contusion en la cabeza",
        ///     "EstadoPacientePostAps": "Fallecido",
        ///     "FechaAtencion": "2026-05-14T00:00:00Z"
        ///     
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ApsEvento>>> Create(System.Text.Json.JsonElement ApsEventoJson)
        {
            try
            {
                //mapeo del json al modelo de ApsEvento
                ApsEvento newApsEvento = new ApsEvento()
                {
                    EventoId = Guid.Parse(ApsEventoJson.GetProperty("CasoId").GetString()),
                    InvolucradoId = ApsEventoJson.GetProperty("InvolucradoId").GetInt32(),
                    DetalleAtencion = ApsEventoJson.GetProperty("DetalleAtencion").GetString(),
                    EstadoPacientePostAps = ApsEventoJson.GetProperty("EstadoPacientePostAps").GetString(),
                    FechaAtencion = ApsEventoJson.GetProperty("FechaAtencion").GetDateTime(),
                };
                //Guardamos en la base de datos.
                var result = await _apsEventoService.Create(newApsEvento);
                if (result == null)
                {
                    return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al crear ApsEvento ", "El involucrado no forma parte de este caso", 400));
                }
                return Ok(ApiResponse<ApsEvento>.SuccessResponse(result, "ApsEvento creado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al crear ApsEvento", ex.Message, 400));
              
            }
        }

        /// <summary>
        /// Actualiza un ApsEvento en la base de datos
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "EventoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "InvolucradoId": 2,
        ///     "DetalleAtencion": "Paciente en estado con traumatismo cerebral, recibira tratamiento de urgencia",
        ///     "EstadoPacientePostAps": "Fallecido",
        ///     "FechaAtencion": "2026-05-14T00:00:00Z"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del ApsEvento</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ApsEvento>>> Update(int id, System.Text.Json.JsonElement ApsEventoJson)
        {
            try
            {
                //mapeo del json al modelo de ApsEvento
                ApsEvento newApsEvento = new ApsEvento()
                {
                    EventoId = Guid.Parse(ApsEventoJson.GetProperty("EventoId").GetString()),
                    InvolucradoId = ApsEventoJson.GetProperty("InvolucradoId").GetInt32(),
                    DetalleAtencion = ApsEventoJson.GetProperty("DetalleAtencion").GetString(),
                    EstadoPacientePostAps = ApsEventoJson.GetProperty("EstadoPacientePostAps").GetString(),
                    FechaAtencion = ApsEventoJson.GetProperty("FechaAtencion").GetDateTime(),
                };
                //Guardamos en la base de datos.
                var result = await _apsEventoService.Update(id ,newApsEvento);
                //si result es null es porque el involucrado no forma parte de este caso
                if (result == null) {
                    return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al actualizar ApsEvento", "El involucrado no forma parte de este caso", 400));
                }
                return Ok(ApiResponse<ApsEvento>.SuccessResponse(result, "ApsEvento actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al actualizar ApsEvento", ex.Message, 400));

            }
        }

        /// <summary>
        /// Actualiza parcialmente un ApsEvento en la base de datos
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso : 
        /// {
        ///     "FechaAtencion": "2026-05-12T00:00:00Z"
        /// }
        /// </remarks>
        /// <param name="id">Identificador Unico del Aps</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<ApsEvento>>> PartialUpdate(int id, System.Text.Json.JsonElement ApsEventoJson)
        {
            try
            {
                //mapeo del json al modelo de ApsEvento
                ApsEvento newApsEvento = new ApsEvento()
                {
                    EventoId = ApsEventoJson.TryGetProperty("EventoId", out var EventoID)? Guid.Parse(EventoID.GetString()): null,
                    InvolucradoId = ApsEventoJson.TryGetProperty("InvolucradoId", out var InvolucradoID)? InvolucradoID.GetInt32(): -1,
                    DetalleAtencion = ApsEventoJson.TryGetProperty("DetalleAtencion", out var DetalleAtencion)? DetalleAtencion.GetString(): null,
                    EstadoPacientePostAps = ApsEventoJson.TryGetProperty("EstadoPacientePostAps", out var EstadoPacientePostAps)? EstadoPacientePostAps.GetString(): null,
                    FechaAtencion = ApsEventoJson.TryGetProperty("FechaAtencion", out var FechaAtencion)? FechaAtencion.GetDateTime(): null
                };
                //Guardamos en la base de datos.
                var result = await _apsEventoService.PartialUpdate(id, newApsEvento);
                if (result == null) {
                    return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("ApsEvento no puede ser actualizado", "El aps no se encuentra o el involucrado no forma parte de este caso", 404));
                }
                return Ok(ApiResponse<ApsEvento>.SuccessResponse(result, "ApsEvento actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ApsEvento>.ErrorResponse("Error al actualizar ApsEvento", ex.Message, 400));

            }
        }

        /// <summary>
        /// Elimina un ApsEvento de la base de datos
        /// </summary>
        /// <param name="id">Identificador unico del aps</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) {
            try
            {
                var result = await _apsEventoService.Delete(id);
                if (!result) {
                    return NotFound(ApiResponse<bool>.ErrorResponse("ApsEvento no encontrado", "El id proporcionado no se consigue en la base de datos", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "ApsEvento eliminado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar ApsEvento", ex.Message, 400));
            }
        }

        
    }

}
