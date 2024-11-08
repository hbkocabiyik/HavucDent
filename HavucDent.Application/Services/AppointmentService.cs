﻿using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Interfaces;

namespace HavucDent.Application.Services
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AppointmentService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task AddAppointmentAsync(Appointment appointment)
		{
			await _unitOfWork.BeginTransactionAsync();

			try
			{
				await _unitOfWork.Appointments.AddAsync(appointment);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitTransactionAsync();
			}
			catch
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
		{
			return await _unitOfWork.Appointments.GetAllAsync();
		}

		public async Task<Appointment> GetAppointmentByIdAsync(int id)
		{
			return await _unitOfWork.Appointments.GetByIdAsync(id);
		}

		public async Task UpdateAppointmentAsync(Appointment appointment)
		{
			await _unitOfWork.BeginTransactionAsync();

			try
			{
				_unitOfWork.Appointments.Update(appointment);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitTransactionAsync();
			}
			catch
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task DeleteAppointmentAsync(int id)
		{
			await _unitOfWork.BeginTransactionAsync();

			try
			{
				var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

				_unitOfWork.Appointments.Remove(appointment);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitTransactionAsync();
			}
			catch
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}


		// Haftalık randevuları belirli bir doktora göre filtreleme
		public async Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int? doctorId, DateTime weekStart, DateTime weekEnd)
		{
			return await _unitOfWork.Appointments.GetAppointmentsByDateRangeAsync(doctorId, weekStart, weekEnd);
		}

		public IEnumerable<(DateTime Start, DateTime End)> GetAvailableTimeSlots(DateTime date)
		{
			var startTime = date.Date.AddHours(9); // Saat 9:00
			var endTime = date.Date.AddHours(22); // Saat 22:00
			var timeSlots = new List<(DateTime Start, DateTime End)>();

			while (startTime < endTime)
			{
				timeSlots.Add((startTime, startTime.AddMinutes(30)));
				startTime = startTime.AddMinutes(30);
			}

			return timeSlots;
		}

		public async Task<IEnumerable<Appointment>> GetAvailableAppointmentsAsync()
		{
			return await _unitOfWork.Appointments.FindAsync(a => a.IsAvailable);
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
		{
			return await _unitOfWork.Appointments.FindAsync(a => a.DoctorId == doctorId);
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
		{
			return await _unitOfWork.Appointments.FindAsync(a => a.PatientId == patientId);
		}
	}
}
