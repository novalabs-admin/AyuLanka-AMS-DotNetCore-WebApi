using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class DoctorSessionService : IDoctorSessionService
    {
        private readonly IDoctorSessionRepository _doctorSessionRepository;

        public DoctorSessionService(IDoctorSessionRepository doctorSessionRepository)
        {
            _doctorSessionRepository = doctorSessionRepository;
        }

        public async Task<IEnumerable<DoctorSession>> GetAllAsync(int? companyId, DateTime? date)
        {
            return await _doctorSessionRepository.GetAllAsync(companyId, date);
        }

        public async Task<DoctorSession?> GetByIdAsync(int id)
        {
            return await _doctorSessionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DoctorSession>> GetByDoctorAsync(int doctorId, DateTime? date)
        {
            return await _doctorSessionRepository.GetByDoctorAsync(doctorId, date);
        }

        public async Task<object> GetAvailabilityAsync(int sessionId)
        {
            var session = await _doctorSessionRepository.GetByIdAsync(sessionId);
            if (session == null) return null;

            var booked = await _doctorSessionRepository.GetBookedCountAsync(sessionId);
            return new
            {
                SessionId = sessionId,
                MaxPatients = session.MaxPatients,
                Booked = booked,
                Remaining = session.MaxPatients - booked,
                IsFull = booked >= session.MaxPatients
            };
        }

        public async Task<DoctorSession> AddAsync(DoctorSession session)
        {
            if (session.EndTime <= session.StartTime)
                throw new InvalidOperationException("End time must be after start time.");

            return await _doctorSessionRepository.AddAsync(session);
        }

        public async Task<DoctorSession?> UpdateAsync(int id, DoctorSession session)
        {
            var existing = await _doctorSessionRepository.GetByIdAsync(id);
            if (existing == null) return null;

            if (session.EndTime <= session.StartTime)
                throw new InvalidOperationException("End time must be after start time.");

            existing.DoctorId = session.DoctorId;
            existing.SessionDate = session.SessionDate;
            existing.StartTime = session.StartTime;
            existing.EndTime = session.EndTime;
            existing.MaxPatients = session.MaxPatients;
            existing.IsActive = session.IsActive;
            existing.Remarks = session.Remarks;

            return await _doctorSessionRepository.UpdateAsync(existing);
        }
    }
}
