using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AyuLanka.AMS.Repositories
{
    public class StaffLeaveRepository : IStaffLeaveRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffLeaveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateAsync(DateTime date)
        {
            return await _context.StaffLeaves
                                 .Include(s => s.Employee)
                                 .Include(s => s.Employee.Designation)
                                 .Include(s => s.LeaveType)
                                 .Where(s => s.FromDate <= date && s.ToDate >= date)
                                 .OrderBy(l => l.Employee.EmployeeNumber)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<StaffLeave>> GetAllStaffLeavesAsync(int? companyId = null)
        {
            var query = _context.StaffLeaves
                                .Include(l => l.Employee)
                                .Include(l => l.LeaveType)
                                .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(l => l.Employee.CompanyId == companyId.Value);
            return await query.OrderBy(l => l.Employee.EmployeeNumber).ToListAsync();
        }

        public async Task<StaffLeave> GetStaffLeaveByIdAsync(int id)
        {
            return await _context.StaffLeaves.FindAsync(id);
        }
        
        public async Task<StaffLeave> GetEmployeeScheduleAsync(int employeeId, string scheduledate)
        {
            var format = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(India Standard Time)'";
            DateTimeOffset parsedDate;

            // Try parsing with the exact format including timezone.
            if (!DateTimeOffset.TryParseExact(scheduledate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                throw new ArgumentException("Invalid date format", nameof(scheduledate));
            }

            // Use the Date component for comparison in database queries.
            DateTime dateOnly = parsedDate.Date;

            return await _context.StaffLeaves
                                .Where(l => l.EmployeeId == employeeId
                                            && l.FromDate <= dateOnly
                                            && l.ToDate >= dateOnly)
                                .FirstOrDefaultAsync();
        }

        public async Task<StaffLeave> AddStaffLeaveAsync(StaffLeave StaffLeave)
        {
            _context.StaffLeaves.Add(StaffLeave);
            await _context.SaveChangesAsync();
            return StaffLeave;
        }

        public async Task<StaffLeave> UpdateStaffLeaveAsync(StaffLeave StaffLeave)
        {
            _context.Entry(StaffLeave).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return StaffLeave;
        }

        public async Task DeleteStaffLeaveAsync(int id)
        {
            var StaffLeave = await _context.StaffLeaves.FindAsync(id);
            if (StaffLeave != null)
            {
                _context.StaffLeaves.Remove(StaffLeave);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.StaffLeaves
                .Include(l => l.Employee)
                .Where(leave => leave.FromDate >= startDate && leave.ToDate <= endDate)
                .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(l => l.Employee.CompanyId == companyId.Value);
            return await query.ToListAsync();
        }
    }
}
