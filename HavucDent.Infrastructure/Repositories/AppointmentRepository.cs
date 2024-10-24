using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;

namespace HavucDent.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>
    {
        public AppointmentRepository(HavucDbContext context) : base(context)
        {
        }

        // Appointment ile ilgili özel metodlar ekle
    }
}