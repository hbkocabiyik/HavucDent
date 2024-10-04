using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HavucDent.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccessControlController : Controller
    {
        private readonly HavucDbContext _context;

        public AccessControlController(HavucDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var accessControls = _context.AccessControls.ToList();
            return View(accessControls);
        }

        [HttpGet]
        public IActionResult AddAccessControl()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAccessControl(AccessControl model)
        {
            if (ModelState.IsValid)
            {
                _context.AccessControls.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccessControl(int id)
        {
            var accessControl = await _context.AccessControls.FindAsync(id);
            if (accessControl != null)
            {
                _context.AccessControls.Remove(accessControl);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
