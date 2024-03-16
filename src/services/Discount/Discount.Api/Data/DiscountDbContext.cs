using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Discount.Api.Data
{
    public class DiscountDbContext : IDiscountDbContext
    {
        public IDbConnection DbConnection { get; }

        public DiscountDbContext( IConfiguration configuration )
        {
            DbConnection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }
    }
}
