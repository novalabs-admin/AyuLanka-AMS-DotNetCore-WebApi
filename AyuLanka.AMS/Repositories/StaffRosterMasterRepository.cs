using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class StaffRosterMasterRepository : IStaffRosterMasterRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffRosterMasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StaffRosterMaster>> GetAllStaffRosterMastersAsync()
        {
            return await _context.StaffRosterMasters.ToListAsync();
        }

        public async Task<StaffRosterMaster> GetStaffRosterMasterByIdAsync(int id)
        {
            return await _context.StaffRosterMasters.FindAsync(id);
        }

        public async Task<StaffRosterMaster> CreateRosterMasterAsync(StaffRosterMaster rosterMaster)
        {
            _context.StaffRosterMasters.Add(rosterMaster);
            await _context.SaveChangesAsync();
            return rosterMaster;
        }

        public async Task<List<StaffRosterMaster>> GetRosterDateRangesAsync(int? companyId = null)
        {
            var query = _context.StaffRosterMasters.AsQueryable();
            if (companyId.HasValue)
                query = query.Where(r => r.CompanyId == companyId.Value);
            return await query.ToListAsync();
        }

        public async Task<StaffRosterMaster> GetRosterMasterByIdAsync(int rosterMasterId)
        {
            return await _context.StaffRosterMasters.FindAsync(rosterMasterId);
        }

        public async Task UpdateRosterMasterAsync(StaffRosterMaster rosterMaster)
        {
            _context.StaffRosterMasters.Update(rosterMaster);
            await _context.SaveChangesAsync();
        }
    }
}
