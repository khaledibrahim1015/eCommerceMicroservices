using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public class ProdcutRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProdcutRepository(ICatalogContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts()
            => await _context
                        .Products
                            .Find(p => true)
                                .ToListAsync();
        public async Task<Product> GetProductById(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _context
                          .Products
                             .Find(filter)
                                 .FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductByName(string name)
                => await _context
                           .Products
                               .Find(p => p.Name == name)
                                  .FirstOrDefaultAsync();
        public async Task<IEnumerable<Product>> GetProdcutByCategoryName(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
             return  await _context.
                              Products
                               .Find(filter)
                                   .ToListAsync();
        }

       
        public async Task CreateProduct(Product product)
                        =>  await _context.Products.InsertOneAsync(product);
 
        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedResult =  await _context
                                          .Products
                                              .ReplaceOneAsync(filter: p=> p.Id ==  product.Id , replacement: product);

            bool res = updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0; 
            return res;
        }
        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                                .Filter
                                                                     .Eq(p => p.Id, id);
                     var deletedResult =   await _context
                                                  .Products
                                                      .DeleteOneAsync(filter);

            return  deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;

        }

      
    }
}
