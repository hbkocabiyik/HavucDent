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

		// POST: Laboratory/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(LaboratoryViewModel model)
		{

			if (ModelState.IsValid)
			{
				var laboratory = _mapper.Map<Laboratory>(model);

				await _laboratoryService.UpdateLaboratoryAsync(laboratory);

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu." });
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _laboratoryService.DeleteLaboratoryAsync(id);

			if (result)
			{
				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Silme işlemi sırasında bir hata oluştu." });
		}
	}
}
