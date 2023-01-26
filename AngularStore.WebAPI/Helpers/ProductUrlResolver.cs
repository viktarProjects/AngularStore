using AngularStore.Core.Entities;
using AngularStore.WebAPI.Dto_s;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace AngularStore.WebAPI.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product,ProductDto,string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source,ProductDto destination,string destMember,ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _configuration["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
