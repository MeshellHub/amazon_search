using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonApp.Models.Networking.Responses
{
    public class ExchangeRateResponse : Response
    {
        public string Disclaimer { get; set; }

        public string License { get; set; }

        public long Timestamp { get; set; }

        public string Base { get; set; }

        public List<ExchangeRate> Rates { get; set; }
    }

    //{
    //  "disclaimer": "Exchange rates provided for informational purposes only 
    //                 and do not constitute financial advice of any kind. 
    //                 Although every attempt is made to ensure quality, no guarantees are made of accuracy, 
    //                 validity, availability, or fitness for any purpose. 
    //                 All usage subject to acceptance of Terms: https://openexchangerates.org/terms/",
    //  "license": "Data sourced from various providers; resale prohibited; no warranties given of any kind. 
    //              All usage subject to License Agreement: https://openexchangerates.org/license/",
    //  "timestamp": 1456329608,
    //  "base": "USD",
    //  "rates": {
    //    "AED": 3.673014,
    //    "AFN": 68.565,
    //    "ALL": 125.548799,
    //    "AMD": 493.425001,
    //    ...
    //  }
    //}
}