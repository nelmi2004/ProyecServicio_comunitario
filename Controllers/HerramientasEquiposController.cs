using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HerramientasEquiposController : Controller
    {
        public readonly HerramientasEquipoService _herramientasEquiposService;

        public HerramientasEquiposController(HerramientasEquipoService herramientasEquiposService)
        {
            _herramientasEquiposService = herramientasEquiposService;
        }
        /// <summary>
        /// Obtiene todas las herramientas y equipos registrados en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<HerramientasEquipo>>>> GetAll() {
            try
            {
                var result = await _herramientasEquiposService.GetAll();
                return Ok(ApiResponse<IEnumerable<HerramientasEquipo>>.SuccessResponse(result, "Herramientas y equipos obtenidos exitosamente", 200));
            } catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener herramientas y equipos", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene una herramienta o equipo el id especificado
        /// </summary>
        /// <param name="Herramientas"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<HerramientasEquipo>>> GetById(int id) {
            try
            {
                //buscamos y devolvemos la herramienta en caso de existir.
                var result = await _herramientasEquiposService.GetById(id);
                if (result == null) {
                    return NotFound(ApiResponse<HerramientasEquipo>.ErrorResponse("Herramienta o equipo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<HerramientasEquipo>.SuccessResponse(result, "Herramienta o equipo obtenido exitosamente", 200));
            } catch (Exception ex) {
                return BadRequest(ApiResponse<HerramientasEquipo>.ErrorResponse("Error al obtener herramienta o equipo", ex.Message, 400));
            }
        }

        ///<summary>
        ///Crea una nueva herramienta o equipo en la base de datos.
        /// </summary>
        ///<remarks>
        ///Ejemplo de uso:
        ///{
        ///    "Nombre": "Motosierra",
        ///    "Descripcion": "Equipo de uso para liberar vias obstaculizadas",
        ///    "CantidadTotal": 5
        ///}
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<HerramientasEquipo>>> Create([FromBody] System.Text.Json.JsonElement Herramientas) {
            try {
                //mapeo del json a la clase HerrramientasEquipo 
                HerramientasEquipo newHerramientasEquipo = new HerramientasEquipo() {
                    Nombre = Herramientas.GetProperty("Nombre").GetString(),
                    Descripcion = Herramientas.GetProperty("Descripcion").GetString(),
                    CantidadTotal = Herramientas.GetProperty("CantidadTotal").GetInt32()
                };
                //guardado y mapep en la base de datos.
                var result = await _herramientasEquiposService.Create(newHerramientasEquipo);
                return Ok(ApiResponse<HerramientasEquipo>.SuccessResponse(result, "Herramienta o equipo creado exitosamente", 200));

            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<HerramientasEquipo>.ErrorResponse("Error al crear herramienta o equipo", ex.Message, 400));
            }
        }

        ///<summary>
        ///Actualiza todos los campos de una herramienta 
        ///</summary>
        ///<remarks>
        ///Ejemplo de uso:
        ///{
        ///    "Nombre": "Motosierra",
        ///    "Descripcion": "Equipo para liberar obstaculos en vias, como arboles caidos etc...",
        ///    "CantidadTotal": 10
        /// }
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<HerramientasEquipo>>> Update(int id, [FromBody] System.Text.Json.JsonElement Herramientas) {
            try
            {
                //mapeo del json a la clase HerramientasEquipo
                HerramientasEquipo newHerramientasEquipo = new HerramientasEquipo() {
                    Nombre = Herramientas.GetProperty("Nombre").GetString(),
                    Descripcion = Herramientas.GetProperty("Descripcion").GetString(),
                    CantidadTotal = Herramientas.GetProperty("CantidadTotal").GetInt32()
                };
                //guardado y mapeo en la base de datos
                var result = await _herramientasEquiposService.Update(id, newHerramientasEquipo);
                return Ok(ApiResponse<HerramientasEquipo>.SuccessResponse(result, "Herramienta o equipo actualizado exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<HerramientasEquipo>.ErrorResponse("Error al actualizar herramienta o equipo", ex.Message, 400));
            }
        }

        ///<summary>
        ///Actualiza solo los campos enviados de una herramienta
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "CantidadTotal": 20
        /// }
        /// </remarks>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<HerramientasEquipo>>> PartialUpdate(int id, [FromBody] System.Text.Json.JsonElement Herramientas)
        {
            try {
                //mapeo del json a la clase de HerramientasEquipo
                HerramientasEquipo newHerramientasEquipo = new HerramientasEquipo()
                {
                    Nombre = Herramientas.TryGetProperty("Nombre", out var Nombre) ? Nombre.GetString() : null,
                    Descripcion = Herramientas.TryGetProperty("Descripcion", out var Descripcion) ? Descripcion.GetString() : null,
                    CantidadTotal = Herramientas.TryGetProperty("CantidadTotal", out var CantidadTotal) ? CantidadTotal.GetInt32() : -1
                };
                //guardado
                var result = await _herramientasEquiposService.PartialUpdate(id, newHerramientasEquipo);
                if (result == null) {
                    return NotFound(ApiResponse<HerramientasEquipo>.ErrorResponse("Herramienta o equipo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<HerramientasEquipo>.SuccessResponse(result, "Herramienta o equipo actualizado exitosamente", 200));

            }catch (Exception ex) {
                return BadRequest(ApiResponse<HerramientasEquipo>.ErrorResponse("Error al actualizar parcialmente herramienta o equipo", ex.Message, 400));
            }
        }

        ///<summary>
        ///Elimina una herramienta basada en el id
        ///
        ///</summary>
        ///<param>name="id">Identificador de la herramienta</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _herramientasEquiposService.Delete(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse("Herramienta o equipo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Herramienta o equipo eliminado exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar herramienta o equipo", ex.Message, 400));
            }
        }
    }
}
