using Dapper;
using Discount.Api.Data;
using Discount.Api.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Services
{
    public class DiscountService : IDiscountService
    {

        private readonly IDiscountDbContext _db;

        public DiscountService(IDiscountDbContext db)
        {
            _db = db;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
           var coupon=   await _db.DbConnection.QueryFirstOrDefaultAsync<Coupon>
                                 ("select * from coupon where ProductName = @ProductName", new { ProductName = productName });
            if(coupon!=null)
                return coupon;

            return new Coupon()
            { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {

           var affected =await  _db.DbConnection
                              .ExecuteAsync("Insert into Coupon (ProductName, Description, Amount) VALUES (@ProductName,@Description ,@Amount)");
            return affected == 0 ? false : true;        
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var deletedRow =await  _db.DbConnection
                                  .ExecuteAsync("delete from coupon where ProductName = @ProdcutName ", new { ProductName = productName });
            return deletedRow == 0 ? false : true;
        }


        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var updatedRow =await _db.DbConnection.
                                            ExecuteAsync
                                            ("Update Coupon Set ProdcutName = @ProdcutName , Description = @Description , Amount =  @Amount",
                                            new { ProdcutName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount }
                                            );
            return updatedRow == 0 ? false : true;
        }
    }
}
