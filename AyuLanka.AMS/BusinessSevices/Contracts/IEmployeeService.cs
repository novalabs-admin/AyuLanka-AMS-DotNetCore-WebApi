using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(int? companyId = null);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(EmployeeRequestModel employeeRequestModel);
        Task<Employee> UpdateEmployeeAsync(int id, EmployeeRequestModel employeeRequestModel);
        Task DeleteEmployeeAsync(int id);
        Task<Employee> AuthenticateAsync(string username, string password);
        Task ResetPasswordAsync(int id, string newPassword);
    }
}
