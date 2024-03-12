using Catalog.Api.Data.SeedData;
using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public  IMongoCollection<Product> Products { get; }


        public CatalogContext(IConfiguration configuration )
        {
            MongoClient client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            IMongoDatabase dataBase = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = dataBase.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }




    }
}
