using Ecommerce.DataAccess.Data;
using EntityLayer.Repositories;
namespace Ecommerce.DataAccess.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public IDiscountRepository Discount { get; private set; }

        public IProductRepository Product { get; }

        public UnitOfWork(Context context)
        {
            this.Discount = new DiscountRepository(context);
            this.Product = new ProductRepository(context);
            this._context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
