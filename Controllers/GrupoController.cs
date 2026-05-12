using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : Controller
    {
        public readonly GrupoService _grupoService;
        public GrupoController(GrupoService grupoService)
        {
            _grupoService = grupoService;
        }

        /// <summary>
        /// Devuelve todos los grupos registrados en la base de datos
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Grupo>>>> GetAll() {
            try
            {
                var result = await _grupoService.GetAll();
                return Ok(ApiResponse<IEnumerable<Grupo>>.SuccessResponse(result, "Grupos obtenidos exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener grupos", ex.Message, 400));
            }

        }

        /// <summary>
        /// Devuelve un grupo por su id
        /// </summary>
        /// <param name="id">Identificacor único</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Grupo>>> GetById(int id)
        {
            try
            {
                Grupo grupo = await _grupoService.GetById(id);
                if (grupo == null)
                {
                    return NotFound(ApiResponse<Grupo>.ErrorResponse("Grupo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<Grupo>.SuccessResponse(grupo, "Grupo obtenido exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<Grupo>.ErrorResponse("Error al obtener grupo", ex.Message, 400));
            }
        }
        /// <summary>
        /// Crea un nuevo grupo en la base de datos
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso: 
        /// {
        ///     "Nombre": "Grupo 1",
        ///     "Descripcion": "Primera linea"
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<Grupo>>> Create(System.Text.Json.JsonElement grupo) {
            try
            {
                Grupo newGrupo = new Grupo() {
                   Nombre = grupo.GetProperty("Nombre").GetString(),
                   Descripcion = grupo.GetProperty("Descripcion").GetString()
                };
                var result = await _grupoService.Create(newGrupo);
                return Ok(ApiResponse<Grupo>.SuccessResponse(result, "Grupo creado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<Grupo>.ErrorResponse("Error al crear grupo", ex.Message, 400));
            }
        }
        /// <summary>
        /// Actualiza la informacion de un grupo por su id
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Grupo Motorizado",
        ///     "Descripcion": "Maniobra de vehiculos motorizados especializados"
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del grupo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Grupo>>> Update(int id, System.Text.Json.JsonElement grupo) {
            try
            {
                //mapeo del json a la clase grupo
                Grupo grupoUpdated = new Grupo() {
                    Nombre = grupo.GetProperty("Nombre").GetString(),
                    Descripcion = grupo.GetProperty("Descripcion").GetString()
                };
                //guardado y mapeo de la respuesta
                var result = await _grupoService.Update(id, grupoUpdated);
                return Ok(ApiResponse<Grupo>.SuccessResponse(result, "Grupo actualizado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<Grupo>.ErrorResponse("Error al actualizar grupo", ex.Message, 400));
            }
            
        }

        /// <summary>
        /// Actualiza solo los campos enviados de un grupo a traves de su id.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Grupo Rescatistas"
        /// }
        /// </remarks>
        /// <param name="id">Identificador Unico del Grupo</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<Grupo>>> PartialUpdate(int id, System.Text.Json.JsonElement grupo){
            try
            {
                //mapeo del json a la clase grupo
                Grupo grupoUpdated = new Grupo()
                {
                    Nombre = grupo.TryGetProperty("Nombre", out var nombre) ? nombre.GetString() : null,
                    Descripcion = grupo.TryGetProperty("Descripcion", out var descripcion) ? descripcion.GetString() : null
                };
                //guardado
                var result = await _grupoService.PartialUpdate(id, grupoUpdated);
                //si no se encontro el grupo se devuelve un 404
                if (result == null)
                {
                    return NotFound(ApiResponse<Grupo>.ErrorResponse("Grupo no encontrado", statusCode: 404));
                }
                //mapeo de la respuesta en caso de exito
          
                return Ok(ApiResponse<Grupo>.SuccessResponse(result, "Grupo actualizado exitosamente", 200));

            }catch (Exception ex) {
                return BadRequest(ApiResponse<Grupo>.ErrorResponse("Error al actualizar parcialmente el grupo", ex.Message, 400));
            }
        }
        /// <summary>
        /// Elimina un grupo por su Id
        /// </summary>
        /// <param name="id">Identificador Unico</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) {
            try
            {
                var result = await _grupoService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("No se logró eliminar el grupo", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Grupo eliminado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el grupo", ex.Message, 400));
            }
        }
    }
}
