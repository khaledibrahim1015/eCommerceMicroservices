using Amazon.Runtime.Internal.Util;
using Catalog.Api.Data.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
               => Ok(await _productRepository.GetProducts());
       

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var product = await _productRepository.GetProductById(id);
                if (product != null)
                      return Ok(product) ;
                else
                {
                    _logger.LogError($"product with id {id} Not Exist !!!");
                    return NotFound();
                }
            }              
            else
                return NotFound();
        }


        [HttpGet("[action]/{category}", Name ="GetProductByCategory")]
        [ProducesResponseType(typeof(Product),(int) HttpStatusCode.OK)]
        [ProducesResponseType( (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductByCategory([FromRoute] string category)
        {
            var product  = await _productRepository.GetProdcutByCategoryName(category);
            if(product !=null ) 
                return Ok( product);
            else
                return NotFound();

        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] 
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", product.Id, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
            =>  Ok(await _productRepository.UpdateProduct(product));

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
            =>  Ok(await _productRepository.DeleteProduct(id));


    }
}
