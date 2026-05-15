using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioService _servicioService;

        public ServicioController(ServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Servicio>>>> GetServicios()
        {
            try
            {
                var result = await _servicioService.GetAll();
                return Ok(ApiResponse<IEnumerable<Servicio>>.SuccessResponse(result, "Servicios obtenidos correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener servicios", ex.Message, 400));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Servicio>>> GetServicio(Guid id)
        {
            try
            {
                var servicio = await _servicioService.GetById(id);
                if (servicio == null)
                    return NotFound(ApiResponse<Servicio>.ErrorResponse("Servicio no encontrado", $"No existe un registro con el Id: {id}", 404));

                return Ok(ApiResponse<Servicio>.SuccessResponse(servicio, "Servicio obtenido correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Servicio>.ErrorResponse("Error al obtener servicio", ex.Message, 400));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Servicio>>> PostServicio([FromBody] Servicio servicio)
        {
            try
            {
                var created = await _servicioService.Create(servicio);
                return CreatedAtAction(
                    nameof(GetServicio),
                    new { id = created.Id },
                    ApiResponse<Servicio>.SuccessResponse(created, "Servicio creado correctamente", 201)
                );
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el servicio", ex.Message, 500));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al crear el servicio", ex.Message, 400));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Servicio>>> PutServicio(Guid id, [FromBody] Servicio servicio)
        {
            if (servicio == null)
                return BadRequest(ApiResponse<Servicio>.ErrorResponse("Cuerpo inválido", "No se proporcionó un objeto Servicio", 400));

            try
            {
                // si el cliente manda un Id distinto, se fuerza a usar el de la ruta
                servicio.Id = id;

                var updated = await _servicioService.Update(id, servicio);
                if (updated == null)
                    return NotFound(ApiResponse<Servicio>.ErrorResponse("Servicio no encontrado", $"No existe un registro con el Id: {id}", 404));

                return Ok(ApiResponse<Servicio>.SuccessResponse(updated, "Servicio actualizado correctamente", 200));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Conflicto al actualizar", ex.Message, 409));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al actualizar el servicio", ex.Message, 400));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteServicio(Guid id)
        {
            try
            {
                var deleted = await _servicioService.Delete(id);
                if (!deleted)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Servicio no encontrado", $"No existe un registro con el Id: {id}", 404));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Servicio eliminado correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar el servicio", ex.Message, 400));
            }
        }
    }
}

