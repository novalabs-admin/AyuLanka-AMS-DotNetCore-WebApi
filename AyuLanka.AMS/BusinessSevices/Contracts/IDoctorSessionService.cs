using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IDoctorSessionService
    {
        Task<IEnumerable<DoctorSession>> GetAllAsync(int? companyId, DateTime? date);
        Task<DoctorSession?> GetByIdAsync(int id);
        Task<IEnumerable<DoctorSession>> GetByDoctorAsync(int doctorId, DateTime? date);
        Task<object> GetAvailabilityAsync(int sessionId);
        Task<DoctorSession> AddAsync(DoctorSession session);
        Task<DoctorSession?> UpdateAsync(int id, DoctorSession session);
    }
}
