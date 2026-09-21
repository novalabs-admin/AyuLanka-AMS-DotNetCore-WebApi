using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class ShiftChangeMasterService : IShiftChangeMasterService
    {
        private readonly IShiftChangeMasterRepository _ShiftChangeMasterRepository;
        private readonly IShiftChangeDetailRepository _ShiftChangeDetailRepository;
        private readonly IStaffRosterRepository _staffRosterRepository;

        public ShiftChangeMasterService(IShiftChangeMasterRepository ShiftChangeMasterRepository,
            IShiftChangeDetailRepository ShiftChangeDetailRepository, IStaffRosterRepository staffRosterRepository)
        {
            _ShiftChangeMasterRepository = ShiftChangeMasterRepository;
            _ShiftChangeDetailRepository = ShiftChangeDetailRepository;
            _staffRosterRepository = staffRosterRepository;
        }

        public async Task<IEnumerable<ShiftChangeDetail>> GetAllShiftChangeDetailsAsync(int? companyId = null)
        {
            return await _ShiftChangeDetailRepository.GetAllShiftChangeDetailsAsync(companyId);
        }

        public async Task<ShiftChangeMaster> GetShiftChangeMasterByIdAsync(int id)
        {
            return await _ShiftChangeMasterRepository.GetShiftChangeMasterByIdAsync(id);
        }

        public async Task AddShiftChangesAsync(ShiftChangeRequestModel ShiftChangeRequestModel)
        {
            ShiftChangeMaster ShiftChangeMaster = null;
            bool masterCreated = false;
            foreach (var detail in ShiftChangeRequestModel.shiftChangeDetails)
            {
                if (detail.IsApproved == false)
                {
                    var ShiftChangeDetailRecord = _ShiftChangeDetailRepository
                    .GetShiftChangeDetailByDatePreAndRosterAsync(detail.ShiftPre, detail.StaffRosterId).Result;

                    if (ShiftChangeDetailRecord == null)
                    {
                        if (!masterCreated)
                        {
                            var ShiftChangeMasterRequest = new ShiftChangeMaster
                            {
                                EmployeeId = ShiftChangeRequestModel.shiftChangeMasterRequest.EmployeeId,
                                ShiftChangeReasonId = ShiftChangeRequestModel.shiftChangeMasterRequest.ShiftChangeReasonId,
                                StaffRosterMasterId = ShiftChangeRequestModel.shiftChangeMasterRequest.StaffRosterMasterId,
                            };

                            ShiftChangeMaster = await _ShiftChangeMasterRepository.AddShiftChangeMasterAsync(ShiftChangeMasterRequest);
                            masterCreated = true; // Set the flag as true after creating the master record
                        }

                        detail.ShiftChangeMasterId = ShiftChangeMaster.Id;
                        var ShiftChangeDetail = new ShiftChangeDetail
                        {
                            ShiftChangeMasterId = detail.ShiftChangeMasterId,
                            StaffRosterId = detail.StaffRosterId,
                            ShiftPost = detail.ShiftPost,
                            ShiftPre = detail.ShiftPre,
                            ExchangeWithPost = detail.ExchangeWithPost,
                            ExchangeWithPre = detail.ExchangeWithPre,
                        };

                        await _ShiftChangeDetailRepository.CreateShiftChangeDetailAsync(ShiftChangeDetail);

                    }
                    else
                    {
                        if (ShiftChangeDetailRecord.IsApproved != true)
                        {
                            ShiftChangeDetailRecord.ShiftPost = detail.ShiftPost;
                            ShiftChangeDetailRecord.ShiftPre = detail.ShiftPre;
                            ShiftChangeDetailRecord.ExchangeWithPost = detail.ExchangeWithPost;
                            ShiftChangeDetailRecord.ExchangeWithPre = detail.ExchangeWithPre;

                            await _ShiftChangeDetailRepository.UpdateShiftChangeDetailAsync(ShiftChangeDetailRecord);
                        }
                    }
                }
                else
                {

                    var ShiftChangeDetailRecord = _ShiftChangeDetailRepository
                    .GetShiftChangeDetailByIdAsync(detail.Id).Result;

                    var staffRoster = await _staffRosterRepository.GetRosterByIdAsync(ShiftChangeDetailRecord.StaffRosterId);

                    // Update individual roster fields
                    staffRoster.ShiftMasterId = ShiftChangeDetailRecord.ShiftPost;

                    await _staffRosterRepository.UpdateStaffRosterAsync(staffRoster);

                    ShiftChangeDetailRecord.IsApproved = detail.IsApproved;
                    ShiftChangeDetailRecord.ApprovedBy = detail.ApprovedBy;
                    ShiftChangeDetailRecord.ApprovedDate = DateTime.UtcNow;

                    await _ShiftChangeDetailRepository.UpdateShiftChangeDetailAsync(ShiftChangeDetailRecord);
                }
            }
        }


        public async Task DeleteShiftChangeMasterAsync(int id)
        {
            await _ShiftChangeMasterRepository.DeleteShiftChangeMasterAsync(id);
        }

        public Task<IEnumerable<ShiftChangeMaster>> AddShiftChangeMastersAsync(IEnumerable<ShiftChangeMasterRequestModel> requestModels)
        {
            throw new NotImplementedException();
        }

        public Task<ShiftChangeMaster> UpdateShiftChangeMasterAsync(int id, ShiftChangeMasterRequestModel ShiftChangeMasterRequestModel)
        {
            throw new NotImplementedException();
        }
    }
}
