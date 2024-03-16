using Discount.Api.Entities;
using System.Threading.Tasks;

namespace Discount.Api.Services
{
    public interface IDiscountService
    {
        Task< Coupon> GetDiscount(string productName);

        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);

    }
}
