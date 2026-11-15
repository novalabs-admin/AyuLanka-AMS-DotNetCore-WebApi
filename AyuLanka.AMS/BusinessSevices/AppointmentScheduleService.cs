using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Transactions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AyuLanka.AMS.BusinessSevices
{
    public class AppointmentScheduleService : IAppointmentScheduleService
    {
        private readonly IAppointmentScheduleRepository _appointmentScheduleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AppointmentScheduleService> _logger;

        public AppointmentScheduleService(IAppointmentScheduleRepository appointmentScheduleRepository, 
            ILocationRepository locationRepository,
            IEmployeeRepository employeeRepository,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<AppointmentScheduleService> logger)
        {
            _appointmentScheduleRepository = appointmentScheduleRepository;
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync(int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetAllAppointmentSchedulesAsync(companyId);
        }

        public async Task<AppointmentSchedule> GetAppointmentScheduleByIdAsync(int id)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByIdAsync(id);
        }
        
        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateAsync(DateTime date, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByDateAsync(date, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetPrimeCareAppointmentScheduleByDateAsync(date, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetTokensByDateAsync(date, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAndCompanyCodeAsync(DateTime date, string? companyCode = null)
        {
            return await _appointmentScheduleRepository.GetTokensByDateAndCompanyCodeAsync(date, companyCode);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync(int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetIssuedTokensByDateAsync(companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetDeletedAppoitmentByDateRangeAsync(startDate, endDate, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetCompletedPreScheduledAppointmentAsync(startDate, endDate, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetAllPreScheduledAppointmentAsync(startDate, endDate, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetAllAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
        }
        public async Task<IEnumerable<DashboardDateChartDto?>> GetAllDashboardChartsDatabyDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetAllDashboardChartsDatabyDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<DashboardSummaryDto>> GetDashboardSummaryByDateRangeAsync(DateTime startDate, DateTime endDate, string category)
        {
            return await _appointmentScheduleRepository.GetDashboardSummaryByDateRangeAsync(startDate, endDate, category);
        }

        public async Task<IEnumerable<DashboardDetailsDto>> GetDashboardDetailsByDateAsync(DateTime date, string category, string type)
        {
            return await _appointmentScheduleRepository.GetDashboardDetailsByDateAsync(date, category, type);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetCustomerDetailsByIdAsync(int customerId)
        {
            return await _appointmentScheduleRepository.GetCustomerDetailsByIdAsync(customerId);
        }

        public async Task<IEnumerable<object>> SearchPatientsAsync(string keyword, string? companyCode = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Enumerable.Empty<object>();

            var client = _httpClientFactory.CreateClient("CustomerApi");

            var response = await client.GetAsync(
                $"api/userManagement/customer/search-customers?searchTerm={Uri.EscapeDataString(keyword)}"
            );

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<object>();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<CustomerSearchApiResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse?.Data?.Result
                ?.Select(c => new
                {
                    Id = c.CustomerId,
                    CustomerName = c.CustomerName,
                    ContactNo = c.Phone
                })
                ?? Enumerable.Empty<object>();
        }

        public async Task<object> CreateCustomerAsync(CreateCustomerRequest request)
        {
            var client = _httpClientFactory.CreateClient("CustomerApi");

            var payload = new
            {
                customerName = request.CustomerName,
                phone = request.Phone,
                CustomerType = "patient",
                Remark = request.Remark ?? null,
                CityId = request.CityId ?? null,
                CountryId = request.CountryId ?? null
            };

            var json = JsonSerializer.Serialize(payload);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "api/userManagement/customer",
                content
            );

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<object>(responseJson);
        }


        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetPrimeCareAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetByDoctorSessionIdAsync(int sessionId, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetByDoctorSessionIdAsync(sessionId, companyId);
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetDoctorChannelingAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetDoctorChannelingAppointmentsByDateRangeAsync(startDate, endDate, companyId);
        }

        //public async Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentScheduleRequestModel appointmentScheduleRequestModel)
        //{
        //    if (appointmentScheduleRequestModel.Id == 0)
        //    {
        //        int location_id;
        //        if (appointmentScheduleRequestModel.MainTreatmentArea.HasValue && appointmentScheduleRequestModel.MainTreatmentArea == 1)
        //        {
        //            var location = await _locationRepository.GetPrimeCareLocationByNameAsync("Doctor Room Waiting");
        //            location_id = location.Id;
        //        }else
        //        {
        //            location_id = (int)appointmentScheduleRequestModel.LocationId;
        //        }

        //        var AppointmentSchedule = new AppointmentSchedule()
        //        {
        //            CustomerName = appointmentScheduleRequestModel.CustomerName,
        //            ContactNo = appointmentScheduleRequestModel.ContactNo,
        //            EmployeeId = appointmentScheduleRequestModel.EmployeeId != 0 ? appointmentScheduleRequestModel.EmployeeId : null,
        //            SecondaryEmployeeId = appointmentScheduleRequestModel.SecondaryEmployeeId != 0 ? appointmentScheduleRequestModel.SecondaryEmployeeId : null,
        //            DoctorEmployeeId = appointmentScheduleRequestModel.DoctorEmployeeId != 0 ? appointmentScheduleRequestModel.DoctorEmployeeId : null,
        //            ScheduleDate = appointmentScheduleRequestModel.ScheduleDate,
        //            LocationId = location_id,
        //            FromTime = appointmentScheduleRequestModel.FromTime,
        //            ToTime = appointmentScheduleRequestModel.ToTime,
        //            ActualFromTime = appointmentScheduleRequestModel.ActualFromTime,
        //            ActualToTime = appointmentScheduleRequestModel.ActualToTime,
        //            ActualFromTimeSecond = appointmentScheduleRequestModel.ActualFromTimeSecond,
        //            ActualToTimeSecond = appointmentScheduleRequestModel.ActualToTimeSecond,
        //            EnteredBy = appointmentScheduleRequestModel.EnteredBy,
        //            Remarks = appointmentScheduleRequestModel.Remarks,
        //            EnteredDate = DateTime.Now,
        //            TokenNo = appointmentScheduleRequestModel.TokenNo,
        //            TokenIssueTime = DateTime.Now,
        //            MainTreatmentArea = appointmentScheduleRequestModel?.MainTreatmentArea,
        //        };


        //        return await _appointmentScheduleRepository.AddAppointmentScheduleAsync(AppointmentSchedule);
        //    } else
        //    {
        //        var existingAppoinment = await _appointmentScheduleRepository.GetAppointmentScheduleByIdAsync(appointmentScheduleRequestModel.Id);

        //        // Get max chitNo from repository
        //        var maxChitNo = await _appointmentScheduleRepository.GetMaxChitNoAsync(appointmentScheduleRequestModel.ScheduleDate);

        //        existingAppoinment.CustomerName = appointmentScheduleRequestModel.CustomerName;
        //        existingAppoinment.ContactNo = appointmentScheduleRequestModel.ContactNo;
        //        existingAppoinment.EmployeeId = appointmentScheduleRequestModel.EmployeeId != 0 ? appointmentScheduleRequestModel.EmployeeId : null;
        //        existingAppoinment.SecondaryEmployeeId = appointmentScheduleRequestModel.SecondaryEmployeeId != 0 ? appointmentScheduleRequestModel.SecondaryEmployeeId : null;
        //        existingAppoinment.DoctorEmployeeId = appointmentScheduleRequestModel.DoctorEmployeeId != 0 ? appointmentScheduleRequestModel.DoctorEmployeeId : null;
        //        existingAppoinment.ScheduleDate = appointmentScheduleRequestModel.ScheduleDate;
        //        existingAppoinment.LocationId = appointmentScheduleRequestModel.LocationId;
        //        existingAppoinment.FromTime = appointmentScheduleRequestModel.FromTime;
        //        existingAppoinment.ToTime = appointmentScheduleRequestModel.ToTime;
        //        existingAppoinment.ActualFromTime = appointmentScheduleRequestModel.ActualFromTime;
        //        existingAppoinment.ActualToTime = appointmentScheduleRequestModel.ActualToTime;
        //        existingAppoinment.ActualFromTimeSecond = appointmentScheduleRequestModel.ActualFromTimeSecond;
        //        existingAppoinment.ActualToTimeSecond = appointmentScheduleRequestModel.ActualToTimeSecond;
        //        existingAppoinment.UpdatedBy = appointmentScheduleRequestModel.EnteredBy;
        //        existingAppoinment.UpdatedDate = DateTime.Now;
        //        existingAppoinment.TokenNo = appointmentScheduleRequestModel.TokenNo;
        //        existingAppoinment.MainTreatmentArea = appointmentScheduleRequestModel.MainTreatmentArea;

        //        if (appointmentScheduleRequestModel.IsTokenIssued)
        //        {
        //            existingAppoinment.ChitNo = maxChitNo + 1;
        //        }

        //        if (existingAppoinment.TokenNo == null && appointmentScheduleRequestModel.TokenNo != null)
        //        {
        //            existingAppoinment.TokenIssueTime = DateTime.Now;
        //        }
        //        existingAppoinment.Remarks = appointmentScheduleRequestModel.Remarks;

        //        return await _appointmentScheduleRepository.UpdateAppointmentScheduleAsync(existingAppoinment);
        //    }
        //}


        public async Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentScheduleRequestModel appointmentScheduleRequestModel)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AppointmentSchedule appointmentResult = null;

                // Next appointments (follow-ups) must always carry a token; the UI enforces it, this is the last line of defence.
                if (appointmentScheduleRequestModel.Id == 0 &&
                    appointmentScheduleRequestModel.ParentAppointmentScheduleId != null &&
                    !appointmentScheduleRequestModel.TokenNo.HasValue)
                {
                    throw new InvalidOperationException("A token number is required to create a next appointment.");
                }

                if (appointmentScheduleRequestModel.TokenNo.HasValue)
                {
                    var tokenExists = await _appointmentScheduleRepository.IsTokenExistsAsync(
                        appointmentScheduleRequestModel.TokenNo.Value,
                        appointmentScheduleRequestModel.ScheduleDate,
                        appointmentScheduleRequestModel.CompanyId,
                        appointmentScheduleRequestModel.DoctorSessionId,
                        appointmentScheduleRequestModel.Id == 0 ? null : appointmentScheduleRequestModel.Id
                    );

                    if (tokenExists)
                    {
                        throw new InvalidOperationException(
                            $"Token number {appointmentScheduleRequestModel.TokenNo} already exists for the selected date."
                        );
                    }
                }

                if (appointmentScheduleRequestModel.Id == 0)
                {
                    int location_id;
                    if (appointmentScheduleRequestModel.MainTreatmentArea.HasValue)
                    {
                        if (appointmentScheduleRequestModel.MainTreatmentArea == 1)
                        {
                            var location = await _locationRepository.GetTreatmentLocationByNameAsync("Doctor Room Waiting", appointmentScheduleRequestModel.CompanyId);
                            if (location == null)
                                throw new InvalidOperationException($"Default location 'Doctor Room Waiting' not found for company {appointmentScheduleRequestModel.CompanyId}. Please ensure this location exists.");
                            location_id = location.Id;
                        }
                        else
                        {
                            if (appointmentScheduleRequestModel.LocationId.HasValue)
                            {
                                location_id = (int)appointmentScheduleRequestModel.LocationId;
                            }
                            else
                            {
                                var location = await _locationRepository.GetTreatmentLocationByNameAsync("Elite Care Waiting", appointmentScheduleRequestModel.CompanyId);
                                if (location == null)
                                    throw new InvalidOperationException($"Default location 'Elite Care Waiting' not found for company {appointmentScheduleRequestModel.CompanyId}. Please ensure this location exists.");
                                location_id = location.Id;
                            }
                        }
                    } else
                    {
                        location_id = appointmentScheduleRequestModel.LocationId.HasValue
                            ? appointmentScheduleRequestModel.LocationId.Value
                            : 0;
                    }

                        var newAppointment = new AppointmentSchedule()
                        {
                            CustomerId = appointmentScheduleRequestModel.CustomerId,
                            CustomerName = appointmentScheduleRequestModel.CustomerName,
                            ContactNo = appointmentScheduleRequestModel.ContactNo,
                            EmployeeId = appointmentScheduleRequestModel.EmployeeId != 0 ? appointmentScheduleRequestModel.EmployeeId : null,
                            SecondaryEmployeeId = appointmentScheduleRequestModel.SecondaryEmployeeId != 0 ? appointmentScheduleRequestModel.SecondaryEmployeeId : null,
                            DoctorEmployeeId = appointmentScheduleRequestModel.DoctorEmployeeId != 0 ? appointmentScheduleRequestModel.DoctorEmployeeId : null,
                            ScheduleDate = appointmentScheduleRequestModel.ScheduleDate,
                            LocationId = location_id == 0 ? null : location_id,
                            FromTime = appointmentScheduleRequestModel.FromTime,
                            ToTime = appointmentScheduleRequestModel.ToTime,
                            ActualFromTime = appointmentScheduleRequestModel.ActualFromTime,
                            ActualToTime = appointmentScheduleRequestModel.ActualToTime,
                            ActualFromTimeSecond = appointmentScheduleRequestModel.ActualFromTimeSecond,
                            ActualToTimeSecond = appointmentScheduleRequestModel.ActualToTimeSecond,
                            EnteredBy = appointmentScheduleRequestModel.EnteredBy,
                            Remarks = appointmentScheduleRequestModel.Remarks,
                            EnteredDate = DateTime.Now,
                            TokenNo = appointmentScheduleRequestModel.TokenNo,
                            TokenIssueTime = DateTime.Now,
                            MainTreatmentArea = appointmentScheduleRequestModel?.MainTreatmentArea,
                            ParentAppointmentScheduleId = appointmentScheduleRequestModel.ParentAppointmentScheduleId != null ? appointmentScheduleRequestModel.ParentAppointmentScheduleId : null,
                            IsNeededToFollowUp = appointmentScheduleRequestModel.IsNeededToFollowUp != null ? appointmentScheduleRequestModel.IsNeededToFollowUp : false,
                            CompanyId = appointmentScheduleRequestModel.CompanyId,
                            DoctorSessionId = appointmentScheduleRequestModel.DoctorSessionId,
                        };

                    appointmentResult = await _appointmentScheduleRepository.AddAppointmentScheduleAsync(newAppointment);
                }
                else
                {
                    var existingAppointment = await _appointmentScheduleRepository.GetAppointmentScheduleByIdAsync(appointmentScheduleRequestModel.Id);

                    int location_id;
                    if (appointmentScheduleRequestModel.MainTreatmentArea.HasValue)
                    {
                        if (appointmentScheduleRequestModel.MainTreatmentArea == 1)
                        {
                            if (appointmentScheduleRequestModel.LocationId.HasValue)
                            {
                                location_id = (int)appointmentScheduleRequestModel.LocationId;
                            }
                            else
                            {
                                var location = await _locationRepository.GetTreatmentLocationByNameAsync("Doctor Room Waiting", appointmentScheduleRequestModel.CompanyId);
                                if (location == null)
                                    throw new InvalidOperationException($"Default location 'Doctor Room Waiting' not found for company {appointmentScheduleRequestModel.CompanyId}. Please ensure this location exists.");
                                location_id = location.Id;
                            }
                        }
                        else
                        {
                            if (appointmentScheduleRequestModel.LocationId.HasValue)
                            {
                                location_id = (int)appointmentScheduleRequestModel.LocationId;
                            }
                            else
                            {
                                var location = await _locationRepository.GetTreatmentLocationByNameAsync("Elite Care Waiting", appointmentScheduleRequestModel.CompanyId);
                                if (location == null)
                                    throw new InvalidOperationException($"Default location 'Elite Care Waiting' not found for company {appointmentScheduleRequestModel.CompanyId}. Please ensure this location exists.");
                                location_id = location.Id;
                            }
                        }
                    }
                    else
                    {
                        // No MainTreatmentArea sent (e.g. drag-and-drop from scheduler) —
                        // use the LocationId directly if provided, otherwise leave unchanged.
                        location_id = appointmentScheduleRequestModel.LocationId.HasValue
                            ? appointmentScheduleRequestModel.LocationId.Value
                            : (existingAppointment.LocationId ?? 0);
                    }


                        existingAppointment.CustomerId = appointmentScheduleRequestModel.CustomerId != 0 ?
                                                        appointmentScheduleRequestModel.CustomerId : existingAppointment.CustomerId;
                    existingAppointment.CustomerName = appointmentScheduleRequestModel.CustomerName;
                    existingAppointment.ContactNo = appointmentScheduleRequestModel.ContactNo;
                    existingAppointment.EmployeeId = appointmentScheduleRequestModel.EmployeeId != 0
                                                    ? appointmentScheduleRequestModel.EmployeeId : null;
                    existingAppointment.SecondaryEmployeeId = appointmentScheduleRequestModel.SecondaryEmployeeId != 0
                                                    ? appointmentScheduleRequestModel.SecondaryEmployeeId : null;
                    existingAppointment.DoctorEmployeeId = appointmentScheduleRequestModel.DoctorEmployeeId != 0
                                                    ? appointmentScheduleRequestModel.DoctorEmployeeId : null;
                    existingAppointment.ScheduleDate = appointmentScheduleRequestModel.ScheduleDate;
                    existingAppointment.LocationId = location_id == 0 ? null : location_id;
                    existingAppointment.FromTime = appointmentScheduleRequestModel.FromTime;
                    existingAppointment.ToTime = appointmentScheduleRequestModel.ToTime;
                    existingAppointment.ActualFromTime = appointmentScheduleRequestModel.ActualFromTime;
                    existingAppointment.ActualToTime = appointmentScheduleRequestModel.ActualToTime;
                    existingAppointment.ActualFromTimeSecond = appointmentScheduleRequestModel.ActualFromTimeSecond;
                    existingAppointment.ActualToTimeSecond = appointmentScheduleRequestModel.ActualToTimeSecond;
                    existingAppointment.UpdatedBy = appointmentScheduleRequestModel.EnteredBy;
                    existingAppointment.UpdatedDate = DateTime.Now;
                    existingAppointment.TokenNo = appointmentScheduleRequestModel.TokenNo != null
                                                    ? appointmentScheduleRequestModel.TokenNo : existingAppointment.TokenNo;
                    existingAppointment.MainTreatmentArea = appointmentScheduleRequestModel.MainTreatmentArea != null
                                                    ? appointmentScheduleRequestModel.MainTreatmentArea : existingAppointment.MainTreatmentArea;
                    existingAppointment.Remarks = appointmentScheduleRequestModel.Remarks;
                    existingAppointment.IsPatientContacted = appointmentScheduleRequestModel.IsPatientContacted != null
                                                    ? appointmentScheduleRequestModel.IsPatientContacted : existingAppointment.IsPatientContacted;

                    // Only assign a new ChitNo if token is being issued AND one hasn't been assigned yet.
                    // Guarding here prevents a duplicate/concurrent request from reading a higher maxChitNo
                    // and overwriting the already-assigned ChitNo, which burns a number and creates gaps.
                    if (appointmentScheduleRequestModel.IsTokenIssued)
                    {
                        if (existingAppointment.ChitNo == null)
                        {
                            var maxChitNo = await _appointmentScheduleRepository.GetMaxChitNoAsync(appointmentScheduleRequestModel.ScheduleDate, appointmentScheduleRequestModel.CompanyId);
                            existingAppointment.ChitNo = maxChitNo + 1;
                        }
                    }

                    if (existingAppointment.TokenNo == null && appointmentScheduleRequestModel.TokenNo != null)
                    {
                        existingAppointment.TokenIssueTime = DateTime.Now;
                    }

                    appointmentResult = await _appointmentScheduleRepository.UpdateAppointmentScheduleAsync(existingAppointment);

                    var enterdByUser = await _employeeRepository.GetEmployeeByIdAsync(appointmentResult.EnteredBy);
                    appointmentResult.EnteredByEmployee = enterdByUser;

                    var locationSub = appointmentResult.Location != null ? appointmentResult.Location
                        : (appointmentResult.LocationId.HasValue
                            ? await _locationRepository.GetLocationByLocationIdAsync(appointmentResult.LocationId.Value)
                            : null);

                    if (locationSub != null)
                    {
                        var locationTypeName = locationSub.LocationTypeId == 1 ? "Prime Care Wing" : "Elite Care Wing";
                        var customerName = appointmentResult.CustomerName;

                        // 🔹 Insert into other DB if TokenNo newly issued
                        if (appointmentScheduleRequestModel.IsTokenIssued)
                        {
                            var timeGap = DateTime.Now - appointmentResult.TokenIssueTime;

                            if (timeGap.TotalMinutes >= 30)
                            {
                                customerName = $"{appointmentResult.CustomerName} - Appt: {appointmentResult.FromTime:hh\\:mm\\:ss}";
                            }

                            // Only sync to the daily token display board for the Wattala branch (CompanyId = 1)
                            if (appointmentResult.CompanyId == 1)
                            {
                                await InsertOrUpdateDailyTokenAsync(appointmentResult, locationSub, locationTypeName, customerName);
                            }
                        }
                    }
                }

                scope.Complete();
                return appointmentResult;
            }
        }


        public async Task<AppointmentSchedule> UpdateAppointmentScheduleAsync(AppointmentSchedule AppointmentSchedule)
        {
            return await _appointmentScheduleRepository.UpdateAppointmentScheduleAsync(AppointmentSchedule);
        }

        public async Task DeleteAppointmentScheduleAsync(int id, int deletedByUserId, string remark)
        {
            await _appointmentScheduleRepository.DeleteAppointmentScheduleAsync(id, deletedByUserId, remark);
        }

        public async Task<bool> IsTokenExistsAsync(int tokenNo, DateTime scheduleDate, int companyId, int? doctorSessionId, int? excludeAppointmentId = null)
        {
            return await _appointmentScheduleRepository.IsTokenExistsAsync(tokenNo, scheduleDate, companyId, doctorSessionId, excludeAppointmentId);
        }


        private async Task InsertOrUpdateDailyTokenAsync(AppointmentSchedule appointment, Location location, string locationTypeName, string customerName)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_InsertDailyToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AppointmentScheduleId", appointment.Id);
                        command.Parameters.AddWithValue("@TokenDate", appointment.ScheduleDate);
                        command.Parameters.AddWithValue("@TokenNo", appointment.TokenNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TreatmentLocationName", locationTypeName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubLocationName", location.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PatientName", customerName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNo", appointment.ContactNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EnteredBy", appointment.EnteredByEmployee.Username ?? (object)DBNull.Value);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // log or handle error without interrupting main save
                Console.WriteLine($"Error inserting Daily Token: {ex.Message}");
                throw;
            }
        }

        public async Task<List<EmployeeForAppointmentDto>> GetEmployeesByAppointmentIdsAsync(List<int> appointmentIds)
        {
            return await _appointmentScheduleRepository.GetEmployeesByAppointmentIdsAsync(appointmentIds);
        }

        public async Task<IEnumerable<AppointmentReportDto>> GetAppointmentReportAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            var rows = (await _appointmentScheduleRepository.GetAppointmentReportAsync(startDate, endDate, companyId)).ToList();

            // One lookup for every distinct item code in the report, then each appointment gets its distinct item modes
            var codes = rows.SelectMany(r => r.TreatmentShortCodes).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var modesByCode = await GetItemModesByItemCodesAsync(codes);

            foreach (var row in rows)
            {
                row.ItemModes = row.TreatmentShortCodes
                    .Where(modesByCode.ContainsKey)
                    .SelectMany(code => modesByCode[code])
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(name => name)
                    .ToList();
            }

            return rows;
        }

        // Item modes per item code from the user-management API. The report is still returned (without item
        // modes) if that API is unavailable, so a failure here is logged rather than thrown.
        private async Task<Dictionary<string, List<string>>> GetItemModesByItemCodesAsync(List<string> itemCodes)
        {
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (itemCodes.Count == 0) return result;

            try
            {
                var client = _httpClientFactory.CreateClient("CustomerApi");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                // Do not let an unreachable user-management API hold the whole report (the default wait is 100 s)
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                foreach (var chunk in itemCodes.Chunk(200))
                {
                    using var response = await client.PostAsJsonAsync(
                        "api/userManagement/itemMode/GetItemModesByItemCodes", new { itemCodes = chunk }, cts.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Item mode lookup failed with HTTP {Status}", (int)response.StatusCode);
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync(cts.Token);
                    var items = JsonSerializer.Deserialize<ApiResponse<ItemModeLookupDto>>(json, options)?.Data?.Result;
                    if (items == null) continue;

                    foreach (var item in items)
                        result[item.ItemCode] = item.ItemModes.Select(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Item mode lookup failed; the Appointment Report is returned without item modes");
            }

            return result;
        }

        public async Task<IEnumerable<AppointmentReportDto>> GetUnlinkedAppointmentsAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _appointmentScheduleRepository.GetUnlinkedAppointmentsAsync(startDate, endDate, companyId);
        }
    }
}
