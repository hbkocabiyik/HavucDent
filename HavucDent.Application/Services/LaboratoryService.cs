using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Interfaces;
using HavucDent.Infrastructure.Repositories;

namespace HavucDent.Application.Services
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LaboratoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Laboratory>> GetAllLaboratoriesAsync()
        {
            return await _unitOfWork.Laboratories.GetAllAsync();
        }

        public async Task<Laboratory> GetLaboratoryByIdAsync(int id)
        {
            return await _unitOfWork.Laboratories.GetByIdAsync(id);
        }

        public async Task AddLaboratoryAsync(Laboratory laboratory)
        {
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
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.Laboratories.Update(laboratory);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteLaboratoryAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var laboratory = await _unitOfWork.Laboratories.GetByIdAsync(id);

                _unitOfWork.Laboratories.Remove(laboratory);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}