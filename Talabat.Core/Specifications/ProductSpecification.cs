using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams productSpec)                              //GetAll
               : base(p=>
                (string.IsNullOrEmpty(productSpec.Search)|| p.Name.ToLower().Contains(productSpec.Search.ToLower()))&&
                (!productSpec.BrandId.HasValue||p.ProductBrandId== productSpec.BrandId) &&
                (!productSpec.TypeId.HasValue||p.ProductTypeId== productSpec.TypeId)    
               )
        {
            Includes.Add(p => p.ProductBrand); 
            Includes.Add(p => p.ProductType);
            

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort.ToLower())
                {
                    case "priceascending":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedescending":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            ApplyPagination(productSpec.PageSize * (productSpec.PageIndex-1), productSpec.PageSize);
           

            

        }
        public ProductSpecification(int id) :base(p=>p.Id==id)          //GetById
        {
            Includes.Add(p => p.ProductBrand); 
            Includes.Add(p => p.ProductType); 
        }
    }
}
