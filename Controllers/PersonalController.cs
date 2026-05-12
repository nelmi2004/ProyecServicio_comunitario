using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;


namespace ProyecServicio_comunitario.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalController : Controller
    {
        public readonly PersonalService _personalService;

        public PersonalController(PersonalService personalService) {
            _personalService = personalService;
        }

        /// <summary>
        /// Obtiene los datos de todos los miembros del personal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Personal>>>> Get() {
            try
            {
                var result = await  _personalService.GetAll();
                return Ok(ApiResponse<IEnumerable<Personal>>.SuccessResponse(result, "Personal obtenido correctamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener el personal", ex.Message, 400));
            }
        }

        /// <summary>
        /// Obtiene los datos de un miembro del personal a traves de su id.
        /// </summary>
        /// <param name="personal">Identificador unico del personal</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Personal>>> Get(int id) {
            try
            {
                Personal? personal = await _personalService.GetById(id);
                if (personal == null)
                {
                    return NotFound(ApiResponse<Personal>.ErrorResponse("Personal no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<Personal>.SuccessResponse(personal, "Personal obtenido correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Personal>.ErrorResponse("Error al obtener el personal", ex.Message, 400));
            }
        }

        ///<summary>
        ///Crea un nuevo miembro del personal en la base de datos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso
        /// {
        ///     "Cedula": "123456789",
        ///     "Nombres": "Juan",
        ///     "Apellidos": "Perez",
        ///     "Telefono": "123456789",
        ///     "Cargo": "Administrador",
        ///     "Activo": true
        /// }
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Personal>>> Create([FromBody] System.Text.Json.JsonElement personal)
        {
            try
            {
                //mapeamos el json a la entidad
                Personal personal1 = new Personal() {
                    Cedula = personal.GetProperty("Cedula").GetString(),
                    Nombres = personal.GetProperty("Nombres").GetString(),
                    Apellidos = personal.GetProperty("Apellidos").GetString(),
                    Telefono = personal.GetProperty("Telefono").GetString(),
                    Cargo = personal.GetProperty("Cargo").GetString(),
                    Activo = personal.GetProperty("Activo").GetBoolean()
                };
                //creamos
                Personal CreatedPersonal = await _personalService.Create(personal1);
                return Ok(ApiResponse<Personal>.SuccessResponse(CreatedPersonal, "Personal creado correctamente", 200));

            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Personal>.ErrorResponse("Error al crear el personal", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza todos los datos de un miembro del personal
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso
        /// {
        ///     "Cedula": "123456789",
        ///     "Nombres": "Juan",
        ///     "Apellidos": "Perez",
        ///     "Telefono": "12367900",
        ///     "Cargo": "Paramedico",
        ///     "Activo": true
        /// }   
        /// </remarks>
        /// <param name="id">Identificador unico del personal</param>
        /// 
        /// <returns></returns>
        [HttpPut("{id}")]
        
        public async Task<ActionResult<ApiResponse<Personal>>> Update(int id,[FromBody] System.Text.Json.JsonElement personal)
        {
            try {

                //mapeamos el json a la entidad
                Personal personal1 = new Personal()
                {
                    Cedula = personal.GetProperty("Cedula").GetString(),
                    Nombres = personal.GetProperty("Nombres").GetString(),
                    Apellidos = personal.GetProperty("Apellidos").GetString(),
                    Telefono = personal.GetProperty("Telefono").GetString(),
                    Cargo = personal.GetProperty("Cargo").GetString(),
                    Activo = personal.GetProperty("Activo").GetBoolean()
                };
                //actualizamos
                var result = await _personalService.Update(id, personal1);
                return Ok(ApiResponse<Personal>.SuccessResponse(result, "Personal actualizado correctamente", 200));
            }catch (Exception ex)
            {
                return BadRequest(ApiResponse<Personal>.ErrorResponse("Error al actualizar el personal", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los campos enviados de un miembro del personal
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso
        /// {
        ///     "Activo": false
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico del personal</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<Personal>>> PartialUpdate(int id, [FromBody] System.Text.Json.JsonElement personal)
        {
            try
            {
                //mapeamos el json a la entidad
                Personal personal1 = new Personal()
                {
                    Cedula = personal.TryGetProperty("Cedula", out var cedula) ? cedula.GetString() : null,
                    Nombres = personal.TryGetProperty("Nombres", out var nombres) ? nombres.GetString() : null,
                    Apellidos = personal.TryGetProperty("Apellidos", out var apellidos) ? apellidos.GetString() : null,
                    Telefono = personal.TryGetProperty("Telefono", out var telefono) ? telefono.GetString() : null,
                    Cargo = personal.TryGetProperty("Cargo", out var cargo) ? cargo.GetString() : null,
                    Activo = personal.TryGetProperty("Activo", out var activo) ? activo.GetBoolean() : (bool?)null
                };
                //actualizamos parcialmente
                var result = await _personalService.PartialUpdate(id, personal1);
                return Ok(ApiResponse<Personal>.SuccessResponse(result, "Personal actualizado correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Personal>.ErrorResponse("Error al actualizar parcialmente el personal", ex.Message, 400));
            }
        }

        /// <summary>
        /// Elimina un miembro del personal
        /// </summary>
        /// <param name="id">identificador unico del personal</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var result = await _personalService.Delete(id);
                if (!result) { 
                    return NotFound(ApiResponse<bool>.ErrorResponse("Personal no encontrado", statusCode: 404));
                };
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Personal eliminado correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el personal", ex.Message, 400));
            }
        }
    }
}
