using System.Data;

namespace Discount.Grpc.Data
{
    public interface IDiscountDbContext
    {
        public IDbConnection DbConnection { get; }
    }
}
