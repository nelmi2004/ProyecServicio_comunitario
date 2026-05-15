using Microsoft.AspNetCore.Mvc;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Models.Common;
using ProyecServicio_comunitario.Services;

namespace ProyecServicio_comunitario.Controllers
{
/// <summary>
/// Controlador para la gestión de empresas (Company).
/// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        /// <summary>
/// Controlador para la gestión de empresas (Company).
/// </summary>
        public readonly CompanyService _companyService;
/// <summary>
/// Controlador para la gestión de empresas (Company).
/// </summary>
        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Permite obtener todas las Compañias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async  Task<ActionResult<ApiResponse<IEnumerable<Company>>>> GetAll() {
            try 
            {

                var result = await _companyService.GetAll();
                return Ok(ApiResponse<IEnumerable<Company>>.SuccessResponse(result, "Compañias obtenidas exitosamente", 200));
            } catch (Exception ex) 
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener companias", ex.Message, 400));
            }
        }

        /// <summary>
        /// Permite Obtener una Compañia por su id
        /// </summary>
        /// <param name="id">Identificador unico de la Compañia</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Company>>> GetById(int id)
        {
            try
            {
                Company company = await _companyService.GetById(id);
                if (company == null) { 
                    return NotFound(ApiResponse<object>.ErrorResponse("Compañia no encontrada", "Compañia no encontrada", 404));
                }
                return Ok(ApiResponse<Company>.SuccessResponse(company, "Compañia obtenida exitosamente", 200));
            }
            catch (Exception ex) {
                return BadRequest(ApiResponse<object>.ErrorResponse("Error al obtener compania", ex.Message, 400));
            }
        }
        ///<summary>
        ///Permite crear una nueva compañia
        /// </summary>
        ///<remarks>
        ///Ejemplo de uso:
        ///{
        ///     "Nombre": "Empresa 1",
        ///     "Rif": "J-12345678-9",
        ///     "Email": "4Fv0H@example.com",
        ///     "TelefonoMaster": "1234567890",
        ///     "UrlWeb": "www.empresa1.com",
        ///     "DireccionFiscal": "Av. Principal 123",
        ///     "LogoUrl": "http:logo.com/empresa1.jpg"
        /// }
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Company>>> Create(System.Text.Json.JsonElement companyJson)
        {
            try
            {
                //mapeamos el json a una entidad
                Company company = new Company()
                {
                    Nombre = companyJson.GetProperty("Nombre").GetString(),
                    Rif = companyJson.GetProperty("Rif").GetString(),
                    Email = companyJson.GetProperty("Email").GetString(),
                    TelefonoMaster = companyJson.GetProperty("TelefonoMaster").GetString(),
                    UrlWeb = companyJson.GetProperty("UrlWeb").GetString(),
                    DireccionFiscal = companyJson.GetProperty("DireccionFiscal").GetString(),
                    LogoUrl = companyJson.GetProperty("LogoUrl").GetString()
                };
                //creamos
                Company CreatedCompany = await _companyService.Create(company);
                return Ok(ApiResponse<Company>.SuccessResponse(CreatedCompany, "Compañia creada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Company>.ErrorResponse("Error al crear compañia", ex.Message, 400));
            }
        }
        /// <summary>
        /// Permite actualizar los datos de la compañia
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Angeles de la autopista",
        ///     "Rif": "J-12345678-9",
        ///     "Email": "Angeles@gmail.com",
        ///     "TelefonoMaster": "1234567890",
        ///     "UrlWeb": "www.angeles.com",
        ///     "DireccionFiscal": "Av. Principal 123",
        ///     "LogoUrl": "http:logo.com/angel.jpg" 
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico de la compañia</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Company>>> Update(int id, System.Text.Json.JsonElement companyJson)
        {
            try 
            { 
                //mapeamos el json a una entidad
                Company company = new Company() 
                {
                    Nombre = companyJson.GetProperty("Nombre").GetString(),
                    Rif = companyJson.GetProperty("Rif").GetString(),
                    Email = companyJson.GetProperty("Email").GetString(),
                    TelefonoMaster = companyJson.GetProperty("TelefonoMaster").GetString(),
                    UrlWeb = companyJson.GetProperty("UrlWeb").GetString(),
                    DireccionFiscal = companyJson.GetProperty("DireccionFiscal").GetString(),
                    LogoUrl = companyJson.GetProperty("LogoUrl").GetString()
                };
                //actualizamos
                Company UpdatedCompany = await _companyService.Update(id, company);
                return Ok(ApiResponse<Company>.SuccessResponse(UpdatedCompany, "Compañia actualizada exitosamente", 200));
                
            }catch(Exception ex) 
            {
                return BadRequest(ApiResponse<Company>.ErrorResponse("Error al actualizar compania", ex.Message, 400));
            }
        }

        /// <summary>
        /// Permite actualizar solo los datos enviados de la compañia
        /// </summary>
        /// <remarks>
        /// Ejemplo de uso:
        /// {
        ///     "Nombre": "Angeles de la Autopista", 
        ///     "Email": "AngelesAutopista@gmail.com",
        ///     "TelefonoMaster": "04241627821",
        /// }
        /// </remarks>
        /// <param name="id">Identificador unico de la compañia</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<Company>>> PartialUpdate(int id, System.Text.Json.JsonElement companyJson)
        {
            try
            {
                //mapeamos el json a una entidad compañia
                Company company = new Company()
                {
                    Nombre = companyJson.TryGetProperty("Nombre", out var Nombre) ? Nombre.GetString() : null,
                    Rif = companyJson.TryGetProperty("Rif", out var Rif) ? Rif.GetString() : null,
                    Email = companyJson.TryGetProperty("Email", out var Email) ? Email.GetString() : null,
                    TelefonoMaster = companyJson.TryGetProperty("TelefonoMaster", out var TelefonoMaster) ? TelefonoMaster.GetString() : null,
                    UrlWeb = companyJson.TryGetProperty("UrlWeb", out var UrlWeb) ? UrlWeb.GetString() : null,
                    DireccionFiscal = companyJson.TryGetProperty("DireccionFiscal", out var DireccionFiscal) ? DireccionFiscal.GetString() : null,
                    LogoUrl = companyJson.TryGetProperty("LogoUrl", out var LogoUrl) ? LogoUrl.GetString() : null
                };
                //actualizamos
                Company UpdatedCompany = await _companyService.PartialUpdate(id, company);
                return Ok(ApiResponse<Company>.SuccessResponse(UpdatedCompany, "Compañia actualizada exitosamente", 200));
            }
            catch (Exception e) {
                return BadRequest(ApiResponse<Company>.ErrorResponse("Error al actualizar compania", e.Message, 400));
            }
        }

        /// <summary>
        /// Permite Eliminar una Compañia
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Identificador unico de la Compañia</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id) 
        {
            try
            {
                bool deleted = await _companyService.Delete(id);
                if (!deleted) {
                    return BadRequest(ApiResponse<bool>.ErrorResponse("Error al eliminar compañia", "Compañia no encontrada", 400));
                }
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Compañia eliminada exitosamente", 200));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Company>.ErrorResponse("Error al eliminar compania", ex.Message, 400));
            }
        }


    }
}
