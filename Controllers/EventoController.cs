
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models; // El alias que creamos

namespace ProyecServicio_comunitario.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    private readonly AngelDbContext _context;

    public EventosController(AngelDbContext context)
    {
        _context = context;
    }

    // GET: api/Eventos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
    {
        return await _context.Eventos.ToListAsync();
    }

    // GET: api/Eventos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Evento>> GetEvento(Guid id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();
        return evento;
    }

    // POST: api/Eventos
    [HttpPost]
    public async Task<ActionResult<Evento>> PostEvento(Evento evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
    }

    // PUT: api/Eventos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvento(Guid id, Evento evento)
    {
        if (id != evento.Id) return BadRequest();
        _context.Entry(evento).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Eventos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvento(Guid id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();
        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}