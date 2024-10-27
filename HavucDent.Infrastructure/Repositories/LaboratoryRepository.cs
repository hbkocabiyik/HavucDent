using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HavucDent.Infrastructure.Repositories
{
    public class LaboratoryRepository : Repository<Laboratory>
    {
        private readonly HavucDbContext _context;

        public LaboratoryRepository(HavucDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Laboratory>> FindAsync(Expression<Func<Laboratory, bool>> predicate)
        {
            return await _context.Laboratories
                .Where(predicate)
                .Where(l => !l.IsDeleted)  // IsDeleted = false olan kayıtları filtrele
                .ToListAsync();
        }

        public async Task SoftDeleteAsync(Laboratory laboratory)
        {
            laboratory.IsDeleted = true;
            _context.Laboratories.Update(laboratory);
            await _context.SaveChangesAsync();
        }
    }
}
