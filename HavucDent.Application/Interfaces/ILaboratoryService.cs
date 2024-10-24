﻿using HavucDent.Domain.Entities;

namespace HavucDent.Infrastructure.Repositories
{
    //public interface ILaboratoryRepository : IRepository<Laboratory>
    //{
    //    // İhtiyaca göre ek metodlar tanımlanabilir
    //}

    public interface ILaboratoryService
    {
        Task<IEnumerable<Laboratory>> GetAllLaboratoriesAsync();
        Task<Laboratory> GetLaboratoryByIdAsync(int id);
        Task AddLaboratoryAsync(Laboratory laboratory);
        Task UpdateLaboratoryAsync(Laboratory laboratory);
        Task DeleteLaboratoryAsync(int id);
    }
}