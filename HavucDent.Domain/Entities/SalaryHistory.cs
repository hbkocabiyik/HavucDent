using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public class SalaryHistory
    {
        public int Id { get; set; }
        public decimal SalaryPaid { get; set; }
        public DateTime PaymentDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
