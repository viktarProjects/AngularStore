using AngularStore.Core.Entities;
using AngularStore.Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        private static Expression<Func<Product, bool>> GetCountOfFilteredProducts(ProductSpecParams specParams)
        {
            return x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search))
            && (!specParams.Brands!.Any() || specParams.Brands!.Contains(x.ProductBrand.Name))
            && (!specParams.Types!.Any() || specParams.Types!.Contains(x.ProductType.Name));
        }

        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
            : base(GetCountOfFilteredProducts(productParams))
        {

        }
    }
}
