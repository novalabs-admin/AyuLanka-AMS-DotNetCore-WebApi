using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            company.CreatedDate = DateTime.Now;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return company;
        }
    }
}
