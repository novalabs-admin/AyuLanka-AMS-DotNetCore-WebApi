using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class DayOffChangeDetailRepository : IDayOffChangeDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public DayOffChangeDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DayOffChangeDetail> CreateDayOffChangeDetailAsync(DayOffChangeDetail dayOffChangeDetail)
        {
            _context.DayOffChangeDetails.Add(dayOffChangeDetail);
            await _context.SaveChangesAsync();
            return dayOffChangeDetail;
        }

        public async Task<IEnumerable<DayOffChangeDetail>> GetAllDayOffChangeDetailsAsync(int? companyId = null)
        {
            var query = _context.DayOffChangeDetails
                .Include(c => c.DayOffChangeMaster.Employee)
                .Where(c => c.IsApproved == false)
                .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(c => c.DayOffChangeMaster.Employee.CompanyId == companyId.Value);
            return await query.OrderBy(c => c.DayOffChangeMaster.Employee.EmployeeNumber).ToListAsync();
        }

        public async Task<DayOffChangeDetail> GetDayOffChangeDetailByDatePreAndRosterAsync(DateTime dayOffPre, int staffRosterId)
        {
            return await _context.DayOffChangeDetails.Where(c => c.DayOffPre == dayOffPre && c.StaffRosterId == staffRosterId)
                .FirstOrDefaultAsync();
        }

        public async Task<DayOffChangeDetail> GetDayOffChangeDetailByIdAsync(int id)
        {
            return await _context.DayOffChangeDetails.Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<DayOffChangeDetail> UpdateDayOffChangeDetailAsync(DayOffChangeDetail dayOffChangeDetail)
        {
            _context.DayOffChangeDetails.Update(dayOffChangeDetail);
            await _context.SaveChangesAsync();
            return dayOffChangeDetail;
        }
    }
}
