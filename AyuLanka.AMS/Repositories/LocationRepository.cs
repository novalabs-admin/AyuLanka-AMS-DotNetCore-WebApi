using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Location> GetLocationByLocationIdAsync(int locationId)
        {
            return await _context.Locations
                .Where(p => p.Id == locationId)
                .FirstOrDefaultAsync() ?? new Location();
        }

        public async Task<IEnumerable<Location>> GetAllLocationAsync(int? companyId = null)
        {
            var query = _context.Locations.AsQueryable();
            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);
            return await query.OrderBy(p => p.IsTreatmentLocation).ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetPrimeCareLocationAsync(int? companyId = null)
        {
            var query = _context.Locations
                .Where(p => p.LocationTypeId == 1 && p.IsTreatmentLocation == true);
            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetEliteCareLocationAsync(int? companyId = null)
        {
            var query = _context.Locations
                .Where(p => p.LocationTypeId == 2 && p.IsTreatmentLocation == true);
            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetDoctorChannelingLocationAsync(int? companyId = null)
        {
            var query = _context.Locations
                .Where(p => p.LocationTypeId == 3 && p.IsTreatmentLocation == true);
            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);
            return await query.ToListAsync();
        }

        public async Task<Location> GetTreatmentLocationByNameAsync(string locationName, int? companyId = null)
        {
            var query = _context.Locations.Where(p => p.Name == locationName);
            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);
            return await query.FirstOrDefaultAsync();
        }
    }
}
