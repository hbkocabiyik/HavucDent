using AutoMapper;
using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using HavucDent.Infrastructure.Repositories;
using HavucDent.Web.Models;

namespace HavucDent.Web.Controllers
{
    public class LaboratoryController : Controller
    {
        private readonly ILaboratoryService _laboratoryService;
        private readonly IMapper _mapper;

		public LaboratoryController(ILaboratoryService laboratoryService, IMapper mapper)
        {
            _laboratoryService = laboratoryService;
            _mapper = mapper;
		}

        // GET: Laboratory/Index
        public async Task<IActionResult> Index()
        {
			var laboratories = await _laboratoryService.GetAllLaboratoriesAsync();
			var laboratoryViewModels = _mapper.Map<IEnumerable<LaboratoryViewModel>>(laboratories);

			return View(laboratoryViewModels);
		}

        // GET: Laboratory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laboratory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LaboratoryViewModel model)
        {
			if (ModelState.IsValid)
            {
	            var laboratory = _mapper.Map<Laboratory>(model);
				await _laboratoryService.AddLaboratoryAsync(laboratory);

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Geçersiz veri." });
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
