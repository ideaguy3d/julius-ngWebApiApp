using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;


namespace CEDR.WebAPI.Models
{
    /// <summary>
    /// Stores data in .json file so no db is needed.
    /// </summary>

    public class ProductRepository
    {
        /// <summary>
        /// Creates a new product with default vals
        /// </summary>
        /// <returns></returns>  
        internal Product Create()
        {
            
            Product product = new Product
            { 
                ReleaseDate = DateTime.Now
            };
            return product; 
        }

        /// <summary>
        /// Retrieves list of products from a local json file
        /// </summary>
        /// <returns></returns>  
        internal List<Product> Retrieve()
        {
            var filePath = HostingEnvironment.MapPath(@"~/App_Data/product.json");
            var json = System.IO.File.ReadAllText(filePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            return products;    
        }

        /// <summary>
        /// Saves a new product to a local json file
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns> 
        internal Product Save(Product product)
        {
            // read existing products 
            var products = this.Retrieve();

            // assign a new Id
            var maxId = products.Max(p => p.ProductId);
            product.ProductId = maxId + 1;
            products.Add(product);

            WriteData(products); 
            return products[0]; 
        }

        /// <summary>
        /// Updates an existing product of a local json file
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns> 
        internal Product Save(int id, Product product)
        {
            // Read in the existing products
            var products = this.Retrieve();

            // Locate and replace the item
            var itemIndex = products.FindIndex(p => p.ProductId == product.ProductId);
            if(itemIndex > 0)
            {
                products[itemIndex] = product; 
            }
            else
            {
                return null; 
            }

            WriteData(products);
            return product; 
        }

        private bool WriteData(List<Product> products)
        {
            // write out the json
            var filePath = HostingEnvironment.MapPath(@"~/App_Data/product.json");

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);

            return true; 
        }
    }
}