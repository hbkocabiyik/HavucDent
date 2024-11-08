using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HavucDent.Web.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly IAppointmentService _appointmentService;
		private readonly IHubContext<AppointmentHub> _appointmentHubContext;

		public AppointmentController(IAppointmentService appointmentService, IHubContext<AppointmentHub> appointmentHubContext)
		{
			_appointmentService = appointmentService;
			_appointmentHubContext = appointmentHubContext;
		}

		// GET: Appointment/Index
		public async Task<IActionResult> Index(DateTime? date)
		{
			if (date == null) date = DateTime.Today;

			var availableTimeSlots = _appointmentService.GetAvailableTimeSlots(date.Value);

			return View(availableTimeSlots);
		}

		// GET: Appointment/Create
		public IActionResult Create(DateTime start, DateTime end)
		{
			ViewBag.StartTime = start;
			ViewBag.EndTime = end;

			return View();
		}

		// POST: Appointment/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Appointment appointment)
		{
			if (ModelState.IsValid)
			{
				await _appointmentService.AddAppointmentAsync(appointment);
				return RedirectToAction(nameof(Index));
			}

			return View(appointment);
		}

		// GET: Appointment/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
			if (appointment == null)
			{
				return NotFound();
			}

			return View(appointment);
		}

		// POST: Appointment/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Appointment appointment)
		{
			if (id != appointment.Id)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				await _appointmentService.UpdateAppointmentAsync(appointment);

				return RedirectToAction(nameof(Index));
			}

			return View(appointment);
		}

		// GET: Appointment/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
			if (appointment == null)
			{
				return NotFound();
			}

			return View(appointment);
		}

		// POST: Appointment/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _appointmentService.DeleteAppointmentAsync(id);

			return RedirectToAction(nameof(Index));
		}

		// Haftalık randevuları getirir
		//public async Task<IActionResult> WeeklySchedule(int doctorId = 0, DateTime? startDate = null)
		//{
		//	var weekStartDate = startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
		//	var weekEndDate = weekStartDate.AddDays(7);

		//	var appointments = await _appointmentService.GetWeeklyAppointmentsAsync(doctorId, weekStartDate, weekEndDate);
		//	ViewBag.CurrentWeekStart = weekStartDate;

		//	return View(appointments);
		//}


		[HttpGet]
		public async Task<IActionResult> WeeklySchedule(DateTime? startDate, int? doctorId)
		{
			// startDate değeri yoksa, Pazartesi olan en yakın haftanın başlangıç tarihini kullanır
			DateTime weekStartDate = startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
			DateTime weekEndDate = weekStartDate.AddDays(6);

			var appointments = await _appointmentService.GetWeeklyAppointmentsAsync(doctorId, weekStartDate, weekEndDate);

			ViewBag.DoctorId = doctorId;
			ViewBag.StartDate = weekStartDate;
			ViewBag.EndDate = weekEndDate;

			return View(appointments);
		}

		public async Task<IActionResult> AddAppointment(Appointment appointment)
		{
			await _appointmentService.AddAppointmentAsync(appointment);
			await _appointmentHubContext.Clients.All.SendAsync("ReceiveAppointmentsUpdate");

			return RedirectToAction("WeeklySchedule", new { doctorId = appointment.DoctorId });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAppointment(int id, int doctorId)
		{
			await _appointmentService.DeleteAppointmentAsync(id);
			await _appointmentHubContext.Clients.All.SendAsync("ReceiveAppointmentsUpdate");

			return RedirectToAction("WeeklySchedule", new { doctorId });
		}
	}
}
