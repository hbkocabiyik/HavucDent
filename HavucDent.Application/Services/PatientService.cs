using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Interfaces;

namespace HavucDent.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public async Task AddPatientAsync(Patient patient)
		{
			await _unitOfWork.Patients.AddAsync(patient);
			await _unitOfWork.SaveChangesWithIntAsync();
		}

		public async Task<Patient> GetPatientByTcAsync(string tcNumber)
        {
            return await _unitOfWork.Patients.FirstOrDefaultAsync(p => p.TcKimlikNo == tcNumber);
        }
    }
}