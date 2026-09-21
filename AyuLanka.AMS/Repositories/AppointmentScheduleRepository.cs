using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.Data;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AyuLanka.AMS.Repositories
{
    public class AppointmentScheduleRepository : IAppointmentScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync(int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.Employee.EmployeeNumber).ToListAsync();
        }

        public async Task<AppointmentSchedule?> GetAppointmentScheduleByIdAsync(int id)
        {
            return await _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.AppointmentTreatments) // Include related AppointmentTreatments
                            .ThenInclude(at => at.TreatmentType) // Include TreatmentLocation within AppointmentTreatments
                        .Include(a => a.Employee) // Include Employee
                        .OrderBy(a => a.Employee.EmployeeNumber)
                        .Where(a => a.Id == id)
                        .Where(a => a.IsDeleted != true)
                        .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateAsync(DateTime date, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= date.Date && a.ScheduleDate < date.Date.AddDays(1))
                        .Where(a => a.IsDeleted != true)
                        .Where(a => a.Location != null && a.Location.LocationTypeId == 2)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= date.Date && a.ScheduleDate < date.Date.AddDays(1))
                        .Where(a => a.IsDeleted != true)
                        .Where(a => a.Location != null && a.Location.LocationTypeId == 1)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Include(a => a.ChildAppointments)
                        .Where(a => a.ScheduleDate >= date.Date && a.ScheduleDate < date.Date.AddDays(1))
                        .Where(a => a.TokenNo != null)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAndCompanyCodeAsync(DateTime date, string? companyCode = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Include(a => a.ChildAppointments)
                        .Include(a => a.Company)
                        .Where(a => a.ScheduleDate >= date.Date && a.ScheduleDate < date.Date.AddDays(1))
                        .Where(a => a.TokenNo != null)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (!string.IsNullOrWhiteSpace(companyCode))
                query = query.Where(a => a.Company != null && a.Company.CompanyCode == companyCode);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync(int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= DateTime.Now.Date && a.ScheduleDate < DateTime.Now.Date.AddDays(1))
                        .Where(a => a.TokenNo != null)
                        .Where(a => a.ChitNo != null)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.DeletedByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.IsDeleted == true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.IsDeleted != true)
                        .Where(a => a.Location != null && a.Location.LocationTypeId == 2)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.ChildAppointments)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.IsDeleted != true)
                        .Where(a => a.Location != null)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<DashboardDateChartDto>>GetAllDashboardChartsDatabyDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var appointments = await _context.AppointmentSchedules
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Where(a =>
                    a.ScheduleDate.Date >= startDate &&
                    a.ScheduleDate.Date <= endDate &&
                    a.CustomerId != null 
                    //&& a.ChitNo != null
                    //&& a.IsDeleted != true
                    )
                .ToListAsync();

            // First visit per customer
            var firstVisit = appointments
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            // First OPD date per customer
            var firstOpd = appointments
                .Where(a => a.AppointmentTreatments.All(t =>
                    t.TreatmentType.TreatmentShortCode.StartsWith("P")))
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            // First Wellness date per customer
            var firstWellness = appointments
                .Where(a => a.AppointmentTreatments.Any(t =>
                    t.TreatmentType.TreatmentShortCode.StartsWith("E")))
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            var result = appointments
                .GroupBy(a => a.ScheduleDate.Date)
                .Select(g =>
                {
                    var opd = g.Where(a =>
                        a.AppointmentTreatments.All(t => t.TreatmentType.TreatmentShortCode.StartsWith("P")));

                    var wellness = g.Where(a =>
                        a.AppointmentTreatments.All(t => t.TreatmentType.TreatmentShortCode.StartsWith("E")));

                    var opdWellness = g.Where(a =>
                        a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("P")) &&
                        a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("E")));


                    // OPD → Wellness conversions on THIS DATE
                    var convertedCount = firstWellness
                        .Where(x =>
                            x.Value == g.Key &&              // conversion date
                            firstOpd.ContainsKey(x.Key) &&
                            firstOpd[x.Key] < x.Value)       // OPD before wellness
                        .Select(x => x.Key)
                        .Distinct()
                        .Count();

                    return new DashboardDateChartDto
                    {
                        Date = g.Key,
                        OpdNew = opd.Count(a => firstVisit[a.CustomerId] == g.Key),
                        OpdRepeat = opd.Count(a => firstVisit[a.CustomerId] < g.Key),

                        WellnessNew = wellness.Count(a => firstVisit[a.CustomerId] == g.Key),
                        WellnessRepeat = wellness.Count(a => firstVisit[a.CustomerId] < g.Key),

                        OpdWellnessNew = opdWellness.Count(a => firstVisit[a.CustomerId] == g.Key),
                        OpdWellnessRepeat = opdWellness.Count(a => firstVisit[a.CustomerId] < g.Key),

                        OpdToWellnessConverted = convertedCount
                    };

                })
                .OrderBy(x => x.Date)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<DashboardSummaryDto>> GetDashboardSummaryByDateRangeAsync(DateTime startDate, DateTime endDate, string category)
        {
            var appointments = await _context.AppointmentSchedules
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Where(a => a.ScheduleDate.Date >= startDate &&
                            a.ScheduleDate.Date <= endDate &&
                            a.CustomerId != null
                            //&& a.ChitNo != null
                            //&& a.IsDeleted != true
                            )
                .ToListAsync();

            var firstVisit = appointments
                .GroupBy(a => a.CustomerId)
                .ToDictionary(g => g.Key, g => g.Min(x => x.ScheduleDate.Date));

            var summary = appointments
                .GroupBy(a => a.ScheduleDate.Date)
                .Select(g =>
                {
                    IEnumerable<AppointmentSchedule> filtered = g;

                    if (category == "OPD")
                    {
                        filtered = g.Where(a => a.AppointmentTreatments.All(
                            t => t.TreatmentType.TreatmentShortCode.StartsWith("P")));
                    }
                    else if (category == "Wellness")
                    {
                        filtered = g.Where(a => a.AppointmentTreatments.All(
                            t => t.TreatmentType.TreatmentShortCode.StartsWith("E")));
                    }
                    else if (category == "OPD + Wellness")
                    {
                        filtered = g.Where(a =>
                            a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("P")) &&
                            a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("E")));
                    }
                    else if (category == "OPD → Wellness")
                    {
                        var firstOpd = appointments
                            .Where(a => a.AppointmentTreatments.All(t =>
                                t.TreatmentType.TreatmentShortCode.StartsWith("P")))
                            .GroupBy(a => a.CustomerId)
                            .ToDictionary(g => g.Key, g => g.Min(x => x.ScheduleDate.Date));

                        var firstWellness = appointments
                            .Where(a => a.AppointmentTreatments.Any(t =>
                                t.TreatmentType.TreatmentShortCode.StartsWith("E")))
                            .GroupBy(a => a.CustomerId)
                            .ToDictionary(g => g.Key, g => g.Min(x => x.ScheduleDate.Date));

                        var convertedCount = firstWellness
                            .Where(x => firstOpd.ContainsKey(x.Key) &&
                                        firstOpd[x.Key] < x.Value &&
                                        x.Value == g.Key)
                            .Count();

                        return new List<DashboardSummaryDto>
                        {
                            new() { Date = g.Key, Type = "Converted", Count = convertedCount }
                        };
                    }

                    return new List<DashboardSummaryDto>
                    {
                        new() { Date = g.Key, Type = "New", Count = filtered.Count(a => firstVisit[a.CustomerId] == g.Key) },
                        new() { Date = g.Key, Type = "Repeat", Count = filtered.Count(a => firstVisit[a.CustomerId] < g.Key) }
                    };
                })
                .SelectMany(x => x)
                .OrderBy(x => x.Date)
                .ToList();

            return summary;
        }


        public async Task<IEnumerable<DashboardDetailsDto>> GetDashboardDetailsByDateAsync(DateTime date, string category, string type)
        {
            var appointments = await _context.AppointmentSchedules
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Include(a => a.Employee)
                .Where(a => a.ScheduleDate.Date == date.Date && a.CustomerId != null
                //&& a.ChitNo != null
                //&& a.IsDeleted != true
                )
                .ToListAsync();

            // First visit per customer
            var firstVisit = appointments
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            // First OPD date per customer
            var firstOpd = appointments
                .Where(a => a.AppointmentTreatments.All(t =>
                    t.TreatmentType.TreatmentShortCode.StartsWith("P")))
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            // First Wellness date per customer
            var firstWellness = appointments
                .Where(a => a.AppointmentTreatments.Any(t =>
                    t.TreatmentType.TreatmentShortCode.StartsWith("E")))
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.ScheduleDate.Date)
                );

            var filtered = appointments.Where(a =>
            {
                switch (category.ToUpper())
                {
                    case "OPD":
                        if (!a.AppointmentTreatments.All(t => t.TreatmentType.TreatmentShortCode.StartsWith("P"))) return false;
                        break;
                    case "WELLNESS":
                        if (!a.AppointmentTreatments.All(t => t.TreatmentType.TreatmentShortCode.StartsWith("E"))) return false;
                        break;
                    case "COMBINED":
                        if (!(a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("P")) &&
                              a.AppointmentTreatments.Any(t => t.TreatmentType.TreatmentShortCode.StartsWith("E"))))
                            return false;
                        break;
                    case "CONVERSION":
                        return firstOpd.ContainsKey(a.CustomerId) &&
                               firstWellness.ContainsKey(a.CustomerId) &&
                               firstWellness[a.CustomerId] == date &&
                               firstOpd[a.CustomerId] < firstWellness[a.CustomerId];
                    default:
                        return false;
                }

                // Filter by type
                if (type == "New")
                    return firstVisit[a.CustomerId] == date;
                if (type == "Repeat")
                    return firstVisit[a.CustomerId] < date;
                if (type == "Converted")
                    return category.ToUpper() == "CONVERSION"; // already filtered above

                return false;
            });

            return filtered.Select(a => new DashboardDetailsDto
            {
                CustomerName = a.CustomerName,
                ContactNo = a.ContactNo,
                Treatments = a.AppointmentTreatments.Select(t => t.TreatmentType.Name).ToList(),
                EmployeeName = a.Employee != null ? a.Employee.Username : "",
            }).ToList();
        }


        public async Task<IEnumerable<AppointmentSchedule>> GetCustomerDetailsByIdAsync(int customerId)
        {
            var results = await _context.AppointmentSchedules
                .Include(a => a.Location)
                .Include(a => a.EnteredByEmployee)
                .Include(a => a.ChildAppointments)
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Include(a => a.Employee)
                .OrderBy(a => a.ScheduleDate)
                .Where(a => a.IsDeleted != true)
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();

            return results;
        }


        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.ChitNo != null)
                        .Where(a => a.IsDeleted != true)
                        .Where(a => a.Location != null && a.Location.LocationTypeId == 1)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.ActualFromTime != null && a.ActualToTime != null)
                        .Where(a => a.EnteredDate < a.ScheduleDate)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                        .Include(a => a.Location)
                        .Include(a => a.EnteredByEmployee)
                        .Include(a => a.AppointmentTreatments)
                            .ThenInclude(at => at.TreatmentType)
                        .Include(a => a.Employee)
                        .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                        .Where(a => a.EnteredDate < a.ScheduleDate)
                        .Where(a => a.IsDeleted != true)
                        .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule)
        {
            _context.AppointmentSchedules.Add(appointmentSchedule);
            await _context.SaveChangesAsync();
            return appointmentSchedule;
        }

        public async Task<AppointmentSchedule> UpdateAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule)
        {
            _context.Entry(appointmentSchedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return appointmentSchedule;
        }

        public async Task DeleteAppointmentScheduleAsync(int id, int deletedByUserId, string remark)
        {
            var appointmentSchedule = await _context.AppointmentSchedules.FindAsync(id);
            if (appointmentSchedule != null)
            {
                appointmentSchedule.Remarks = remark;
                appointmentSchedule.IsDeleted = true;
                appointmentSchedule.DeletedBy = deletedByUserId;
                appointmentSchedule.DeletedDate = DateTime.Now;

                _context.Entry(appointmentSchedule).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetMaxChitNoAsync(DateTime scheduleDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                .Where(a => a.ScheduleDate.Date == scheduleDate.Date);
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.MaxAsync(a => (int?)a.ChitNo) ?? 0;
        }

        public async Task<bool> IsTokenExistsAsync(int tokenNo, DateTime scheduleDate, int? excludeAppointmentId = null)
        {
            return await _context.AppointmentSchedules.AnyAsync(a =>
                    a.TokenNo == tokenNo &&
                    a.IsDeleted == false &&
                    a.ScheduleDate.Date == scheduleDate.Date &&
                    (excludeAppointmentId == null || a.Id != excludeAppointmentId)
                );
        }

        public async Task<List<EmployeeForAppointmentDto>> GetEmployeesByAppointmentIdsAsync(List<int> appointmentIds)
        {
            return await _context.AppointmentSchedules
                .Where(a => appointmentIds.Contains(a.Id))
                .Select(a => new EmployeeForAppointmentDto
                {
                    AppointmentId = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeFullName = a.Employee != null ? a.Employee.FullName : null,
                    EmployeeCallingName = a.Employee != null ? a.Employee.CallingName : null,
                    EmployeeNumber = a.Employee != null ? a.Employee.EmployeeNumber : null,
                })
                .ToListAsync();
                
        public async Task<IEnumerable<AppointmentSchedule>> GetByDoctorSessionIdAsync(int sessionId, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Include(a => a.Employee)
                .Include(a => a.ChildAppointments)
                .Where(a => a.DoctorSessionId == sessionId)
                .Where(a => a.IsDeleted != true)
                .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetDoctorChannelingAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var query = _context.AppointmentSchedules
                .Include(a => a.Location)
                .Include(a => a.Employee)
                .Include(a => a.AppointmentTreatments)
                    .ThenInclude(at => at.TreatmentType)
                .Where(a => a.DoctorSessionId != null)
                .Where(a => a.IsDeleted != true)
                .Where(a => a.ScheduleDate >= startDate.Date && a.ScheduleDate < endDate.Date.AddDays(1))
                .AsQueryable();
            if (companyId.HasValue)
                query = query.Where(a => a.CompanyId == companyId.Value);
            return await query.OrderBy(a => a.TokenNo).ToListAsync();
        }
    }
}
