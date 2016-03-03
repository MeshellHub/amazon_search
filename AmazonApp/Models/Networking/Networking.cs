using System.Diagnostics;
using AmazonApp.ServiceReference1;
using AmazonApp.Models.Networking.SOAPSigning;
using System.Collections.Generic;
using AmazonApp.Models.Networking.Responses;
using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace AmazonApp.Models.Networking
{
    public class Networking
    {
        // https://affiliate-program.amazon.com/gp/advertising/api/detail/api-changes.html
        // 2011 changes:
        // ItemPage Parameter: For the ItemSearch operation, 
        // the ItemPage parameter will have a maximum value of 10 instead of 400. 
        // The limit for the “All” search index will continue to be 5 pages. 
        // A new element “MoreSearchResults” will be added to ItemSearch responses, 
        // containing a link to the Amazon website where your customers can see additional search results 
        // beyond the ItemPage limit. 
        // All requests for ItemPage greater than 10 will be rejected with an appropriate error message.

        const string ALL = "All";
        public const int REALITEMSPERPAGE = 13;
        const int MAXITEMSPERPAGE = 10;

        const string ASSOCIATEID = "";
        const string ACCESSKEY = "";
        const string SECRETKEY = "";

        const string OERAPPID = "";
        const string OERURL = "https://openexchangerates.org/api/latest.json?app_id=";

        static int LastRequestedPage { get; set; }
        static List<Product> ProductOverflow { get; set; } = new List<Product>();

        public static ProductResponse GetInitialProducts(string searchIndex, string keyword)
        {
            GetExchangeRates();

            LastRequestedPage = 1;

            ProductResponse response1 = GetProducts(searchIndex, keyword);

            if (response1.PageCount > 1)
            {
                LastRequestedPage++;

                ProductResponse response2 = GetProducts(searchIndex, keyword);

                List<Product> overflow = response1.CombineWith(response2);
                ProductOverflow.AddRange(overflow);
            }

            return response1;
        }

        public static ProductResponse GetAdditionalProducts (string searchIndex, string keyword)
        {
            if (ProductOverflow.Count >= REALITEMSPERPAGE)
            {
                List<Product> products = ProductOverflow.Take(REALITEMSPERPAGE).ToList();
                ProductOverflow.RemoveRange(0, REALITEMSPERPAGE);
                return new ProductResponse { Products = products };
            }

            LastRequestedPage++;
            ProductResponse response1 = GetProducts(searchIndex, keyword);

            int diff = REALITEMSPERPAGE - MAXITEMSPERPAGE;

            if (ProductOverflow.Count >= diff)
            {
                List<Product> products = ProductOverflow.Take(diff).ToList();
                ProductOverflow.RemoveRange(0, diff);
                response1.Products.InsertRange(0, products);
            }
            else
            {
                if (LastRequestedPage <= GetPageCount(searchIndex, response1.TotalProducts))
                {
                    LastRequestedPage++;
                    ProductResponse response2 = GetProducts(searchIndex, keyword);

                    response1.Products.InsertRange(0, ProductOverflow);
                    ProductOverflow.Clear();

                    List<Product> overflow = response1.CombineWith(response2);
                    ProductOverflow.AddRange(overflow);
                }
                else
                {
                    response1.Products.InsertRange(0, ProductOverflow);
                    ProductOverflow.Clear();
                }
            }

            return response1;
        }

        static ProductResponse GetProducts(string searchIndex, string keyword)
        {
            ProductResponse response = new ProductResponse();

            ItemSearch search = new ItemSearch();
            search.AssociateTag = ASSOCIATEID;
            search.AWSAccessKeyId = ACCESSKEY;

            ItemSearchRequest req = new ItemSearchRequest();
            req.ResponseGroup = new string[] { "ItemAttributes", "Images", "OfferFull" };
            req.Availability = ItemSearchRequestAvailability.Available;

            req.ItemPage = LastRequestedPage.ToString();

            req.SearchIndex = searchIndex;
            req.Keywords = keyword;

            search.Request = new ItemSearchRequest[] { req };
            
            AWSECommerceServicePortTypeClient amzwc = new AWSECommerceServicePortTypeClient();
            amzwc.ChannelFactory.Endpoint.EndpointBehaviors.Add(new AmazonSigningEndpointBehavior(ACCESSKEY, SECRETKEY));

            // Throws MessageSecurityException: The HTTP request was forbidden with client authentication scheme 'Anonymous'.
            // if keys missing or don't match
            ItemSearchResponse resp = amzwc.ItemSearch(search);

            Items items = resp.Items[0];

            response.TotalProducts = items.TotalResults;
            response.PageCount = GetPageCount(searchIndex, items.TotalResults);
            response.MoreResultsUrl = items.MoreSearchResultsUrl;

            if (searchIndex.Equals(ALL))
            {
                response.MaxResults = 50; 
            } else
            {
                response.MaxResults = 100;
            }

            if (items.Request.Errors != null && items.Request.Errors.Length > 0)
            {
                response.ErrorMessage = items.Request.Errors[0].Message;
                return response;
            }

            response.Products = Codec.DecodeProducts(items);

            return response;
        }

        static int GetPageCount (string searchIndex, string totalResults)
        {
            double maxPages = 0;

            // cf. explanation at the beginning of this class
            if (searchIndex.Equals(ALL))
            {
                maxPages = 5;
            }
            else
            {
                maxPages = 10;
            }

            double maxResults = maxPages * MAXITEMSPERPAGE;

            int actualResults;

            bool success = int.TryParse(totalResults, out actualResults);

            if (success && actualResults < maxResults)
            {
                maxResults = actualResults;
            }

            return (int)Math.Ceiling(maxResults / REALITEMSPERPAGE);
        }

        public static ExchangeRateResponse GetExchangeRates()
        {
            WebRequest request = WebRequest.Create(OERURL + OERAPPID);

            try
            {
                Stream stream = request.GetResponse().GetResponseStream();

                string result = stream.GetStringValue();

                return Codec.DecodeExchangeRateResponse(result);
            }
            catch (WebException e)
            {
                return new ExchangeRateResponse { ErrorMessage = e.Message };
            }
        }   
        
    }

    public static class Extensions
    {
        public static string GetStringValue (this Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}