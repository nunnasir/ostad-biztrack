using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure.Repositories;

public class ProductRepository : Repository<Product, int>, IProductRepository
{
    public ProductRepository(BizTrackDbContext context) : base(context)
    {
    }
}
