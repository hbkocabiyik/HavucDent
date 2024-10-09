
namespace HavucDent.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsAvailable { get; set; }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public int? AssistantId { get; set; }
        public virtual Assistant Assistant { get; set; }

        public virtual ICollection<Product> UsedProducts { get; set; } // Randevuda kullanılan ürünler

        public decimal TotalFee { get; set; } // Randevu ve tedavi için alınan toplam ücret
        public bool PaymentStatus { get; set; } // Ödeme durumu (True: Ödenmiş, False: Ödenmemiş)
        public bool IsCompleted { get; set; } // Randevu tamamlandı mı?
    }
}
