using Ecommerce.DataAccess.Data;
using EntityLayer.Models;
using EntityLayer.Repositories;

namespace Ecommerce.DataAccess.Implementations
{
    internal class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(Context context) : base(context)
        {
        }
    }
}
