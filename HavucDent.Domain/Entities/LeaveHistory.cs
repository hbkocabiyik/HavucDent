using System;
    
namespace HavucDent.Domain.Entities
{
    public class LeaveHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DaysUsed { get; set; } // Kullanılan izin gün sayısı
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
    }
}
