using HavucDent.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HavucDent.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task AddAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
    }
}