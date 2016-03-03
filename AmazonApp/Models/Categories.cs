using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonApp.Models
{
    public class Categories
    {
        public static List<Category> Get()
        {
            return new List<Category> {
                new Category { Value = "All", Text = "All" },
                new Category { Value = "Apparel", Text = "Apparel" },
                new Category { Value = "Appliances", Text = "Appliances" },
                new Category { Value = "ArtsAndCrafts", Text = "Arts &amp; Crafts" },
                new Category { Value = "Automotive", Text = "Automotive" },
                new Category { Value = "Baby", Text = "Baby" },
                new Category { Value = "Beauty", Text = "Beauty" },
                // Additional info on blended searches:
                // http://docs.aws.amazon.com/AWSECommerceService/latest/DG/BlendedSearches.html
                //new Category { Value = "Blended", Text = "Blended" },
                new Category { Value = "Books", Text = "Books" },
                new Category { Value = "Classical", Text = "Classical" },
                new Category { Value = "Collectibles", Text = "Collectibles" },
                new Category { Value = "DVD", Text = "DVD" },
                new Category { Value = "DigitalMusic", Text = "Digital Music" },
                new Category { Value = "Electronics", Text = "Electronics" },
                new Category { Value = "Fashion", Text = "Fashion" },
                new Category { Value = "FashionBaby", Text = "&nbsp;&nbsp;&nbsp; Baby" },
                new Category { Value = "FashionBoys", Text = "&nbsp;&nbsp;&nbsp; Boys'" },
                new Category { Value = "FashionGirls", Text = "&nbsp;&nbsp;&nbsp; Girls'" },
                new Category { Value = "FashionMen", Text = "&nbsp;&nbsp;&nbsp; Men's" },
                new Category { Value = "FashionWomen", Text = "&nbsp;&nbsp;&nbsp; Women's" },
                new Category { Value = "GiftCards", Text = "Giftcards" },
                new Category { Value = "GourmetFood", Text = "Gourmet Food" },
                new Category { Value = "Grocery", Text = "Groceries" },
                new Category { Value = "HealthPersonalCare", Text = "Health & Personal Care" },
                new Category { Value = "HomeGarden", Text = "Home Garden" },
                new Category { Value = "Industrial", Text = "Industrial" },
                new Category { Value = "Jewelry", Text = "Jewelry" },
                new Category { Value = "KindleStore'> Kindle Store" },
                new Category { Value = "Kitchen", Text = "Kitchen" },
                new Category { Value = "LawnAndGarden", Text = "Lawn &amp; Garden" },
                new Category { Value = "Luggage", Text = "Luggage" },
                new Category { Value = "MP3Downloads", Text = "MP3 Downloads" },
                new Category { Value = "Magazines", Text = "Magazines" },
                new Category { Value = "Marketplace", Text = "Marketplace" },
                new Category { Value = "Miscellaneous", Text = "Miscellaneous" },
                new Category { Value = "MobileApps", Text = "Mobile Applications" },
                new Category { Value = "Movies", Text = "Movies" },
                new Category { Value = "Music", Text = "Music" },
                new Category { Value = "MusicTracks", Text = "Music Tracks" },
                new Category { Value = "MusicalInstruments", Text = "Musical Instruments" },
                new Category { Value = "OfficeProducts", Text = "OfficeProducts" },
                new Category { Value = "OutdoorLiving", Text = "Outdoor Living" },
                new Category { Value = "PCHardware", Text = "PC Hardware" },
                new Category { Value = "Pantry", Text = "Pantry" },
                new Category { Value = "PetSupplies", Text = "Pet Supplies" },
                new Category { Value = "Photo", Text = "Photo" },
                new Category { Value = "Shoes", Text = "Shoes" },
                new Category { Value = "Software", Text = "Software" },
                new Category { Value = "SportingGoods", Text = "Sporting Goods" },
                new Category { Value = "Tools", Text = "Tools" },
                new Category { Value = "Toys", Text = "Toys" },
                new Category { Value = "UnboxVideo", Text = "Unbox Video" },
                new Category { Value = "VHS", Text = "VHS" },
                new Category { Value = "Video", Text = "Video" },
                new Category { Value = "VideoGames", Text = "Video Games" },
                new Category { Value = "Watches", Text = "Watches" },
                new Category { Value = "Wine", Text = "Wine" },
                new Category { Value = "Wireless", Text = "Wireless" },
                new Category { Value = "WirelessAccessories", Text = "Wireless Accessories" }
            };
        }
    }
}