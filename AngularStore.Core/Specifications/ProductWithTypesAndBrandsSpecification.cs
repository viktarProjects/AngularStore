using AngularStore.Core.Entities;
using System.Linq.Expressions;

namespace AngularStore.Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        private static Expression<Func<Product, bool>> GetProductsFilter(ProductSpecParams specParams)
        {
            return x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search))
            && (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId)
            && (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId)
            && (string.IsNullOrEmpty(specParams.DescriptionSearch) || x.Description.ToLower().Contains(specParams.DescriptionSearch));
        }

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productParams) : base(GetProductsFilter(productParams))
        {
            AddInclude(x => x.ProductType);

            AddInclude(x => x.ProductBrand);

            ApplyPaging(productParams.PageSize * (productParams.PageNumber - 1), productParams.PageSize);

            ApplySorting(productParams);
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public void ApplySorting(ProductSpecParams productParams)
        {
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }
    }
}
