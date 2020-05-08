using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Geo.Models;
using Demo.Mvc.Core.Geo.Service;

namespace Demo.Mvc.Core.Geo.Core
{
    public class GetGeoCommand : Command<GetGeoInput, CommandResult<dynamic>>
    {
        private readonly IGeocodingService _geocodingService;

        public GetGeoCommand(IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
        }

        protected override async Task ActionAsync()
        {
            Result.Data = await _geocodingService.ReverseAsync(new AddressInput
            {
                Street = Input.Street,
                PostalCode = Input.PostalCode,
                City = Input.City
            });
        }
    }
}