using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
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

        public AppointmentScheduleService(IAppointmentScheduleRepository appointmentScheduleRepository, 
            ILocationRepository locationRepository,
            IEmployeeRepository employeeRepository,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _appointmentScheduleRepository = appointmentScheduleRepository;
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync()
        {
            return await _appointmentScheduleRepository.GetAllAppointmentSchedulesAsync();
        }

        public async Task<AppointmentSchedule> GetAppointmentScheduleByIdAsync(int id)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByIdAsync(id);
        }
        
        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateAsync(DateTime date)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByDateAsync(date);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date)
        {
            return await _appointmentScheduleRepository.GetPrimeCareAppointmentScheduleByDateAsync(date);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date)
        {
            return await _appointmentScheduleRepository.GetTokensByDateAsync(date);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync()
        {
            return await _appointmentScheduleRepository.GetIssuedTokensByDateAsync();
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetDeletedAppoitmentByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetCompletedPreScheduledAppointmentAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetAllPreScheduledAppointmentAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetAppointmentScheduleByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetAllAppointmentScheduleByDateRangeAsync(startDate, endDate);
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

        public async Task<IEnumerable<object>> SearchPatientsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Enumerable.Empty<object>();

            var client = _httpClientFactory.CreateClient("CustomerApi");

            var response = await client.GetAsync(
                $"api/customer/search-customers?searchTerm={Uri.EscapeDataString(keyword)}"
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
                CustomerType = "patient"
            };

            var json = JsonSerializer.Serialize(payload);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "api/customer",
                content
            );

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<object>(responseJson);
        }


        public async Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appointmentScheduleRepository.GetPrimeCareAppointmentScheduleByDateRangeAsync(startDate, endDate);
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

                if (appointmentScheduleRequestModel.TokenNo.HasValue)
                {
                    var tokenExists = await _appointmentScheduleRepository.IsTokenExistsAsync(
                        appointmentScheduleRequestModel.TokenNo.Value,
                        appointmentScheduleRequestModel.ScheduleDate,
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
                    if (appointmentScheduleRequestModel.MainTreatmentArea.HasValue && appointmentScheduleRequestModel.MainTreatmentArea == 1)
                    {
                        var location = await _locationRepository.GetTreatmentLocationByNameAsync("Doctor Room Waiting");
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
                            var location = await _locationRepository.GetTreatmentLocationByNameAsync("Elite Care Waiting");
                            location_id = location.Id;
                        }
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
                        LocationId = location_id,
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
                    };

                    appointmentResult = await _appointmentScheduleRepository.AddAppointmentScheduleAsync(newAppointment);
                }
                else
                {
                    var existingAppointment = await _appointmentScheduleRepository.GetAppointmentScheduleByIdAsync(appointmentScheduleRequestModel.Id);

                    int location_id;
                    if (appointmentScheduleRequestModel.MainTreatmentArea.HasValue && appointmentScheduleRequestModel.MainTreatmentArea == 1)
                    {
                        if (appointmentScheduleRequestModel.LocationId.HasValue)
                        {
                            location_id = (int)appointmentScheduleRequestModel.LocationId;
                        }
                        else
                        {
                            var location = await _locationRepository.GetTreatmentLocationByNameAsync("Doctor Room Waiting");
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
                            var location = await _locationRepository.GetTreatmentLocationByNameAsync("Elite Care Waiting");
                            location_id = location.Id;
                        }
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
                    existingAppointment.LocationId = location_id;
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
                            var maxChitNo = await _appointmentScheduleRepository.GetMaxChitNoAsync(appointmentScheduleRequestModel.ScheduleDate);
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
                        : await _locationRepository.GetLocationByLocationIdAsync((int)appointmentResult.LocationId);

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

                            await InsertOrUpdateDailyTokenAsync(appointmentResult, locationSub, locationTypeName, customerName);
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

        public async Task<bool> IsTokenExistsAsync(int tokenNo, DateTime scheduleDate, int? excludeAppointmentId = null)
        {
            return await _appointmentScheduleRepository.IsTokenExistsAsync(tokenNo, scheduleDate, excludeAppointmentId);
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
    }
}
