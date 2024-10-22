
namespace HavucDent.Domain.Entities
{
    public class Doctor : User
    {
        public decimal CommissionRate { get; set; } // Maaş komisyonu
        public decimal LaboratoryCommissionRate { get; set; } // Laboratuvar komisyonu

        public virtual ICollection<Appointment> Appointments { get; set; } // Doktorun randevuları
    }
}
