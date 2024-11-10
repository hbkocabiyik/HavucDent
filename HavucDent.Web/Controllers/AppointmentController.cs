using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Web.Hubs;
using HavucDent.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HavucDent.Web.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly IAppointmentService _appointmentService;
		private readonly IHubContext<AppointmentHub> _appointmentHubContext;
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;

        public AppointmentController(
            IAppointmentService appointmentService, 
            IHubContext<AppointmentHub> appointmentHubContext, 
            IPatientService patientService, 
            IUserService userService)
		{
			_appointmentService = appointmentService;
			_appointmentHubContext = appointmentHubContext;
			_patientService = patientService;
			_userService = userService;
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
            DateTime weekEndDate = weekStartDate.AddDays(7);

            var weeklyAppointments = await _appointmentService.GetWeeklyAppointmentsAsync(weekStartDate, weekEndDate, doctorId);
            var doctors = await _appointmentService.GetAllDoctorsAsync();

            ViewBag.DoctorId = doctorId;
            ViewBag.StartDate = weekStartDate;
            ViewBag.EndDate = weekEndDate;
            ViewBag.Doctors = doctors;

            return View(weeklyAppointments);
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


        #region Ajax & SignalR Services

        [HttpGet]
        public async Task<IActionResult> GetPatientByTc(string tcNumber)
        {
            var patient = await _patientService.GetPatientByTcAsync(tcNumber);

            if (patient != null)
                return Json(new { exists = true, patient });
            

            return Json(new { exists = false });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false });

            var patient = new Patient
            {
	            TcKimlikNo = model.TcNumber,
	            FirstName = model.FirstName,
	            LastName = model.LastName,
	            Email = model.Email,
	            PhoneNumber = model.Phone,
	            Address = model.Address,
	            BirthDate = model.BirthDate,
	            CreateDate = DateTime.Now,
	            UserId = _userService.GetLoggedUserId()
            };

            await _patientService.AddPatientAsync(patient);

			var appointment = new Appointment
            {
                AppointmentDate = model.AppointmentDate,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                AssistantId = _userService.GetLoggedUserId(),
                IsAvailable = model.IsAvailable,
                TotalFee = model.TotalFee,
                PaymentStatus = model.PaymentStatus,
                IsCompleted = model.IsCompleted,
				CreateDate = DateTime.Now
            };

            var result = await _appointmentService.CreateAppointmentAsync(appointment);
            await _appointmentHubContext.Clients.All.SendAsync("ReceiveAppointmentsUpdate");

            return Json(new { success = result });
        }

        #endregion
    }
}
