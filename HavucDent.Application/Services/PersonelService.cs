using AutoMapper;
using Azure.Core;
using HavucDent.Application.DTOs;
using HavucDent.Application.Interfaces;
using HavucDent.Common.Services;
using HavucDent.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;

namespace HavucDent.Application.Services
{
    public class PersonelService : IPersonelService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public PersonelService(UserManager<AppUser> userManager, IMapper mapper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<bool> CreatePersonelAsync(CreateUserDto userDto, string role)
        {
            var appUser = _mapper.Map<AppUser>(userDto);
            appUser.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(appUser);

            if (result.Succeeded)
            {
                // Rol Atama
                await _userManager.AddToRoleAsync(appUser, role);

                // E-posta doğrulama token oluşturma
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                await SendConfirmationEmailAsync(userDto, token); // Onay e-postası gönder

                return true;
            }
            return false;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            // Kullanıcı doğrulama işlemi
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        //public async Task<UserDto> FindByEmailAsync(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);

        //    return _mapper.Map<UserDto>(user); // Kullanıcıyı DTO olarak döndür
        //}

        //public async Task<bool> IsEmailConfirmedAsync(UserDto userDto)
        //{
        //    // DTO'yu varlığa dönüştürerek e-posta doğrulama durumunu kontrol etme
        //    var user = await _userManager.FindByIdAsync(userDto.Id.ToString());

        //    return user != null && await _userManager.IsEmailConfirmedAsync(user);
        //}

        public async Task SendConfirmationEmailAsync(CreateUserDto userDto, string token)
        {
            // E-posta doğrulama bağlantısını oluşturma
            //var confirmationLink = $"localhost:1903/confirmemail?userId={userDto.Id}&token={token}";
            var confirmationLink = $"localhost:1903/confirmemail";
            //var confirmationLink = Url.Action("ConfirmEmail", "Account",
            //    new { userId = userDto.Id, token = token }, Request.Scheme);
            var message = $"Merhaba {userDto.FirstName}, hesabınızı doğrulamak için aşağıdaki bağlantıya tıklayın: {confirmationLink}";

            await _emailSender.SendEmailAsync(userDto.Email, "E-posta Doğrulama", message);
        }

        public async Task<bool> UpdatePersonelAsync(UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null) return false;

            _mapper.Map(userDto, user);
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeletePersonelAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<List<UserDto>> GetAllPersonelAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDto>();

            foreach (var user in users) 
            {
                var userDto = _mapper.Map<UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                //userDto.Role = roles.FirstOrDefault(); // İlk rolü ekle
                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<CreateUserDto> GetPersonelByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return _mapper.Map<CreateUserDto>(user);
        }
    }
}
