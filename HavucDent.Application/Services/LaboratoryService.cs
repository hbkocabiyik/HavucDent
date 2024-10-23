using HavucDent.Application.Interfaces;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HavucDent.Application.Services
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly IRepository<Laboratory> _laboratoryRepository;

        public LaboratoryService(IRepository<Laboratory> laboratoryRepository)
        {
            _laboratoryRepository = laboratoryRepository;
        }

        public async Task<IEnumerable<Laboratory>> GetAllLaboratoriesAsync()
        {
            return await _laboratoryRepository.GetAllAsync();
        }

        public async Task<Laboratory> GetLaboratoryByIdAsync(int id)
        {
            return await _laboratoryRepository.GetByIdAsync(id);
        }

        public async Task AddLaboratoryAsync(Laboratory laboratory)
        {
            await _laboratoryRepository.AddAsync(laboratory);
        }

        public async Task UpdateLaboratoryAsync(Laboratory laboratory)
        {
            _laboratoryRepository.Update(laboratory);
        }

        public async Task DeleteLaboratoryAsync(int id)
        {
            var laboratory = await _laboratoryRepository.GetByIdAsync(id);
            if (laboratory != null)
            {
                _laboratoryRepository.Remove(laboratory);
            }
        }
    }
}