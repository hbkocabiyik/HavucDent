using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Web.Controllers
{
    public class LaboratoryController : Controller
    {
        private readonly ILaboratoryService _laboratoryService;

        public LaboratoryController(ILaboratoryService laboratoryService)
        {
            _laboratoryService = laboratoryService;
        }

        // GET: Laboratory/Index
        public async Task<IActionResult> Index()
        {
            var laboratories = await _laboratoryService.GetAllLaboratoriesAsync();
            return View(laboratories);
        }

        // GET: Laboratory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laboratory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                await _laboratoryService.AddLaboratoryAsync(laboratory);
                return RedirectToAction(nameof(Index));
            }
            return View(laboratory);
        }

        // GET: Laboratory/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var laboratory = await _laboratoryService.GetLaboratoryByIdAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: Laboratory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Laboratory laboratory)
        {
            if (id != laboratory.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _laboratoryService.UpdateLaboratoryAsync(laboratory);
                return RedirectToAction(nameof(Index));
            }
            return View(laboratory);
        }

        // GET: Laboratory/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var laboratory = await _laboratoryService.GetLaboratoryByIdAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: Laboratory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _laboratoryService.DeleteLaboratoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
