using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;

namespace ProyecServicio_comunitario.Services
{
    public class CompanyService
    {
        public readonly AngelDbContext _context;

        public CompanyService(AngelDbContext context)
        {
            _context = context;
        }


        public async Task<List<Company>> GetAll() => await _context.Companies.ToListAsync();

        public async Task<Company> GetById(int id) {
            Company company = await _context.Companies.FindAsync(id);
            if (company == null) return null;
            return company;
        }
        public async Task<Company> Create(Company company) {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> Update(int id, Company company) {
            company.Id = id;
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> PartialUpdate(int id, Company company) {
            //buscamos la compañia
            Company oldCompany = await _context.Companies.FindAsync(id);
            if (oldCompany == null) return null;
            //de existir, se actualizan los datos que se hayan enviado
            if (company.Nombre != null) oldCompany.Nombre = company.Nombre;
            if (company.Rif != null) oldCompany.Rif = company.Rif;
            if (company.DireccionFiscal != null) oldCompany.DireccionFiscal = company.DireccionFiscal;
            if (company.TelefonoMaster != null) oldCompany.TelefonoMaster = company.TelefonoMaster;
            if (company.Email != null) oldCompany.Email = company.Email;
            if (company.UrlWeb != null) oldCompany.UrlWeb = company.UrlWeb;
            if (company.LogoUrl != null) oldCompany.LogoUrl = company.LogoUrl;
            //Guardar los cambios
            _context.Companies.Update(oldCompany);
            await _context.SaveChangesAsync();
            return oldCompany;
        }

        public async Task<bool> Delete(int id) {
            Company company = await _context.Companies.FindAsync(id);
            if (company == null) return false;
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
