using EntityLayer.Models;

namespace EntityLayer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDiscountRepository Discount { get; }
        IProductRepository Product { get; }
        int Complete();
    }
}
