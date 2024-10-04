using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavucDent.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } // Ürünün kullanıldığı randevular
    }
}
