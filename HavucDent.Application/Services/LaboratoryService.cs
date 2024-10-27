using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Application.Services
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LaboratoryService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Laboratory>> GetAllLaboratoriesAsync()
        {
            var laboratories = await _unitOfWork.Laboratories
                .FindAsync(l => !l.IsDeleted);

            return laboratories.OrderBy(l => l.Id); // Id’ye göre sıralama
        }

        private string CurrentUserEmail => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonim";

        public async Task<Laboratory> GetLaboratoryByIdAsync(int id)
        {
            return await _unitOfWork.Laboratories.GetByIdAsync(id);
        }

        public async Task AddLaboratoryAsync(Laboratory laboratory)
        {
            laboratory.CreatedDate = DateTime.UtcNow;
            laboratory.CreatedByUserMail = CurrentUserEmail;
            laboratory.UpdatedByUserMail = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.Laboratories.AddAsync(laboratory);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateLaboratoryAsync(Laboratory laboratory)
        {
            var existingLab = await GetLaboratoryByIdAsync(laboratory.Id);
            if (existingLab == null) return;

            existingLab.CompanyName = laboratory.CompanyName;
            existingLab.ProductName = laboratory.ProductName;
            existingLab.UpdatedDate = DateTime.UtcNow;
            existingLab.UpdatedByUserMail = CurrentUserEmail;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.Laboratories.Update(existingLab);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteLaboratoryAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var laboratory = await _unitOfWork.Laboratories.GetByIdAsync(id);

                laboratory.IsDeleted = true;
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }
    }
}
