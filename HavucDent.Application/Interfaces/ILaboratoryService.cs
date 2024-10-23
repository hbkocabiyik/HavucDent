using HavucDent.Domain.Entities;

namespace HavucDent.Application.Interfaces
{
    public interface ILaboratoryService
    {
        Task<IEnumerable<Laboratory>> GetAllLaboratoriesAsync();
        Task<Laboratory> GetLaboratoryByIdAsync(int id);
        Task AddLaboratoryAsync(Laboratory laboratory);
        Task UpdateLaboratoryAsync(Laboratory laboratory);
        Task DeleteLaboratoryAsync(int id);
    }
}
