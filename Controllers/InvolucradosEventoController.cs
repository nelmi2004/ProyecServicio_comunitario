using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvolucradosEventoController : Controller
    {
        public readonly InvolucradosEventoService _involucradosEventoService;

        public InvolucradosEventoController(InvolucradosEventoService involucradosEventoService)
        {
            _involucradosEventoService = involucradosEventoService;
        }

        /// <summary>
        /// Obtiene todos los involucrados registrados en la base de datos.
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<InvolucradosEvento>>>> GetAll() {
            try
            {
                var result = await  _involucradosEventoService.GetAll();
                return Ok(ApiResponse<IEnumerable<InvolucradosEvento>>.SuccessResponse(result, "Involucrados obtenidos exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<InvolucradosEvento>.ErrorResponse("Error al obtener involucrados", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene un involucrado por su id
        /// </summary>
        /// <param name="id">Identificador del involucrado</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<InvolucradosEvento>>> GetById(int id) {
            try
            { 
                var result = await _involucradosEventoService.GetById(id);
                if (result == null) {
                    return NotFound(ApiResponse<InvolucradosEvento>.ErrorResponse("Involucrado no encontrado", "El id proporcionado no se consigue en la base de datos", 404));
                }
                return Ok(ApiResponse<InvolucradosEvento>.SuccessResponse(result, "Involucrado obtenido exitosamente", 200));
            }catch (Exception ex) {
                return BadRequest(ApiResponse<InvolucradosEvento>.ErrorResponse("Error al obtener el involucrado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea un nuevo involucrado para un caso en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "Cedula": "123456789",
        ///     "NombreCompleto": "Juan Perez",
        ///     "Sexo": "M",
        ///     "Edad": 25,
        ///     "EstadoPaciente": "Recuperado"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<InvolucradosEvento>>> Create(System.Text.Json.JsonElement involucradosEvento)
        {
            try 
            {
                //mapeo del json a la entidad involucrado
                InvolucradosEvento involucradoEvento = new InvolucradosEvento()
                {
                    EventoId = Guid.Parse(involucradosEvento.GetProperty("CasoId").GetString()),
                    Cedula = involucradosEvento.GetProperty("Cedula").GetString(),
                    NombreCompleto = involucradosEvento.GetProperty("NombreCompleto").GetString(),
                    Sexo = char.Parse(involucradosEvento.GetProperty("Sexo").GetString()),
                    Edad = involucradosEvento.GetProperty("Edad").GetInt32(),
                    EstadoPaciente = involucradosEvento.GetProperty("EstadoPaciente").GetString(),
                };
                //guardamos el involucrado en la base de datos.
                var result = await _involucradosEventoService.Create(involucradoEvento);
                return Ok(ApiResponse<InvolucradosEvento>.SuccessResponse(result, "Involucrado creado exitosamente", 200));
            }catch (Exception ex) {
                return BadRequest(ApiResponse<InvolucradosEvento>.ErrorResponse("Error al crear el involucrado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza todos los datos del involucrado
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CasoId": "f778e00e-a225-4870-83ae-18ac050ec1c5",
        ///     "Cedula": "123456789",
        ///     "NombreCompleto": "Juan  Gonzales",
        ///     "Sexo": "M",
        ///     "Edad": 35,
        ///     "EstadoPaciente": "Moderado"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del involucrado</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<InvolucradosEvento>>> Update(int id, System.Text.Json.JsonElement involucradosEvento) 
        {
            try
            {
                //mapeo del json a la entidad involucrado
                InvolucradosEvento involucradoEvento = new InvolucradosEvento()
                {
                    EventoId = Guid.Parse(involucradosEvento.GetProperty("CasoId").GetString()),
                    Cedula = involucradosEvento.GetProperty("Cedula").GetString(),
                    NombreCompleto = involucradosEvento.GetProperty("NombreCompleto").GetString(),
                    Sexo = char.Parse(involucradosEvento.GetProperty("Sexo").GetString()),
                    Edad = involucradosEvento.GetProperty("Edad").GetInt32(),
                    EstadoPaciente = involucradosEvento.GetProperty("EstadoPaciente").GetString(),
                };
                //Actualizamos el involucrado en la base de datos.
                var result = await _involucradosEventoService.Update(id, involucradoEvento);
                return Ok(ApiResponse<InvolucradosEvento>.SuccessResponse(result, "Involucrado actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<InvolucradosEvento>.ErrorResponse("Error al actualizar el involucrado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los datos enviados del involucrado
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Cedula": "12671931",
        ///     "Edad": 45
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del involucrado</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<InvolucradosEvento>>> PartialUpdate(int id, System.Text.Json.JsonElement involucradosEvento) 
        {
            try
            {
                //mapeo del json a la entidad involucrado
                InvolucradosEvento involucradoEvento = new InvolucradosEvento()
                {
                    EventoId = involucradosEvento.TryGetProperty("CasoId", out var CasoID)? Guid.Parse(CasoID.GetString()): null,
                    Cedula = involucradosEvento.TryGetProperty("Cedula", out var Cedula)? Cedula.GetString(): null,
                    NombreCompleto = involucradosEvento.TryGetProperty("NombreCompleto", out var NombreCompleto)? NombreCompleto.GetString(): null,
                    Sexo = involucradosEvento.TryGetProperty("Sexo", out var Sexo)? char.Parse(Sexo.GetString()): null,
                    Edad = involucradosEvento.TryGetProperty("Edad", out var Edad)? Edad.GetInt32(): null,
                    EstadoPaciente = involucradosEvento.TryGetProperty("EstadoPaciente", out var EstadoPaciente)? EstadoPaciente.GetString(): null
                };
                //Actualizamos el involucrado en la base de datos.
                var result = await _involucradosEventoService.PartialUpdate(id, involucradoEvento);
                if (result == null)
                {
                    return NotFound(ApiResponse<InvolucradosEvento>.ErrorResponse("Involucrado no encontrado", "El id proporcionado no se consigue en la base de datos", 404));
                }
                return Ok(ApiResponse<InvolucradosEvento>.SuccessResponse(result, "Involucrado actualizado exitosamente", 200));
            }
            catch (Exception ex) 
            {
                return BadRequest(ApiResponse<InvolucradosEvento>.ErrorResponse("Error al actualizar el involucrado", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un involucrado por su id
        /// </summary>
        /// <param name="id">Identificador unico del involucrado</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) {
            try
            {
                var result = await _involucradosEventoService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Involucrado no encontrado", "El id proporcionado no se consigue en la base de datos", 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Involucrado eliminado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el involucrado", ex.Message, 400));
            }
        }



    }
}
