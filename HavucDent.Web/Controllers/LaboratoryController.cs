using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HavucDent.Web.Controllers
{
    public class LaboratoryController : Controller
    {
        private readonly ILaboratoryService _laboratoryService;

        public LaboratoryController(ILaboratoryService laboratoryService)
        {
            _laboratoryService = laboratoryService;
        }

        // GET: /Laboratory/Index
        public async Task<IActionResult> Index()
        {
            var laboratories = await _laboratoryService.GetAllLaboratoriesAsync();
            return View(laboratories);
        }

        // GET: /Laboratory/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Laboratory/Create
        [HttpPost]
        public async Task<IActionResult> Create(Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                await _laboratoryService.AddLaboratoryAsync(laboratory);
                return RedirectToAction("Index");
            }
            return View(laboratory);
        }

        // GET: /Laboratory/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var laboratory = await _laboratoryService.GetLaboratoryByIdAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: /Laboratory/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                await _laboratoryService.UpdateLaboratoryAsync(laboratory);
                return RedirectToAction("Index");
            }
            return View(laboratory);
        }

        // GET: /Laboratory/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var laboratory = await _laboratoryService.GetLaboratoryByIdAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: /Laboratory/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _laboratoryService.DeleteLaboratoryAsync(id);
            return RedirectToAction("Index");
        }
    }
}
