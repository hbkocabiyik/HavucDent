﻿using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface IAppointmentService
    {
	    Task AddAppointmentAsync(Appointment appointment);

	    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();

	    Task<Appointment> GetAppointmentByIdAsync(int id);

	    Task UpdateAppointmentAsync(Appointment appointment);

	    Task DeleteAppointmentAsync(int id);

	    IEnumerable<(DateTime Start, DateTime End)> GetAvailableTimeSlots(DateTime date);

		Task<IEnumerable<Appointment>> GetAvailableAppointmentsAsync();

	    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);

	    Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);

		// Haftalık randevuları doktor bazında getirir.
		Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int? doctorId, DateTime weekStart, DateTime weekEnd);
	}
}