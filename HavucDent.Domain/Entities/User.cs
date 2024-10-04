using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public abstract class User 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TcKimlikNo { get; set; }
        public string SgkNo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime SalaryPaymentDate { get; set; }
        public int AnnualLeaveDays { get; set; } // Yıllık izin gün sayısı
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public virtual ICollection<LeaveHistory> LeaveHistories { get; set; } // İzin geçmişi
        public virtual ICollection<SalaryHistory> SalaryHistories { get; set; } // Maaş geçmişi
    }
}
