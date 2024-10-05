
namespace HavucDent.Domain.Entities
{
    public class Assistant : User
    {
        public virtual ICollection<Appointment> AssignedAppointments { get; set; } // Asistanın atadığı randevular
    }
}
