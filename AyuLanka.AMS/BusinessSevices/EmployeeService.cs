using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int? companyId = null)
        {
            return await _employeeRepository.GetAllEmployeesAsync(companyId);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(EmployeeRequestModel employeeRequestModel)
        {
            var password = HashPassword(employeeRequestModel.Password);
            var employee = new Employee()
            {
                Address = employeeRequestModel.Address,
                FullName = employeeRequestModel.FullName,
                CallingName = employeeRequestModel.CallingName,
                EmployeeNumber = employeeRequestModel.EmployeeNumber,
                DesignationId = employeeRequestModel.DesignationId,
                EmploymentTypeId = employeeRequestModel.EmploymentTypeId,
                JoinedDate = employeeRequestModel.JoinedDate,
                NIC = employeeRequestModel.NIC,
                ShiftMasterId = employeeRequestModel.ShiftMasterId,
                Username = employeeRequestModel.Username,
                Password = password,
                CompanyId = employeeRequestModel.CompanyId
            };
            return await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, EmployeeRequestModel employeeRequestModel)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return null;
            }

            // Update properties
            existingEmployee.FullName = employeeRequestModel.FullName;
            existingEmployee.CallingName = employeeRequestModel.CallingName;
            existingEmployee.EmployeeNumber = employeeRequestModel.EmployeeNumber;
            existingEmployee.Address = employeeRequestModel.Address;
            existingEmployee.NIC = employeeRequestModel.NIC;
            existingEmployee.JoinedDate = employeeRequestModel.JoinedDate;
            existingEmployee.ShiftMasterId = employeeRequestModel.ShiftMasterId;
            existingEmployee.DesignationId = employeeRequestModel.DesignationId;
            existingEmployee.EmploymentTypeId = employeeRequestModel.EmploymentTypeId;
            existingEmployee.Username = employeeRequestModel.Username;

            // Handle password change appropriately
            if (!string.IsNullOrWhiteSpace(employeeRequestModel.Password))
            {
                existingEmployee.Password = HashPassword(employeeRequestModel.Password);
            }

            // Save changes
            await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
            return existingEmployee;
        }


        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

        public async Task<Employee> AuthenticateAsync(string username, string password)
        {
            var user = await _employeeRepository.GetByUsernameAsync(username);
            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task ResetPasswordAsync(int id, string newPassword)
        {
            var hashed = HashPassword(newPassword);
            await _employeeRepository.ResetPasswordAsync(id, hashed);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        private bool VerifyPassword(string input, string hashed)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(input, hashed);
        }
    }
}
