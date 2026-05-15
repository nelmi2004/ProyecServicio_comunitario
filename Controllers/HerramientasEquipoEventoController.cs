using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Services;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HerramientasEquipoEventoController : Controller
    {
        public readonly HerramientasEquipoEventoService _herramientasEquipoEventoService;

        public HerramientasEquipoEventoController(HerramientasEquipoEventoService herramientasEquipoEventoService)
        {
            _herramientasEquipoEventoService = herramientasEquipoEventoService;
        }

        /// <summary>
        /// Obtiene todas las herramientas y equipos asignados a eventos registrados en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<HerramientasEquipoEvento>>>> GetAll()
        {
            try
            {
                var result = await _herramientasEquipoEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<HerramientasEquipoEvento>>.SuccessResponse(result, "Herramientas y equipos de eventos obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<HerramientasEquipoEvento>>.ErrorResponse("Error al obtener herramientas y equipos de eventos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene la información de una herramienta/equipo de evento por su ID de caso y ID de herramienta.
        /// </summary>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="herramientaId">Identificador único de la herramienta/equipo.</param>
        /// <returns></returns>
        [HttpGet("{casoId}/{herramientaId}")]
        public async Task<ActionResult<ApiResponse<HerramientasEquipoEvento>>> GetById(Guid casoId, int herramientaId)
        {
            try
            {
                var result = await _herramientasEquipoEventoService.GetById(casoId, herramientaId);
                if (result == null)
                {
                    return NotFound(ApiResponse<HerramientasEquipoEvento>.ErrorResponse("Asignación de herramienta/equipo no encontrada", "El ID de caso o ID de herramienta proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<HerramientasEquipoEvento>.SuccessResponse(result, "Asignación de herramienta/equipo de evento obtenida exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HerramientasEquipoEvento>.ErrorResponse("Error al obtener la asignación de herramienta/equipo del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea una nueva asignación de herramienta/equipo a un evento en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "HerramientaId": 1,
        ///     "CantidadUsada": 5
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<HerramientasEquipoEvento>>> Create([FromBody] System.Text.Json.JsonElement herramientasEquipoEventoJson)
        {
            try
            {
                HerramientasEquipoEvento newHerramientasEquipoEvento = new HerramientasEquipoEvento()
                {
                    CasoId = Guid.Parse(herramientasEquipoEventoJson.GetProperty("CasoId").GetString()!),
                    HerramientaId = herramientasEquipoEventoJson.GetProperty("HerramientaId").GetInt32(),
                    CantidadUsada = herramientasEquipoEventoJson.GetProperty("CantidadUsada").GetInt32()
                };

                var result = await _herramientasEquipoEventoService.Create(newHerramientasEquipoEvento);
                return Ok(ApiResponse<HerramientasEquipoEvento>.SuccessResponse(result, "Asignación de herramienta/equipo a evento creada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HerramientasEquipoEvento>.ErrorResponse("Error al crear la asignación de herramienta/equipo al evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza todos los datos de una asignación de herramienta/equipo a un evento.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CantidadUsada": 10
        /// }
        /// </remarks>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="herramientaId">Identificador único de la herramienta/equipo.</param>
        /// <returns></returns>
        [HttpPut("{casoId}/{herramientaId}")]
        public async Task<ActionResult<ApiResponse<HerramientasEquipoEvento>>> Update(Guid casoId, int herramientaId, [FromBody] System.Text.Json.JsonElement herramientasEquipoEventoJson)
        {
            try
            {
                HerramientasEquipoEvento updatedHerramientasEquipoEvento = new HerramientasEquipoEvento()
                {
                    // CasoId y HerramientaId vienen de la ruta, no se actualizan desde el body
                    CantidadUsada = herramientasEquipoEventoJson.GetProperty("CantidadUsada").GetInt32()
                };

                var result = await _herramientasEquipoEventoService.Update(casoId, herramientaId, updatedHerramientasEquipoEvento);
                if (result == null)
                {
                    return NotFound(ApiResponse<HerramientasEquipoEvento>.ErrorResponse("Asignación de herramienta/equipo no encontrada", "El ID de caso o ID de herramienta proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<HerramientasEquipoEvento>.SuccessResponse(result, "Asignación de herramienta/equipo a evento actualizada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HerramientasEquipoEvento>.ErrorResponse("Error al actualizar la asignación de herramienta/equipo al evento", ex.Message, 400));
            }
        }

        

        /// <summary>
        /// Elimina una asignación de herramienta/equipo a un evento por su ID de caso y ID de herramienta.
        /// </summary>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="herramientaId">Identificador único de la herramienta/equipo.</param>
        /// <returns></returns>
        [HttpDelete("{casoId}/{herramientaId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid casoId, int herramientaId)
        {
            try
            {
                var result = await _herramientasEquipoEventoService.Delete(casoId, herramientaId);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Asignación de herramienta/equipo no encontrada", "El ID de caso o ID de herramienta proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Asignación de herramienta/equipo a evento eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar la asignación de herramienta/equipo al evento", ex.Message, 400));
            }
        }
    }
}
