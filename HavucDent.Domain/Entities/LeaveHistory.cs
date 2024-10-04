using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public class LeaveHistory
    {
        public int Id { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public int LeaveDaysUsed { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
