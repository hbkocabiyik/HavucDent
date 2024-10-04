using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HavucDent.Application.Services
{
    public class AppointmentService
    {
        private readonly HavucDbContext _context;

        public AppointmentService(HavucDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDoctorAvailable(int doctorId, DateTime appointmentDate)
        {
            var appointment = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate)
                .FirstOrDefaultAsync();

            return appointment == null || appointment.IsAvailable;
        }

        public async Task AssignDoctorToAppointment(int patientId, DateTime appointmentDate)
        {
            var availableDoctor = await _context.Doctors
                .Where(d => !d.Appointments.Any(a => a.AppointmentDate == appointmentDate && !a.IsAvailable))
                .FirstOrDefaultAsync();

            if (availableDoctor == null)
                throw new Exception("No available doctors at this time.");

            var appointment = new Appointment
            {
                DoctorId = availableDoctor.Id,
                AppointmentDate = appointmentDate,
                IsAvailable = false
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
