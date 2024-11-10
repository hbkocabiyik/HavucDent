
namespace HavucDent.Domain.Entities
{
    public class Appointment
    {
	    public int Id { get; set; }
	    public DateTime AppointmentDate { get; set; } // Randevu tarihi ve saati
	    public bool IsAvailable { get; set; } = true; // Varsayılan olarak randevu mevcut
	    public bool IsCompleted { get; set; } // Tedavi tamamlandı mı?
	    public decimal TotalFee { get; set; } // Randevu için toplam ücret
	    public bool PaymentStatus { get; set; } // Ödeme durumu (Ödendi/Ödenmedi)

	    public int DoctorId { get; set; }
	    public virtual Doctor Doctor { get; set; } // Doktor bilgisi

	    public int PatientId { get; set; }
	    public virtual Patient Patient { get; set; } // Hasta bilgisi

	    public int? AssistantId { get; set; } // Randevuyu atayan asistan
	    public virtual Assistant Assistant { get; set; } // Asistan bilgisi

	    public DateTime CreateDate { get; set; } // Randevu kayıt tarihi

	    public virtual ICollection<Product> UsedProducts { get; set; } // Randevuda kullanılan ürünler

		public virtual ICollection<AppointmentLaboratory> AppointmentLaboratories { get; set; } // Randevuda kullanılan laboratuvar masrafları
	}
}

