using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company> AddCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
    }
}
