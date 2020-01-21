using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Geo;
using Demo.Geo.Models;

namespace Demo.Business.Command.Geo
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