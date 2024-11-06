﻿using AutoMapper;
using HavucDent.Application.DTOs;
using HavucDent.Application.Interfaces;
using HavucDent.Common.Services;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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

                await _emailSender.SendEmailAsync(userDto.Email, "E-posta Doğrulama", GenerateEmailContent(userDto, encodedToken));// Onay e-postası gönder
                return true;
            }
            return false;
        }

        private string GenerateEmailContent(CreateUserDto userDto, string token)
        {
            var domain = _configuration["ApplicationSettings:Domain"];
            var port = _configuration["ApplicationSettings:Port"];
            var confirmationLink = $"{domain}:{port}/personel/confirmemail?userId={userDto.Id}&token={WebUtility.UrlEncode(token)}";

            return $"Merhaba {userDto.FirstName}, hesabınızı doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a> <br><br/> Bağlantı adresi:  {confirmationLink}";
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            // Kullanıcı doğrulama işlemi
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return false;

            var decodedToken = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

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

        public async Task<bool> ResendConfirmationEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var newToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "E-posta Doğrulama", GenerateEmailContent(new CreateUserDto() { Id = user.Id, Email = user.Email, FirstName = user.FirstName }, newToken));

            return true;
        }

        public async Task<bool> VerifyTokenAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var decodedToken = WebUtility.UrlDecode(token);

            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "EmailConfirmation", decodedToken);
            return isTokenValid;
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
