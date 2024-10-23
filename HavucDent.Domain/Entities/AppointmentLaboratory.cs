namespace HavucDent.Domain.Entities
{
    public class AppointmentLaboratory
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; } // Bir laboratuvar masrafı tek bir randevuya aittir.
        public virtual Appointment Appointment { get; set; } // Tek bir randevu ile ilişki.

        public int LaboratoryId { get; set; } // Bir laboratuvar masrafı tek bir laboratuvara aittir.
        public virtual Laboratory Laboratory { get; set; } // Tek bir laboratuvar ile ilişki.

        public decimal UnitPrice { get; set; } // Ürünün birim fiyatı
        public int Quantity { get; set; } // Ürünün adedi
        public decimal TotalCost => UnitPrice * Quantity; // Toplam maliyet
    }
}