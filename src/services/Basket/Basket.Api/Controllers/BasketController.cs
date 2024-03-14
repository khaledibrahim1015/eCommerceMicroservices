using Basket.Api.Cache;
using Basket.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController :  ControllerBase
    {
        private readonly ICacheService _cacheService;

        public BasketController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

     
        [HttpGet]
        [Route("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(IEnumerable<ShoppingCart>), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetBasket([FromRoute] string userName)
        {
            var basket = await _cacheService.GetData<IEnumerable<ShoppingCart>>(userName);
            if (basket != null && basket.Count() > 0)
                return Ok(basket);
            else
                return Ok(Enumerable.Empty<ShoppingCart>());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var res = await _cacheService.SetData<ShoppingCart>(basket.UserName, basket, expirationTime);
            return Ok(res);
        }


        [HttpDelete]
        [Route("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket([FromRoute] string userName)
        {
            var removed = await _cacheService.RemoveData(userName);

            if (removed)
                return Ok();

            return NotFound();
        }


    }
}
