using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models.Common;

namespace ProyecServicio_comunitario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class VehicleController : Controller
    {

        private readonly VehicleService _vehicleService;




        public VehicleController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }



        ///<summary>
        ///Devuelve multiples vehiculos de la base de datos
        ///</summary>
        ///
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Vehiculo>>>> Get() {
            try {
                var result =  await _vehicleService.GetAll();
                return Ok(ApiResponse<IEnumerable<Vehiculo>>.SuccessResponse(result, "Vehiculos obtenidos correctamente", 200));
            } catch (Exception ex) { 
                return BadRequest(ApiResponse<object>.ErrorResponse("Error inesperado no se pudo obtener el vehiculo", ex.Message, 400));
            }
        }


        /// <summary>
        /// Devuelve la informacion de un vehiculo por su id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">El vehículo se actualizó correctamente y se devuelve el objeto.</response>
        /// <response code="400">Los datos enviados son inválidos o el ID no coincide.</response>
        /// <response code="404">No se encontró ningún vehículo con el ID proporcionado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<Vehiculo>>> GetById(int id) {
            try
            {
                var vehicle = await _vehicleService.GetById(id);
                if (vehicle == null) {
                    return NotFound(ApiResponse<Vehiculo>.ErrorResponse("No se encontró el vehículo", statusCode: 404));
                }
                return Ok(ApiResponse<Vehiculo>.SuccessResponse(vehicle, "Vehículo obtenido correctamente", 200));
            }
            catch (Exception ex) { 

                return BadRequest(ApiResponse<Vehiculo>.ErrorResponse("Error inesperado no se pudo obtener el vehículo", ex.Message, 400));

                
            }
        }
        /// <summary>
        /// Crea un vehiculo en la base de datos
        /// </summary>
        /// <returns></returns>
        // POST: VehicleController/Create
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<vehicleResponseDto>>> Create([FromBody] VehiculoDto vehiculoDto)
        {
            try
            {
                var createdVehicle = await _vehicleService.Create(vehiculoDto);

                var responseDto = new vehicleResponseDto
                {
                    id = createdVehicle.Id,
                    placa = createdVehicle.Placa,
                    modelo = createdVehicle.Modelo,
                    tipo = createdVehicle.Tipo,
                    estado = createdVehicle.StatusVehiculo,
                    condicion = createdVehicle.Condicion
                };

                return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, ApiResponse<vehicleResponseDto>.SuccessResponse(responseDto, "Vehículo creado exitosamente", 201));
            }
            catch (Exception ex)
            {
                //devolucion de un error genérico
                return BadRequest(ApiResponse<vehicleResponseDto>.ErrorResponse("No se pudo crear el vehículo", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza la informacion de un vehiculo
        /// </summary>
        /// <param name="id">Identificador del vehiculo</param>
        /// <returns>
        ///     
        /// </returns>
        // PUT: VehicleController/Edit/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<object>>> Edit(int id, [FromBody] VehicleUpdateDto vehicleUpdate)
        {
            try
            {
                var updatedVehicle = await _vehicleService.Update(id, vehicleUpdate);
                if (updatedVehicle == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Vehículo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<object>.SuccessResponse(null, "Vehículo actualizado correctamente", 204));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("No se pudo actualizar el vehículo", ex.Message, 400));
            }
        }

        /// <summary>
        /// Actualiza solo los campos enviados de un vehiculo.
        /// </summary>
        /// <param name="id">Identificador del vehiculo</param>
        /// <returns>
        ///     
        /// </returns>
        // PATCH: VehicleController/Edit/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<object>>> PartialEdit(int id, [FromBody] VehiclePartialDto vehicleUpdate)
        {
            try
            {
                var updatedVehicle = await _vehicleService.PartialUpdate(id, vehicleUpdate);
                if (updatedVehicle == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Vehículo no encontrado", statusCode: 404));
                }
                return Ok(ApiResponse<object>.SuccessResponse(null, "Vehículo actualizado parcialmente", 204));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("No se pudo actualizar el vehículo", ex.Message, 400));
            }
        }
  
        /// <summary>
        /// Elimina un vehiculo de la base de datos.
        /// </summary>
        /// <param name="id">Identificador unico del vehiculo</param>
        /// <returns></returns>
        // POST: VehicleController/Delete/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                bool deleted = await _vehicleService.Delete(id);
                if (!deleted)
                {
                    return BadRequest(ApiResponse<bool>.ErrorResponse("No se pudo eliminar el vehículo", statusCode: 400));

                }
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Vehículo eliminado correctamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Ocurrió un fallo inesperado al intentar eliminar el vehículo de la base de datos", ex.Message, 400));
            }
        }
    }
}
