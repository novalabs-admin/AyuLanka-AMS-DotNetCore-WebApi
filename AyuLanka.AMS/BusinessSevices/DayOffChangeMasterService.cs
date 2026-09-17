using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class DayOffChangeMasterService : IDayOffChangeMasterService
    {
        private readonly IDayOffChangeMasterRepository _dayOffChangeMasterRepository;
        private readonly IDayOffChangeDetailRepository _dayOffChangeDetailRepository;
        private readonly IStaffRosterRepository _staffRosterRepository;

        public DayOffChangeMasterService(IDayOffChangeMasterRepository dayOffChangeMasterRepository, 
            IDayOffChangeDetailRepository dayOffChangeDetailRepository, IStaffRosterRepository staffRosterRepository)
        {
            _dayOffChangeMasterRepository = dayOffChangeMasterRepository;
            _dayOffChangeDetailRepository = dayOffChangeDetailRepository;
            _staffRosterRepository = staffRosterRepository;
        }

        public async Task<IEnumerable<DayOffChangeDetail>> GetAllDayOffChangeDetailsAsync(int? companyId = null)
        {
            return await _dayOffChangeDetailRepository.GetAllDayOffChangeDetailsAsync(companyId);
        }

        public async Task<DayOffChangeMaster> GetDayOffChangeMasterByIdAsync(int id)
        {
            return await _dayOffChangeMasterRepository.GetDayOffChangeMasterByIdAsync(id);
        }

        public async Task AddDayOffChangesAsync(DayOffChangeRequestModel dayOffChangeRequestModel)
        {
            DayOffChangeMaster dayOffChangeMaster = null;
            bool masterCreated = false;
            foreach (var detail in dayOffChangeRequestModel.dayOffChangeDetails)
            {
                if (detail.IsApproved == false)
                {
                    var dayOffChangeDetailRecord = _dayOffChangeDetailRepository
                    .GetDayOffChangeDetailByDatePreAndRosterAsync(detail.DayOffPre, detail.StaffRosterId).Result;

                    if (dayOffChangeDetailRecord == null)
                    {
                        if (!masterCreated)
                        {
                            var dayOffChangeMasterRequest = new DayOffChangeMaster
                            {
                                EmployeeId = dayOffChangeRequestModel.dayOffChangeMaster.EmployeeId,
                                DayOffChangeReasonId = dayOffChangeRequestModel.dayOffChangeMaster.DayOffChangeReasonId,
                                StaffRosterMasterId = dayOffChangeRequestModel.dayOffChangeMaster.StaffRosterMasterId,
                            };

                            dayOffChangeMaster = await _dayOffChangeMasterRepository.AddDayOffChangeMasterAsync(dayOffChangeMasterRequest);
                            masterCreated = true; // Set the flag as true after creating the master record
                        }

                        detail.DayOffChangeMasterId = dayOffChangeMaster.Id;
                        var dayOffChangeDetail = new DayOffChangeDetail
                        {
                            DayOffChangeMasterId = detail.DayOffChangeMasterId,
                            StaffRosterId = detail.StaffRosterId,
                            DayOffPost = detail.DayOffPost,
                            DayOffPre = detail.DayOffPre,
                            ExchangeWithPost = detail.ExchangeWithPost,
                            ExchangeWithPre = detail.ExchangeWithPre,
                        };

                        await _dayOffChangeDetailRepository.CreateDayOffChangeDetailAsync(dayOffChangeDetail);

                    }
                    else
                    {
                        if (dayOffChangeDetailRecord.IsApproved != true)
                        {
                            dayOffChangeDetailRecord.DayOffPost = detail.DayOffPost;
                            dayOffChangeDetailRecord.DayOffPre = detail.DayOffPre;
                            dayOffChangeDetailRecord.ExchangeWithPost = detail.ExchangeWithPost;
                            dayOffChangeDetailRecord.ExchangeWithPre = detail.ExchangeWithPre;

                            await _dayOffChangeDetailRepository.UpdateDayOffChangeDetailAsync(dayOffChangeDetailRecord);
                        } else
                        {
                            dayOffChangeDetailRecord.DayOffPost = detail.DayOffPost;
                            dayOffChangeDetailRecord.DayOffPre = detail.DayOffPre;
                            dayOffChangeDetailRecord.ExchangeWithPost = detail.ExchangeWithPost;
                            dayOffChangeDetailRecord.ExchangeWithPre = detail.ExchangeWithPre;
                            dayOffChangeDetailRecord.IsApproved = false;
                            dayOffChangeDetailRecord.ApprovedBy = null;
                            dayOffChangeDetailRecord.ApprovedDate = null;

                            await _dayOffChangeDetailRepository.UpdateDayOffChangeDetailAsync(dayOffChangeDetailRecord);
                        }
                    }
                } else
                {

                    var dayOffChangeDetailRecord = _dayOffChangeDetailRepository
                    .GetDayOffChangeDetailByIdAsync(detail.Id).Result;

                    var staffRoster = await _staffRosterRepository.GetRosterByIdAsync(dayOffChangeDetailRecord.StaffRosterId);

                    // Update individual roster fields
                    staffRoster.DayOffDate = dayOffChangeDetailRecord.DayOffPost;

                    await _staffRosterRepository.UpdateStaffRosterAsync(staffRoster);

                    dayOffChangeDetailRecord.IsApproved = detail.IsApproved;
                    dayOffChangeDetailRecord.ApprovedBy = detail.ApprovedBy;
                    dayOffChangeDetailRecord.ApprovedDate = DateTime.UtcNow;

                    await _dayOffChangeDetailRepository.UpdateDayOffChangeDetailAsync(dayOffChangeDetailRecord);
                }
            }
        }


        public async Task DeleteDayOffChangeMasterAsync(int id)
        {
            await _dayOffChangeMasterRepository.DeleteDayOffChangeMasterAsync(id);
        }

        public Task<IEnumerable<DayOffChangeMaster>> AddDayOffChangeMastersAsync(IEnumerable<DayOffChangeMasterRequestModel> requestModels)
        {
            throw new NotImplementedException();
        }

        public Task<DayOffChangeMaster> UpdateDayOffChangeMasterAsync(int id, DayOffChangeMasterRequestModel DayOffChangeMasterRequestModel)
        {
            throw new NotImplementedException();
        }
    }
}
