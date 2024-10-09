
namespace HavucDent.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; } // Ürün birim fiyatı
        public int StockQuantity { get; set; } // Stok adedi

        public virtual ICollection<Appointment> Appointments { get; set; } // Ürünün kullanıldığı randevular
    }
}
