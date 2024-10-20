using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HavucDent.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.AddAppointmentAsync(appointment);
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }
    }
}
