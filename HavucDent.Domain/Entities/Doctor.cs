using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public class Doctor : User
    {
        public decimal CommissionRate { get; set; } // Maaş komisyonu
        public decimal ProductCommissionRate { get; set; } // Ürün komisyonu

        public virtual ICollection<Appointment> Appointments { get; set; } // Doktorun randevuları
    }
}
