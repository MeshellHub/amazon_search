using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonApp.Models.Networking.Responses
{
    public class ProductResponse : Response
    {
        public List<Product> Products { get; set; } = new List<Product>();

        public string TotalProducts { get; set; }

        public int MaxResults { get; set; }

        public int PageCount { get; set; }

        public string MoreResultsUrl { get; set; }

        public bool CanRetrieveAllProducts
        {
            get
            {
                int result;

                bool success = int.TryParse(TotalProducts, out result);

                if (success)
                {
                    return MaxResults >= result;
                }

                return false;
            }
        }

        public List<Product> CombineWith(ProductResponse response)
        {
            int count = Networking.REALITEMSPERPAGE - Products.Count;
            List<Product> toAdd = response.Products.Take(count).ToList();
            
            Products.AddRange(toAdd);

            response.Products.RemoveRange(0, count);

            return response.Products;
        }
    }
}