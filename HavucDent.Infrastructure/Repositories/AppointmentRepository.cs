using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HavucDent.Infrastructure.Repositories
{
	public class AppointmentRepository : Repository<Appointment>
	{
		public AppointmentRepository(HavucDbContext context) : base(context)
		{
		}

		// Belirli bir doktor ve tarih aralığı için randevuları getiren özel metot
		public async Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(int? doctorId, DateTime startDate, DateTime endDate)
		{
			return await _context.Set<Appointment>()
				.Where(a => a.DoctorId == doctorId && a.AppointmentDate >= startDate && a.AppointmentDate < endDate)
				.ToListAsync();
		}
	}
}