using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CentrosSaludController : Controller
    {
        public readonly CentrosSaludService _CentrosSaludService;

        public CentrosSaludController(CentrosSaludService CentrosSaludService)
        {
            _CentrosSaludService = CentrosSaludService;
        }

        ///<summary>
        ///Obtiene todos los centros de salud
        ///
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CentrosSalud>>>> GetAll()
        {
            try
            { 
                ///obtencion de los centros de salud y mapeo de la respuesta
               var result = await _CentrosSaludService.GetAll();
                return Ok(ApiResponse<IEnumerable<CentrosSalud>>.SuccessResponse(result, "Centros de salud obtenidos exitosamente", 200));
            }catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener centros de salud", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene un centro de salud en base a su id
        /// </summary>
        /// <param name="id">Identificador unico del centro</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CentrosSalud>>> GetById(int id)
        {
            try
            {
                var result = await _CentrosSaludService.GetById(id);
                if (result == null) {
                    return NotFound(ApiResponse<CentrosSalud>.ErrorResponse("Centro de salud no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<CentrosSalud>.SuccessResponse(result, "Centro de salud obtenido exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CentrosSalud>.ErrorResponse("Error al obtener centro de salud", ex.Message, 400));
            }
        }

        ///<summary>
        ///Crea un nuevo centro de salud en la base de datos con la informacion proporcionada
        ///</summary>
        ///<remarks>
        ///Ejemplo de uso:
        /// {
        ///     "Nombre": "Hospital perez de leon",
        ///     "Tipo": "Publico",
        ///     "Direccion": "Petare"
        /// }
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CentrosSalud>>> Create(System.Text.Json.JsonElement Centro)
        {
            //mapeo del json al modelo de centro
            try
            {
                CentrosSalud centrosSalud = new CentrosSalud
                {
                    Nombre = Centro.GetProperty("Nombre").GetString(),
                    Tipo = Centro.GetProperty("Tipo").GetString(),
                    Direccion = Centro.GetProperty("Direccion").GetString()
                };
                //guardado en la base de datos y mapeo de la respuesta
                var result = await _CentrosSaludService.Create(centrosSalud);
                return Ok(ApiResponse<CentrosSalud>.SuccessResponse(result, "Centro de salud creado exitosamente", 200));
            }        
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CentrosSalud>.ErrorResponse("Error al crear centro de salud", ex.Message, 400));
            }
        }

        ///<summary>
        ///Actualiza todos los datos de un centro de salud existente en base a su id.
        ///</summary>
        ///<remarks>
        ///Ejemplo de uso:
        ///{
        ///     "Nombre": "Clinica Perez de Leon",
        ///     "Tipo": "Publico",
        ///     "Direccion": "Redoma de petare"
        /// }    
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CentrosSalud>>> Update(int id, System.Text.Json.JsonElement Centro)
        {
            //mapeo del json al modelo de centro
            try {

                CentrosSalud newInfo = new CentrosSalud() {
                    Nombre = Centro.GetProperty("Nombre").GetString(),
                    Tipo = Centro.GetProperty("Tipo").GetString(),
                    Direccion = Centro.GetProperty("Direccion").GetString()
                };
                //actualizacion en la base de datos y mapeo de la respuesta
                var result = await _CentrosSaludService.Update(id, newInfo);
                return Ok(ApiResponse<CentrosSalud>.SuccessResponse(result, "Centro de salud actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CentrosSalud>.ErrorResponse("Error al actualizar centro de salud", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los datos enviados de un centro de salud en base a su id
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Centro medico Perez de Leon"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del centro</param>
        /// 
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<CentrosSalud>>> PartialUpdate(int id, System.Text.Json.JsonElement centro)
        { 
            //mapeo del json al modelo
            try {
                CentrosSalud newInfo = new CentrosSalud()
                {
                    Nombre = centro.TryGetProperty("Nombre", out var nombre) ? nombre.GetString() : null,
                    Tipo = centro.TryGetProperty("Tipo", out var tipo) ? tipo.GetString() : null,
                    Direccion = centro.TryGetProperty("Direccion", out var direccion) ? direccion.GetString() : null
                };
                //actualizacion en la base de datos y mapeo de la respuesta
                var result = await _CentrosSaludService.PartialUpdate(id, newInfo);
                if (result == null)
                {
                    return NotFound(ApiResponse<CentrosSalud>.ErrorResponse("Centro de salud no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<CentrosSalud>.SuccessResponse(result, "Centro de salud actualizado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CentrosSalud>.ErrorResponse("Error al actualizar parcialmente centro de salud", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un centro de salud en base a su id
        /// </summary>
        /// <param name="id">Identificador unico del centro</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) {
            try
            {
                var result = await _CentrosSaludService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Centro de salud no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Centro de salud eliminado exitosamente", 200));
            } catch (Exception ex) 
            { 
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error inesperado al eliminar el centro de salud", ex.Message, 400));
            }
        }
    }
}
