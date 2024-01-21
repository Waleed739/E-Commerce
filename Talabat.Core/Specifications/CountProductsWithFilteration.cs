using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class CountProductsWithFilteration : BaseSpecification<Product>
    {

        public CountProductsWithFilteration(ProductSpecParams productSpec)                              //GetAll
               : base(p =>
                               (string.IsNullOrEmpty(productSpec.Search) || p.Name.ToLower().Contains(productSpec.Search.ToLower())) &&
                               (!productSpec.BrandId.HasValue || p.ProductBrandId == productSpec.BrandId) &&
                               (!productSpec.TypeId.HasValue || p.ProductTypeId == productSpec.TypeId))

        {




        }
    }
}
