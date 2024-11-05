using AutoMapper;
using Azure.Core;
using HavucDent.Application.DTOs;
using HavucDent.Application.Interfaces;
using HavucDent.Common.Services;
using HavucDent.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace HavucDent.Application.Services
{
    public class PersonelService : IPersonelService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public PersonelService(UserManager<AppUser> userManager, IMapper mapper, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<bool> CreatePersonelAsync(CreateUserDto userDto, string role)
        {
            var appUser = _mapper.Map<AppUser>(userDto);
            appUser.Id = Guid.NewGuid().ToString();
            appUser.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(appUser);

            if (result.Succeeded)
            {
                userDto.Id = appUser.Id;

                // Rol Atama
                await _userManager.AddToRoleAsync(appUser, role);

                // E-posta doğrulama token oluşturma
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                var encodedToken = WebUtility.UrlEncode(token);

                await SendConfirmationEmailAsync(userDto, encodedToken); // Onay e-postası gönder

                return true;
            }
            return false;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            // Kullanıcı doğrulama işlemi
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                // Token geçersiz veya süresi dolmuş
                return false;
            }

            return true;
        }

        public async Task<bool> SetPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
                return false;
            

            var addPasswordResult = await _userManager.AddPasswordAsync(user, password);

            return addPasswordResult.Succeeded;
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

            var domain = _configuration["ApplicationSettings:Domain"];
            var port = _configuration["ApplicationSettings:Port"];

            var confirmationLink = $"{domain}:{port}/personel/confirmemail?userId={userDto.Id}&token={token}";
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

        public Task<bool> ResendConfirmationEmailAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
