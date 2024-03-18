using Dapper;
using Discount.Grpc.Data;
using Discount.Grpc.Entities;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDiscountDbContext _discountDb;

        public DiscountRepository(IDiscountDbContext discountDb)
        {
            _discountDb = discountDb;
        }


        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon =await _discountDb.DbConnection
                 .QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM COUPON WHERE ProdcutName =@ProductName",
                    new { ProductName = productName } );
            if (coupon != null)
                return coupon;

            return new Coupon()
            { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {

            var affected = await _discountDb.DbConnection
                               .ExecuteAsync("Insert into Coupon (ProductName, Description, Amount) VALUES (@ProductName,@Description ,@Amount)");
            return affected == 0 ? false : true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var deletedRow = await _discountDb.DbConnection
                                  .ExecuteAsync("delete from coupon where ProductName = @ProdcutName ", new { ProductName = productName });
            return deletedRow == 0 ? false : true;
        }


        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var updatedRow = await _discountDb.DbConnection.
                                            ExecuteAsync
                                            ("Update Coupon Set ProdcutName = @ProdcutName , Description = @Description , Amount =  @Amount",
                                            new { ProdcutName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount }
                                            );
            return updatedRow == 0 ? false : true;
        }
    }
}
