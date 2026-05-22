using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalEventoController : Controller
    {
        public readonly PersonalEventoService _personalEventoService;

        public PersonalEventoController(PersonalEventoService personalEventoService)
        {
            _personalEventoService = personalEventoService;
        }

        /// <summary>
        /// Obtiene todo el personal asignado a eventos registrado en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PersonalEvento>>>> GetAll()
        {
            try
            {
                var result = await _personalEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<PersonalEvento>>.SuccessResponse(result, "Personal de eventos obtenido exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<PersonalEvento>>.ErrorResponse("Error al obtener el personal de eventos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene la información de un personal de evento por su ID de caso y ID de personal.
        /// </summary>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="personalId">Identificador único del personal.</param>
        /// <returns></returns>
        [HttpGet("{casoId}/{personalId}")]
        public async Task<ActionResult<ApiResponse<PersonalEvento>>> GetById(Guid casoId, int personalId)
        {
            try
            {
                var result = await _personalEventoService.GetById(casoId, personalId);
                if (result == null)
                {
                    return NotFound(ApiResponse<PersonalEvento>.ErrorResponse("Personal de evento no encontrado", "El ID de caso o ID de personal proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<PersonalEvento>.SuccessResponse(result, "Personal de evento obtenido exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PersonalEvento>.ErrorResponse("Error al obtener el personal del evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea una nueva asignación de personal a un evento en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "PersonalId": 1,
        ///     "RolEnSitio": "Paramédico"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PersonalEvento>>> Create([FromBody] System.Text.Json.JsonElement personalEventoJson)
        {
            try
            {
                PersonalEvento newPersonalEvento = new PersonalEvento()
                {
                    EventoId = Guid.Parse(personalEventoJson.GetProperty("CasoId").GetString()!),
                    PersonalId = personalEventoJson.GetProperty("PersonalId").GetInt32(),
                    RolEnSitio = personalEventoJson.GetProperty("RolEnSitio").GetString()
                };

                var result = await _personalEventoService.Create(newPersonalEvento);
                return Ok(ApiResponse<PersonalEvento>.SuccessResponse(result, "Asignación de personal a evento creada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PersonalEvento>.ErrorResponse("Error al crear la asignación de personal al evento", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza todos los datos de una asignación de personal a un evento.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "RolEnSitio": "Conductor"
        /// }
        /// </remarks>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="personalId">Identificador único del personal.</param>
        /// <returns></returns>
        [HttpPut("{casoId}/{personalId}")]
        public async Task<ActionResult<ApiResponse<PersonalEvento>>> Update(Guid casoId, int personalId, [FromBody] System.Text.Json.JsonElement personalEventoJson)
        {
            try
            {
                PersonalEvento updatedPersonalEvento = new PersonalEvento()
                {
                    // CasoId y PersonalId vienen de la ruta, no se actualizan desde el body
                    RolEnSitio = personalEventoJson.GetProperty("RolEnSitio").GetString()
                };

                var result = await _personalEventoService.Update(casoId, personalId, updatedPersonalEvento);
                if (result == null)
                {
                    return NotFound(ApiResponse<PersonalEvento>.ErrorResponse("Personal de evento no encontrado", "El ID de caso o ID de personal proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<PersonalEvento>.SuccessResponse(result, "Asignación de personal a evento actualizada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PersonalEvento>.ErrorResponse("Error al actualizar la asignación de personal al evento", ex.Message, 400));
            }
        }

       

        /// <summary>
        /// Elimina una asignación de personal a un evento por su ID de caso y ID de personal.
        /// </summary>
        /// <param name="casoId">Identificador único del caso.</param>
        /// <param name="personalId">Identificador único del personal.</param>
        /// <returns></returns>
        [HttpDelete("{casoId}/{personalId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid casoId, int personalId)
        {
            try
            {
                var result = await _personalEventoService.Delete(casoId, personalId);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Personal de evento no encontrado", "El ID de caso o ID de personal proporcionado no se encuentra en la base de datos", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Asignación de personal a evento eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar la asignación de personal al evento", ex.Message, 400));
            }
        }
    }
}
