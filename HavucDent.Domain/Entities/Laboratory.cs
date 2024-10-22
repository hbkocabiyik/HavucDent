
namespace HavucDent.Domain.Entities
{
    public class Laboratory
    {
        public int Id { get; set; }
        public string ProductName { get; set; } // Kullanılan ürünün adı (örn. Zirkonyum Dolgu)
        public string CompanyName { get; set; } // Laboratuvarın firma adı (örn. XX Laboratuvar)

        public virtual ICollection<AppointmentLaboratory> AppointmentLaboratories { get; set; } // Laboratuvar randevu ilişkisi
    }
}
