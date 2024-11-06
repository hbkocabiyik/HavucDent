using HavucDent.Application.DTOs;

namespace HavucDent.Application.Interfaces
{
    public interface IPersonelService
    {
        // Kullanıcı oluşturma işlemi, CreateUserDto ile kullanıcı verileri ve rol bilgisi alınır
        Task<bool> CreatePersonelAsync(CreateUserDto userDto, string role);

        // Kullanıcı e-posta doğrulama işlemi
        Task<bool> ConfirmEmailAsync(string userId, string token);

        // Kullanıcının parola oluşturma işlemi
        Task<bool> SetPasswordAsync(string userId, string password);

        // E-posta doğrulama bağlantısı tekrar gönderme işlemi
        Task<bool> ResendConfirmationEmailAsync(string userId);

        // Token doğrulama işlemi
        Task<bool> VerifyTokenAsync(string userId, string token);

        Task<bool> UpdatePersonelAsync(UpdateUserDto userDto);

        Task<bool> DeletePersonelAsync(int userId);

        Task<List<UserDto>> GetAllPersonelAsync();

        Task<CreateUserDto> GetPersonelByIdAsync(int id);
    }
}