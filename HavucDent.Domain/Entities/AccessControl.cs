
namespace HavucDent.Domain.Entities
{
    public class AccessControl
    {
        public int Id { get; set; }
        public string RoleName { get; set; } // Rol adı
        public string ControllerName { get; set; } // Controller adı
        public string ActionName { get; set; } // Action adı
    }
}
