﻿using MultiMiner.Coin.Api;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace MultiMiner.CoinChoose.Api
{
    public class ApiContext : IApiContext
    {
        public IEnumerable<CoinInformation> GetCoinInformation(string userAgent = "",
            BaseCoin profitabilityBasis = BaseCoin.Bitcoin)
        {
            WebClient client = new WebClient();
            if (!string.IsNullOrEmpty(userAgent))
                client.Headers.Add("user-agent", userAgent);

            string apiUrl = GetApiUrl(profitabilityBasis);

            string jsonString = client.DownloadString(apiUrl);
            JArray jsonArray = JArray.Parse(jsonString);

            List<CoinInformation> result = new List<CoinInformation>();

            foreach (JToken jToken in jsonArray)
            {
                CoinInformation coinInformation = new CoinInformation();
                coinInformation.PopulateFromJson(jToken);
                if (coinInformation.Difficulty > 0)
                    //only add coins with valid info since the user may be basing
                    //strategies on Difficulty
                    result.Add(coinInformation);
            }

            return result;
        }

        public string GetApiUrl(BaseCoin profitabilityBasis)
        {
            string apiUrl = "http://www.coinchoose.com/api.php";
            if (profitabilityBasis == BaseCoin.Litecoin)
                apiUrl = apiUrl + "?base=LTC";
            return apiUrl;
        }

        public string GetInfoUrl(BaseCoin profitabilityBasis)
        {
            if (profitabilityBasis == BaseCoin.Litecoin)
                return "http://coinchoose.com/litecoin.php";
            else
                return "http://coinchoose.com/index.php";
        }
        
        public string GetApiName()
        {
            return "CoinChoose.com";
        }
    }
}
