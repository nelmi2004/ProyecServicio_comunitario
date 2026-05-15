
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // El alias que creamos
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    private readonly EventoService _eventoService;

    public EventosController(EventoService eventoService)
    {
        _eventoService = eventoService;
    }

    // GET: api/Eventos
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Evento>>>> GetEventos()
    {
        var result = await _eventoService.GetAll();
        return Ok(ApiResponse<IEnumerable<Evento>>.SuccessResponse(result.ToList(), "Eventos obtenidos correctamente", 200));
    }

    // GET: api/Eventos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Evento>>> GetEvento(Guid id)
    {
        var evento = await _eventoService.GetById(id);
        if (evento == null)
            return NotFound(ApiResponse<Evento>.ErrorResponse("Evento no encontrado", statusCode: 404));

        return Ok(ApiResponse<Evento>.SuccessResponse(evento, "Evento obtenido correctamente", 200));
    }

    // POST: api/Eventos
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Evento>>> PostEvento(Evento evento)
    {
        try
        {
            var created = await _eventoService.Create(evento);
            return CreatedAtAction(
                nameof(GetEvento),
                new { id = created.Id },
                ApiResponse<Evento>.SuccessResponse(created, "Evento creado exitosamente", 201)
            );
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(
                500,
                ApiResponse<object>.ErrorResponse("Error al crear el evento", ex.Message, 500)
            );
        }
    }

    // PUT: api/Eventos/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> PutEvento(Guid id, Evento evento)
    {
        if (id != evento.Id)
            return BadRequest(ApiResponse<object>.ErrorResponse("El id del evento no coincide con el id de la ruta", statusCode: 400));

        try
        {
            var updated = await _eventoService.Update(id, evento);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Evento actualizado correctamente", 200));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el evento", ex.Message, 500));
        }
    }

    // DELETE: api/Eventos/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteEvento(Guid id)
    {
        try
        {
        var deleted = await _eventoService.Delete(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.ErrorResponse("Evento no encontrado", statusCode: 404));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Evento eliminado correctamente", 200));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("Error al eliminar el evento", ex.Message, 500));
        }
    }
}
