using AmazonApp.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonApp.Models
{
    public class Product
    {
        const string TOOLOW = "Too low to display";

        public string ASIN { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string DetailUrl { get; set; }

        public Price Price { get; set; }

        public bool HasPrice
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Price.FormattedPrice);
            }
        }

        public string FormattedPrice
        {
            get
            {
                if (!HasPrice)
                {
                    return "";
                }

                if (IsPriceTooLow)
                {
                    return TOOLOW;
                }

                return PriceNumericValue + " " + Price.CurrencyCode;
            }
        }

        public string PriceNumericValue
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Price.FormattedPrice))
                {
                    return "";
                }

                if (IsPriceTooLow)
                {
                    return Price.FormattedPrice;
                }

                return Price.FormattedPrice.Substring(1);
            }
        }

        public bool IsPriceTooLow
        {
            get
            {
                return Price.FormattedPrice == TOOLOW;
            }
        }

    }
}