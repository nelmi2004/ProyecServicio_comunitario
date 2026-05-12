using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganismosController : Controller
    {
        public readonly OrganismoService _organismoService;

        public OrganismosController(OrganismoService organismoService)
        {
            _organismoService = organismoService;
        }

        /// <summary>
        /// Devuelve los organismos disponibles en la base de datos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Organismo>>>> GetAll()
        {
            try
            {
                var result = await _organismoService.GetAll();
                return Ok(ApiResponse<IEnumerable<Organismo>>.SuccessResponse(result, "Organismos obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener organismos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Devuelve la informacion de un organismo por su id
        /// </summary>
        /// <param name="id">Identificador unico del organismo</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Organismo>>> GetById(int id)
        {
            try
            {
                Organismo organismo = await _organismoService.GetById(id);
                if (organismo == null)
                {
                    return NotFound(ApiResponse<Organismo>.ErrorResponse("Organismo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<Organismo>.SuccessResponse(organismo, "Organismo encontrado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Organismo>.ErrorResponse("Error al obtener organismo", ex.Message, 400));
            }
        }

        /// <summary>
        /// Crea un nuevo organismo en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Organismo 1"
        /// }
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Organismo>>> Create(System.Text.Json.JsonElement organismoJson)
        {
            try
            {
                Organismo newOrganismo = new Organismo()
                {
                    Nombre = organismoJson.GetProperty("Nombre").GetString()
                };

                var result = await _organismoService.Create(newOrganismo);
                return Ok(ApiResponse<Organismo>.SuccessResponse(result, "Organismo creado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Organismo>.ErrorResponse("Error al crear organismo", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza la informacion de un organismo
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Organismo1"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del organismo</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Organismo>>> Update(int id, System.Text.Json.JsonElement organismoJson)
        {
            try
            {
                Organismo updatedOrganismo = new Organismo()
                {
                    Nombre = organismoJson.GetProperty("Nombre").GetString()
                };

                var result = await _organismoService.Update(id, updatedOrganismo);

                return Ok(ApiResponse<Organismo>.SuccessResponse(result, "Organismo actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Organismo>.ErrorResponse("Error al actualizar organismo", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un organismo de la base de datos.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _organismoService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Organismo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Organismo eliminado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar organismo", ex.Message, 400));
            }
        }
    }
}
