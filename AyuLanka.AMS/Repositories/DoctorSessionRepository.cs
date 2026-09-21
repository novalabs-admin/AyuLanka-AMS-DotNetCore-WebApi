using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class DoctorSessionRepository : IDoctorSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorSession>> GetAllAsync(int? companyId, DateTime? date)
        {
            var query = _context.DoctorSessions
                .Include(ds => ds.Doctor)
                .Include(ds => ds.Company)
                .AsQueryable();

            if (companyId.HasValue)
                query = query.Where(ds => ds.CompanyId == companyId.Value);

            if (date.HasValue)
                query = query.Where(ds => ds.SessionDate.Date == date.Value.Date);

            return await query.OrderBy(ds => ds.SessionDate).ThenBy(ds => ds.StartTime).ToListAsync();
        }

        public async Task<DoctorSession?> GetByIdAsync(int id)
        {
            return await _context.DoctorSessions
                .Include(ds => ds.Doctor)
                .Include(ds => ds.Company)
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

        public async Task<IEnumerable<DoctorSession>> GetByDoctorAsync(int doctorId, DateTime? date)
        {
            var query = _context.DoctorSessions
                .Include(ds => ds.Company)
                .Where(ds => ds.DoctorId == doctorId);

            if (date.HasValue)
                query = query.Where(ds => ds.SessionDate.Date == date.Value.Date);

            return await query.OrderBy(ds => ds.SessionDate).ThenBy(ds => ds.StartTime).ToListAsync();
        }

        public async Task<int> GetBookedCountAsync(int sessionId)
        {
            return await _context.AppointmentSchedules
                .CountAsync(a => a.DoctorSessionId == sessionId && a.IsDeleted != true);
        }

        public async Task<DoctorSession> AddAsync(DoctorSession session)
        {
            session.CreatedDate = DateTime.Now;
            _context.DoctorSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<DoctorSession> UpdateAsync(DoctorSession session)
        {
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return session;
        }
    }
}
