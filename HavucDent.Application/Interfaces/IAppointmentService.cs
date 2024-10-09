using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task AddAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    }
}
