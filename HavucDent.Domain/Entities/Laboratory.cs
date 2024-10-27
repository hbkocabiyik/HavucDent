using System.ComponentModel.DataAnnotations.Schema;

namespace HavucDent.Domain.Entities
{
    public class Laboratory
    {
        public int Id { get; set; }
        public string ProductName { get; set; } // Kullanılan ürünün adı (örn. Zirkonyum Dolgu)
        public string CompanyName { get; set; } // Laboratuvarın firma adı (örn. XX Laboratuvar)
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string CreatedByUserMail { get; set; }
        public string? UpdatedByUserMail { get; set; }

        [NotMapped]
		public virtual ICollection<AppointmentLaboratory> AppointmentLaboratories { get; set; } // Laboratuvar randevu ilişkisi
    }
}
