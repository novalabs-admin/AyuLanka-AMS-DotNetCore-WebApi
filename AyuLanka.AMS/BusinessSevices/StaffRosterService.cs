using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class StaffRosterService : IStaffRosterService
    {
        private readonly IStaffRosterRepository _staffRosterRepository;
        private readonly IStaffRosterMasterRepository _staffRosterMasterRepository;
        private readonly IStaffLeaveRepository _staffLeaveRepository;

        public StaffRosterService(IStaffRosterRepository staffRosterRepository, 
            IStaffRosterMasterRepository staffRosterMasterRepository,
            IStaffLeaveRepository staffLeaveRepository)
        {
            _staffRosterRepository = staffRosterRepository;
            _staffRosterMasterRepository = staffRosterMasterRepository;
            _staffLeaveRepository = staffLeaveRepository;
        }


        public async Task<IEnumerable<StaffRoster>> GetDayOffsByDateAsync(DateTime date)
        {
            return await _staffRosterRepository.GetDayOffsByDateAsync(date);
        }
        
        public async Task<IEnumerable<StaffRoster>> GetWorkingShiftsByDate(DateTime date)
        {
            return await _staffRosterRepository.GetWorkingShiftsByDate(date);
        }

        public async Task<StaffRosterMaster> SaveRoster(StaffRosterRequestModel staffRosterRequestModel)
        {
            StaffRosterMaster rosterMaster;

            if (staffRosterRequestModel.rosterMaster.Id == 0)
            {
                var staffRosterMasterRequest = new StaffRosterMaster
                {
                    FromDate = staffRosterRequestModel.rosterMaster.FromDate,
                    Todate = staffRosterRequestModel.rosterMaster.Todate
                };

                rosterMaster = await _staffRosterMasterRepository.CreateRosterMasterAsync(staffRosterMasterRequest);
            }
            else
            {
                // Fetch the existing roster master
                rosterMaster = await _staffRosterMasterRepository.GetRosterMasterByIdAsync((int)staffRosterRequestModel.rosterMaster.Id);

                if (rosterMaster == null)
                {
                    throw new Exception("Roster master not found");
                }

                //Update the roster when approved
                if(staffRosterRequestModel.rosterMaster.IsApproved)
                {
                    rosterMaster.IsApproved = staffRosterRequestModel.rosterMaster.IsApproved;
                    rosterMaster.ApprovedBy = (int)staffRosterRequestModel.rosterMaster.ApprovedBy;
                    rosterMaster.ApprovedDate = DateTime.UtcNow;

                    await _staffRosterMasterRepository.UpdateRosterMasterAsync(rosterMaster);
                }

                // Delete existing staff rosters for the given roster master ID
                await _staffRosterRepository.DeleteStaffRostersByRosterMasterIdAsync(rosterMaster.Id);
            }

            List<StaffRoster> staffRosterRequests = new List<StaffRoster>();
            foreach (var roster in staffRosterRequestModel.staffRosters)
            {
                roster.RosterMasterId = rosterMaster.Id;
                var staffRosterRequest = new StaffRoster
                {
                    EmployeeId = roster.EmployeeId,
                    ShiftMasterId = roster.ShiftMasterId,
                    RosterMasterId = rosterMaster.Id,
                    IsDayOff = roster.IsDayOff,
                    DayOffDate = roster.DayOffDate,
                };
                staffRosterRequests.Add(staffRosterRequest);
            }
            await _staffRosterRepository.CreateStaffRostersAsync(staffRosterRequests);

            return rosterMaster;
        }

        public async Task<List<StaffRosterMaster>> GetRosterDateRangesAsync(int? companyId = null)
        {
            return await _staffRosterMasterRepository.GetRosterDateRangesAsync(companyId);
        }

        public async Task<IEnumerable<StaffRoster>> GetRosterByRosterMasterIdAsync(int rosterMasterId)
        {
            return await _staffRosterRepository.GetRosterByRosterMasterIdAsync(rosterMasterId);
        }

        public async Task<IEnumerable<StaffRoster>> GetApprovedRosterDatesAsync(int employeeId, int rosterMasterId)
        {
            return await _staffRosterRepository.GetApprovedRosterDatesAsync(employeeId, rosterMasterId);
        }
        
        public async Task<StaffRoster> GetEmployeeScheduleAsync(int employeeId, string scheduledate)
        {
            var employeeLeave = await _staffLeaveRepository.GetEmployeeScheduleAsync(employeeId, scheduledate);
            var employeeRoster = await _staffRosterRepository.GetEmployeeScheduleAsync(employeeId, scheduledate);

            if (employeeRoster != null)
            {
                if (employeeLeave == null)
                {
                    return employeeRoster;
                }
            }
            return null;
        }

        public async Task<StaffRosterMaster> UpdateRoster(int rosterMasterId, StaffRosterRequestModel staffRosterRequestModel)
        {
            // Retrieve the roster master first
            var rosterMaster = await _staffRosterMasterRepository.GetRosterMasterByIdAsync(rosterMasterId);
            if (rosterMaster == null)
            {
                throw new KeyNotFoundException("Roster master not found.");
            }


            // Process each roster detail
            foreach (var detail in staffRosterRequestModel.staffRosters)
            {
                var staffRoster = await _staffRosterRepository.GetRosterByIdAsync((int)detail.Id);
                if (staffRoster == null)
                {
                    continue;  // or handle the error
                }

                // Update individual roster fields
                staffRoster.ShiftMasterId = detail.ShiftMasterId;
                staffRoster.IsDayOff = detail.IsDayOff;
                staffRoster.DayOffDate = detail.DayOffDate;

                await _staffRosterRepository.UpdateStaffRosterAsync(staffRoster);
            }

            return rosterMaster;
        }
    }
}
