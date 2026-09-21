using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _companyRepository.GetAllCompaniesAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _companyRepository.GetCompanyByIdAsync(id);
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            return await _companyRepository.AddCompanyAsync(company);
        }

        public async Task<Company> UpdateCompanyAsync(int id, Company company)
        {
            var existing = await _companyRepository.GetCompanyByIdAsync(id);
            if (existing == null) return null;

            existing.Name = company.Name;
            existing.Address = company.Address;
            existing.PhoneNo = company.PhoneNo;
            existing.IsActive = company.IsActive;

            return await _companyRepository.UpdateCompanyAsync(existing);
        }
    }
}
