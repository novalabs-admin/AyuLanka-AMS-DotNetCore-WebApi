using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int? companyId = null)
        {
            var query = _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.EmploymentType)
                .AsQueryable();

            if (companyId.HasValue)
                query = query.Where(e => e.CompanyId == companyId.Value);

            return await query.OrderBy(e => e.EmployeeNumber).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetByUsernameAsync(string username)
        {
            return await _context.Employees
                                 .Include(e => e.Designation)
                                 .Include(e => e.Company)
                                 .FirstOrDefaultAsync(e => e.Username == username);
        }

        public async Task ResetPasswordAsync(int id, string newHashedPassword)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee {id} not found.");
            employee.Password = newHashedPassword;
            await _context.SaveChangesAsync();
        }
    }
}
