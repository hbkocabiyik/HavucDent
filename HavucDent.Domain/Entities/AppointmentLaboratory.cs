namespace HavucDent.Domain.Entities
{
    public class AppointmentLaboratory
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

        public int LaboratoryId { get; set; }
        public virtual Laboratory Laboratory { get; set; }

        public decimal UnitPrice { get; set; } // Ürünün birim fiyatı
        public int Quantity { get; set; } // Ürünün adedi
        public decimal TotalCost => UnitPrice * Quantity; // Toplam maliyet
    }
}