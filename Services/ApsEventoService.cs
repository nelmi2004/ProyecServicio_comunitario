using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class ApsEventoService
    {
        public readonly AngelDbContext _context;

        public ApsEventoService(AngelDbContext context)
        {
            _context = context;
        }


        public async Task<List<ApsEvento>> GetAll() => await _context.ApsEventos.ToListAsync();

        public async Task<ApsEvento?> GetById(int id)
        {
            var aps = await _context.ApsEventos.FirstOrDefaultAsync(e => e.Id == id);
            if (aps == null) return null;
            return aps;
        }

        public async Task<ApsEvento> Create(ApsEvento entity) 
        {
            //Validamos que el id del caso sea el mismo que el id del involucrado
            var involucradoCasoId = await _context.InvolucradosEventos.Where(i => i.Id == entity.InvolucradoId).Select(involucrado => involucrado.CasoId).FirstOrDefaultAsync();
            if (involucradoCasoId != entity.CasoId) return null;
            _context.ApsEventos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ApsEvento> Update(int id,ApsEvento entity) 
        {
            entity.Id = id;
            var involucradoCasoId = await _context.InvolucradosEventos.Where(i => i.Id == entity.InvolucradoId).Select(involucrado => involucrado.CasoId).FirstOrDefaultAsync();
            if (involucradoCasoId != entity.CasoId) return null;
            _context.ApsEventos.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ApsEvento> PartialUpdate(int id, ApsEvento entity) 
        {
            //buscamos el Aps en la base de datos
            var aps = await _context.ApsEventos.FindAsync(id);
            if (aps == null) return null;

            //actualizamos los campos que sean diferentes de null
            if (entity.CasoId != null) aps.CasoId = entity.CasoId;
            if (entity.DetalleAtencion != null) aps.DetalleAtencion = entity.DetalleAtencion;
            if (entity.EstadoPacientePostAps != null) aps.EstadoPacientePostAps = entity.EstadoPacientePostAps;
            if (entity.InvolucradoId != -1) {
                var involucradoCasoId = await _context.InvolucradosEventos.Where(i => i.Id == entity.InvolucradoId).Select(involucrado => involucrado.CasoId).FirstOrDefaultAsync();
                //Validamos que el id del caso sea el mismo que el id del involucrado
                if (involucradoCasoId != entity.CasoId) return null;
                //asignamos el id del involucrado
                aps.InvolucradoId = entity.InvolucradoId; 
            }
            if (entity.FechaAtencion != null) aps.FechaAtencion = entity.FechaAtencion;
            _context.ApsEventos.Update(aps);
            await _context.SaveChangesAsync();
            return aps;
        }

        public async Task<bool> Delete(int id) 
        {
            var aps = await _context.ApsEventos.FindAsync(id);
            if (aps == null) return false;
            _context.ApsEventos.Remove(aps);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
