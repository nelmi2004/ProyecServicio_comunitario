using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalGrupoController : Controller
    {
        public readonly PersonalGrupoService _personalGrupoService;

        public PersonalGrupoController(PersonalGrupoService personalGrupoService)
        {
            _personalGrupoService = personalGrupoService;
        }
        /// <summary>
        /// Obtiene todos los personal de grupos
        /// </summary>
        /// <remarks>
        /// Devuelve todos los grupos en un objeto y dentro de cada grupo un array con el id de cada personal que forma parte de ese grupo
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetAll() {
            try
            {
                var result = await _personalGrupoService.GetAll();
                return Ok(ApiResponse<object>.SuccessResponse(result, "Personal de grupos obtenido exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener personal de grupos", ex.Message, 400));
            }
        }
        /// <summary>
        /// obtiene la relacion de un personal con un grupo en caso de existir
        /// </summary>
        /// <param name="idPersonal">Id del personal</param>
        /// <param name="idGrupo">Id del grupo</param>
        /// <returns></returns>
        [HttpGet("{idPersonal}/{idGrupo}")]
        public async Task<ActionResult<ApiResponse<object>>> GetById(int idPersonal, int idGrupo) {
            try
            {
                //Obtenemos los identificadores
                
                var result = await _personalGrupoService.GetById(idPersonal, idGrupo);
                if (result == null) {
                    return NotFound(ApiResponse<object>.ErrorResponse("El personal o el grupo no fueron encontrados en la relacion", statusCode: 404));
                }
                return Ok(ApiResponse<object>.SuccessResponse(result, "Personal de grupo obtenido exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener personal de grupos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Agrega un nuevo personal al grupo.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "IdPersonal": 1,
        ///     "IdGrupo": 1
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> Create([FromBody] System.Text.Json.JsonElement relacion)
        {
            try
            {
                //Obtenemos los identificadores
                int personalId = relacion.GetProperty("IdPersonal").GetInt32();
                int grupoId = relacion.GetProperty("IdGrupo").GetInt32();
                bool result = await _personalGrupoService.Create(personalId, grupoId);
                if (!result) {
                    return NotFound(ApiResponse<bool>.ErrorResponse("El personal o el grupo no fueron encontrados", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Personal agregado al grupo correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al agregar el personal al grupo", ex.Message, 400));
            }
        }


        /// <summary>
        /// Elimina a un miembro personal del un grupo
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "IdPersonal": 1,
        ///     "IdGrupo": 1
        /// }
        /// </remarks>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromBody] System.Text.Json.JsonElement relacion )
        {
            try
            { 
                //Obtenemos los identificadores
                int personalId = relacion.GetProperty("IdPersonal").GetInt32();
                int grupoId = relacion.GetProperty("IdGrupo").GetInt32();
                bool result = await _personalGrupoService.Delete(personalId, grupoId);
                if (!result) {
                    return NotFound(ApiResponse<bool>.ErrorResponse("El personal o el grupo no fueron encontrados en la relacion", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Personal eliminado del grupo correctamente", 200));

            }catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el personal del grupo", ex.Message, 400));
            }
        }
    }
}
