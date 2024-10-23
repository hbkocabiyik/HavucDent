using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.UnitOfWork;

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
            await _unitOfWork.Laboratories.AddAsync(laboratory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateLaboratoryAsync(Laboratory laboratory)
        {
            _unitOfWork.Laboratories.Update(laboratory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteLaboratoryAsync(int id)
        {
            var laboratory = await _unitOfWork.Laboratories.GetByIdAsync(id);
            if (laboratory != null)
            {
                _unitOfWork.Laboratories.Remove(laboratory);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}