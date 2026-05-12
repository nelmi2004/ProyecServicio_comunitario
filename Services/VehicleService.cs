using ProyecServicio_comunitario.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace ProyecServicio_comunitario.Services
{
    public class VehicleService
    {
        private readonly AngelDbContext _context;

        public VehicleService(AngelDbContext context) {
            _context = context;
        }

        //Devuelve todos los vehiculos
        public async Task<IEnumerable<Vehiculo>> GetAll(){
            return await _context.Vehiculos.ToListAsync();
        }
        
        //Devuelve un vehiculo por id
        public async Task<Vehiculo?> GetById(int id) {

            Vehiculo? vehicle = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);
            return vehicle;

        }

        //crea un vehiculo
        public async Task<Vehiculo> Create(VehiculoDto vehicle) {
            //Mapear el DTO a la entidad Vehiculo
            Vehiculo newVehicle = new Vehiculo() {
                Placa = vehicle.placa,
                Modelo = vehicle.modelo,
                Tipo = vehicle.tipo,
                StatusVehiculo = (bool) vehicle.estado,
                Condicion = vehicle.condicion

            };
            //Guarda en la base de datos
            _context.Vehiculos.Add(newVehicle);
            await _context.SaveChangesAsync();
            return newVehicle;
        }

        //actualiza un vehiculo completamente
        public async Task<VehicleUpdateDto> Update(int id,VehicleUpdateDto vehicle) {
            //Mapear el DTO a la entidad Vehiculo
            Vehiculo newVehicle = new Vehiculo() {
                Id=id,
                Placa = vehicle.placa,
                Modelo = vehicle.modelo,
                Tipo = vehicle.tipo,
                StatusVehiculo = vehicle.estado,
                Condicion = vehicle.condicion
            };
            //Guarda en la base de datos
            _context.Vehiculos.Update(newVehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        //actualiza parcialmente un vehiculo
        public async Task<VehiclePartialDto> PartialUpdate(int id, VehiclePartialDto vehicle) {
            
            var vehicleDb = await _context.Vehiculos.FindAsync(id); 
            
            if (vehicleDb == null) return null;

            // Solo asignamos si el valor NO es nulo
            if (!string.IsNullOrWhiteSpace(vehicle.placa))
                vehicleDb.Placa = vehicle.placa;

            if (!string.IsNullOrWhiteSpace(vehicle.modelo))
                vehicleDb.Modelo = vehicle.modelo;

            if (!string.IsNullOrWhiteSpace(vehicle.tipo))
                vehicleDb.Tipo = vehicle.tipo;

            if (!string.IsNullOrWhiteSpace(vehicle.condicion))
                vehicleDb.Condicion = vehicle.condicion;

            if (vehicle.estado.HasValue)
                vehicleDb.StatusVehiculo = vehicle.estado.Value;

            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<bool> Delete(int id) {
            var vehicle = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return false;
            _context.Vehiculos.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
