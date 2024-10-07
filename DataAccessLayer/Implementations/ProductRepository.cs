using Ecommerce.DataAccess.Data;
using EntityLayer.Models;
using EntityLayer.Repositories;

namespace Ecommerce.DataAccess.Implementations
{
    internal class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(Context context) : base(context)
        {
        }
    }
}
