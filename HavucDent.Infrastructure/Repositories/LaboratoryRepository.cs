using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;

namespace HavucDent.Infrastructure.Repositories
{
    public class LaboratoryRepository : Repository<Laboratory>
    {
        public LaboratoryRepository(HavucDbContext context) : base(context)
        {
        }

        // Laboratory ile ilgili özel metodlar eklenebilir
    }
}
