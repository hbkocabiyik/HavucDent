using HavucDent.Application.DTOs;
using HavucDent.Application.Interfaces;
using HavucDent.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HavucDent.Web.Controllers
{
    public class PersonelController : Controller
    {
        private readonly IPersonelService _personelService;

        public PersonelController(IPersonelService personelService)
        {
            _personelService = personelService;
        }

        public async Task<IActionResult> Index()
        {
            var personelList = await _personelService.GetAllPersonelAsync();

            return View(personelList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto model, string role)
        {
            if (ModelState.IsValid)
            {
                var result = await _personelService.CreatePersonelAsync(model, role);

                if (result)
                {
                    return Json(new { success = true, message = "Personel başarıyla eklendi." });
                }

                return Json(new { success = false, message = "Personel eklenirken hata oluştu." });
            }

            return Json(new { success = false, message = "Geçersiz model verisi." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var personel = await _personelService.GetPersonelByIdAsync(id);

            return View(personel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Girdi verilerini doğrulayın." });

            var result = await _personelService.UpdatePersonelAsync(model);

            if (result)
                return Json(new { success = true, message = "Personel başarıyla güncellendi." });

            return Json(new { success = false, message = "Personel güncellenirken hata oluştu." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _personelService.DeletePersonelAsync(id);

            if (result)
                return Json(new { success = true, message = "Personel başarıyla silindi." });

            return Json(new { success = false, message = "Personel silinirken hata oluştu." });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "Home");

            var result = await _personelService.ConfirmEmailAsync(userId, token);

            if (result)
            {
                // Kullanıcı e-posta onayı başarılı, şifre belirleme sayfasına yönlendir
                TempData["Token"] = token; // Token'ı geçici olarak sakla
                return RedirectToAction("SetPassword", new { userId });
            }

            // Hata durumunda bir hata sayfasına yönlendir
            ViewData["TokenExpired"] = true; // Token süresi dolmuş veya geçersiz
            return View("SetPassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SetPassword(string userId)
        {
            if (TempData["Token"] == null)
            {
                return RedirectToAction("Error", "Home"); // Token olmadan erişim yok
            }

            var model = new SetPasswordViewModel { UserId = userId, Token = TempData["Token"].ToString() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Token doğrulama işlemi
            var isValidToken = await _personelService.VerifyTokenAsync(model.UserId, model.Token);
            if (!isValidToken)
            {
                ModelState.AddModelError(string.Empty, "Token geçersiz veya süresi dolmuş.");
                return View(model);
            }

            var result = await _personelService.SetPasswordAsync(model.UserId, model.Password);
            if (result)
            {
                TempData["Success"] = "Şifre başarıyla oluşturuldu. Giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Account");
            }

            // Hata durumunda model hata mesajı ile tekrar yüklenir
            ModelState.AddModelError(string.Empty, "Şifre belirleme işlemi başarısız.");
            return View(model);
        }



        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> SetPassword(SetPasswordDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _personelService.SetPasswordAsync(model);

        //        if (result)
        //        {
        //            TempData["Success"] = "Şifre başarıyla oluşturuldu. Giriş yapabilirsiniz.";
        //            return RedirectToAction("Login", "Account");
        //        }

        //        ModelState.AddModelError("", "Şifre oluşturulurken hata oluştu.");
        //    }
        //    return View(model);
        //}
    }
}