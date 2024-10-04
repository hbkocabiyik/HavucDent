using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public class Assistant : User
    {
        public virtual ICollection<Appointment> AssignedAppointments { get; set; } // Asistanın atadığı randevular
    }
}
