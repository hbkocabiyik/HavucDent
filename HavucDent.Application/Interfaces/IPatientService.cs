using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByTcAsync(string tcNumber);
        Task AddPatientAsync(Patient patient);

    }
}