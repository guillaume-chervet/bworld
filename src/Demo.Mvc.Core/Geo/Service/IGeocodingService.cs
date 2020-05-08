using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Geo.Models;

namespace Demo.Mvc.Core.Geo.Service
{
    public interface IGeocodingService
    {
        Task<IList<RootGeo>> ReverseAsync(string address);
        Task<IList<RootGeo>> ReverseAsync(AddressInput address);
        Task<RootIpGeo> ReverseFromIpAsync(string ip);
    }
}