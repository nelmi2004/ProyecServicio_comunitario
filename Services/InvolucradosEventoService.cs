using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
namespace ProyecServicio_comunitario.Services
{
    public class InvolucradosEventoService
    {
        public readonly AngelDbContext _context;
        public InvolucradosEventoService(AngelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvolucradosEvento>> GetAll() => await _context.InvolucradosEventos.ToListAsync();

        public async Task<InvolucradosEvento> GetById(int id)
        {
            var trasladosEvento = await _context.InvolucradosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (trasladosEvento == null) return null;
            return trasladosEvento;
        }

        public async Task<InvolucradosEvento> Create(InvolucradosEvento involucradosEvento)
        {
            _context.InvolucradosEventos.Add(involucradosEvento);
            await _context.SaveChangesAsync();
            return involucradosEvento;
        }

        public async Task<InvolucradosEvento> Update(int id, InvolucradosEvento involucradosEvento)
        {
            involucradosEvento.Id = id;
            _context.InvolucradosEventos.Update(involucradosEvento);
            await _context.SaveChangesAsync();
            return involucradosEvento;
        }

        public async Task<InvolucradosEvento> PartialUpdate(int id, InvolucradosEvento involucradosEvento)
        {
            //buscamos el involucrado
            var existingInvolucradosEvento = await _context.InvolucradosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (existingInvolucradosEvento == null) return null;
            //En evento de existir actualizamos solo los datos enviados.
            if (involucradosEvento.EventoId != null) existingInvolucradosEvento.EventoId = involucradosEvento.EventoId;   
            if (involucradosEvento.Cedula != null) existingInvolucradosEvento.Cedula = involucradosEvento.Cedula;
            if (involucradosEvento.NombreCompleto != null) existingInvolucradosEvento.NombreCompleto = involucradosEvento.NombreCompleto;
            if (involucradosEvento.Sexo != null) existingInvolucradosEvento.Sexo = involucradosEvento.Sexo;
            if (involucradosEvento.Edad != null) existingInvolucradosEvento.Edad = involucradosEvento.Edad;
            if (involucradosEvento.EstadoPaciente != null) existingInvolucradosEvento.EstadoPaciente = involucradosEvento.EstadoPaciente;
            _context.InvolucradosEventos.Update(existingInvolucradosEvento);
            await _context.SaveChangesAsync();
            return existingInvolucradosEvento;
        }

        public async Task<bool> Delete(int id)
        {
            //eliminamos si el elemento es encontrado en la base de datos
            var involucradosEvento = await _context.InvolucradosEventos.FirstOrDefaultAsync(t => t.Id == id);
            if (involucradosEvento == null) return false;
            _context.InvolucradosEventos.Remove(involucradosEvento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
