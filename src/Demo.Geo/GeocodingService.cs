using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Demo.Geo.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Geo
{
    public class GeocodingService : IGeocodingService
    {
        private readonly ILogger<GeocodingService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConcurrentDictionary<string, RootIpGeo> _reverseFromIpDico = new ConcurrentDictionary<string, RootIpGeo>();
        public const string Geocoding = "Geocoding";

        public GeocodingService(ILogger<GeocodingService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IList<RootGeo>> ReverseAsync(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                return null;
            }

            address = address.Replace(" ", "+");
            var lines = address.Split(',');

            if (lines.Length == 3)
            {
                return await ReverseAsync(new AddressInput {City = lines[2], PostalCode = lines[1], Street = lines[0]});
            }

            return null;
        }

        private  HttpClient GetWebClient()
        {
            return _clientFactory.CreateClient(Geocoding);
        }

        public async Task<IList<RootGeo>> ReverseAsync(AddressInput address)
        {
            var street = address.Street.Trim(' ');
            var postalcode = address.PostalCode.Trim(' ');
            var city = address.City.Trim(' ');

            const string uri =
                "https://nominatim.openstreetmap.org/search?street={0}&city={1}&postalcode={2}&country=france&format=json&polygon=1&addressdetails=1";

            var queryUrl = String.Format(uri, street, city, postalcode);

            var webClient = GetWebClient();
            {
                var uriObj = new Uri(queryUrl);
                var response = await webClient.GetAsync(uriObj);
                var htmlResult = await response.Content.ReadAsStringAsync();
                var addressResults = JsonConvert.DeserializeObject<List<RootGeo>>(htmlResult);

                if (addressResults == null || addressResults.Count <= 0)
                {
                    _logger.LogWarning("No address found for: " + queryUrl);
                }

                return addressResults;
            }
        }

        public async Task<RootIpGeo> ReverseFromIpAsync(string ip)
        {
            try
            {
                if (ip.Contains("::"))
                {
                    return null;
                }

                if (_reverseFromIpDico.ContainsKey(ip))
                {
                    return _reverseFromIpDico[ip];
                }

                const string uri = "http://ip-api.com/json/{0}";
                var queryUrl = String.Format(uri, ip);

                var webClient = GetWebClient();
                {
                    var uriObj = new Uri(queryUrl);
                    var response = await webClient.GetAsync(uriObj);
                    var htmlResult = await response.Content.ReadAsStringAsync();
                    var jsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    var addressResults = JsonConvert.DeserializeObject<RootIpGeo>(htmlResult, jsonSerializerSettings);
                    if (!_reverseFromIpDico.ContainsKey(ip))
                    {
                        _reverseFromIpDico.TryAdd(ip, addressResults);
                    }
                    return addressResults;
                }
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method: ReverseFromIp");
            }
            return null;
        }
    }
}