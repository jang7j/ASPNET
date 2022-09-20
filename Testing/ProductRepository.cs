using Dapper;
using System.Collections.Generic;
using System.Data;
using Testing.Models;

namespace Testing
{
    public class ProductRepository: IProductRepository
    {
        private readonly IDbConnection _conn;
        private object productToInsert;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;
            return product;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM products;");
        }

        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });
        }

        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * FROM products where productID = @id", new { id });
        }

        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE products SET name = @name, Price = @price WHERE ProductID = @id", new { name = product.Name, product.Price, id = product.ProductID });
;        }

        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }

        public void DeleteProduct(Product product)
        {
                _conn.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;", new { id = product.ProductID });
                _conn.Execute("DELETE FROM Sales WHERE ProductID = @id;", new { id = product.ProductID });
                _conn.Execute("DELETE FROM Products WHERE ProductID = @id;", new { id = product.ProductID });
            
        }
    }
}
