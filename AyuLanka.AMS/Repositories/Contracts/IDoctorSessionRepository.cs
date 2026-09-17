using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IDoctorSessionRepository
    {
        Task<IEnumerable<DoctorSession>> GetAllAsync(int? companyId, DateTime? date);
        Task<DoctorSession?> GetByIdAsync(int id);
        Task<IEnumerable<DoctorSession>> GetByDoctorAsync(int doctorId, DateTime? date);
        Task<int> GetBookedCountAsync(int sessionId);
        Task<DoctorSession> AddAsync(DoctorSession session);
        Task<DoctorSession> UpdateAsync(DoctorSession session);
    }
}
