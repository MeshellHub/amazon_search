using AmazonApp.Models.Networking.Responses;
using AmazonApp.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Json;

namespace AmazonApp.Models.Networking
{
    public class Codec
    {
        public static List<Product> DecodeProducts (Items items)
        {
            List<Product> products = new List<Product>();

            foreach (Item item in items.Item)
            {
                Product product = DecodeProduct(item);

                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }

        static Product DecodeProduct(Item item)
        {
            if (item.ItemAttributes == null)
            {
                // can be null in certain situations, e.g. Blended searches
                return null;
            }

            string url = "none";
            Price price = new Price { FormattedPrice = "" };

            if (item.MediumImage != null && item.MediumImage.URL != null)
            {
                url = item.MediumImage.URL;
            }
            
            //if (item.ItemAttributes.ListPrice != null)
            //{
            //    price = item.ItemAttributes.ListPrice;
            //}

            if (item.OfferSummary != null && item.OfferSummary.LowestNewPrice != null)
            {
                price = item.OfferSummary.LowestNewPrice;
            }

            Product product = new Product
            {
                Title = item.ItemAttributes.Title,
                Price = price,
                ASIN = item.ASIN,
                DetailUrl = item.DetailPageURL,
                ImageUrl = url,
            };

            return product;
        }

        const string DISCLAIMER = "disclaimer";
        const string LICENSE = "license";
        const string TIMESTAMP = "timestamp";
        const string BASE = "base";
        const string RATES = "rates";

        public static ExchangeRateResponse DecodeExchangeRateResponse (string result)
        {
            ExchangeRateResponse response = new ExchangeRateResponse();

            JsonValue json;

            try
            {
                json = JsonValue.Parse(result);
            }
            catch
            {
                response.ErrorMessage = "Encountered error parsing data";
                return response;
            }

            response.Disclaimer = GetStringValue(json, DISCLAIMER, "");

            response.License = GetStringValue(json, LICENSE, "");

            response.Timestamp = GetLongValue(json, TIMESTAMP, 0);

            response.Base = GetStringValue(json, BASE, "");

            if (json.ContainsKey(RATES))
            {
                response.Rates = DecodeRates(json[RATES]);
            }
            return response;
        }

        static List<ExchangeRate> DecodeRates (JsonValue json)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach (KeyValuePair<string, JsonValue> child in json)
            {
                ExchangeRate rate = DecodeRate(child);

                if (rate != null)
                {
                    rates.Add(rate);
                }
            }
            return rates;
        }

        static ExchangeRate DecodeRate (KeyValuePair<string, JsonValue> item)
        {
            try
            {
                double rate = double.Parse(item.Value.ToString(), CultureInfo.CurrentUICulture);
                return new ExchangeRate { Code = item.Key, Rate = rate };
            }
            catch
            {
                return null;
            }
        }

        static string GetStringValue(JsonValue json, string key, string defaultValue)
        {
            if (!json.ContainsKey(key))
            {
                return defaultValue;
            }

            if (json[key] == null)
            {
                return defaultValue;
            }

            return (string)json[key];
        }

        static long GetLongValue(JsonValue json, string key, long defaultValue)
        {
            if (!json.ContainsKey(key))
            {
                return defaultValue;
            }

            if (json[key] == null)
            {
                return defaultValue;
            }

            return (long)json[key];
        }

        static double GetDoubleValue(JsonValue json, string key, double defaultValue)
        {
            if (!json.ContainsKey(key))
            {
                return defaultValue;
            }

            if (json[key] == null)
            {
                return defaultValue;
            }

            return (double)json[key];
        }
    }
}