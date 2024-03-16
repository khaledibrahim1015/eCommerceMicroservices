using System.Data;

namespace Discount.Api.Data
{
    public interface IDiscountDbContext
    {
            public IDbConnection DbConnection { get; }

    }
}
