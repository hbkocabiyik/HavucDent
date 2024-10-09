
namespace HavucDent.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TcKimlikNo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public bool IsPaymentCompleted { get; set; } // Ödeme tamamlandı mı? 

        public virtual ICollection<Appointment> Appointments { get; set; } // Hastanın randevuları
    }
}
