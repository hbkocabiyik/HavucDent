
namespace HavucDent.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //Unit of work'ü sadece transaction yönetimi için kullanmak ve
        //her servisin tüm unit of worke bağımlı olmamasını ve
        //her servisin ihtiyacı olan repository kendi servis katmanına eklemek ve
        //modüler hale getirmek için refactör işlemi yapıldı

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}