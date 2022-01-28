using Catalog.API.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Entities.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContacts _contaxt;

        public ProductRepository(ICatalogContacts contact)
        {
            _contaxt = contact ?? throw new AccessViolationException(nameof(contact));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _contaxt.Products.Find(p => true).ToListAsync();

        }

        public async Task<Product> GetProduct(string id)
        {
            return await _contaxt.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _contaxt.Products.Find(p => p.Name == name).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateproduct = await _contaxt.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateproduct.IsAcknowledged && updateproduct.ModifiedCount > 0;
        }

        public async Task CreateProduct(Product product)
        {
            await _contaxt.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Where(p => p.Id == id);
            DeleteResult deleteResult = await _contaxt.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
