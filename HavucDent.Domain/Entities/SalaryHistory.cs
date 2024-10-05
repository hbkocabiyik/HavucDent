
namespace HavucDent.Domain.Entities
{
    public class SalaryHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; } // User ID (Doctor, Assistant)
        public User User { get; set; }  // Kullanıcı ile ilişkilendirme
        public decimal Amount { get; set; } // Ödenen maaş miktarı
        public DateTime PaymentDate { get; set; } // Ödeme tarihi
    }
}
