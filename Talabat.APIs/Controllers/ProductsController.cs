using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
   
    public class ProductsController : ApiBaseController
    {

        //private readonly IGenericRepository<Product> genericRepos;
        //private readonly IGenericRepository<ProductType> typeRepo;
        //private readonly IGenericRepository<ProductBrand> brandRepo;
        private readonly IUniteOfWork uniteOfWork;
        private readonly IMapper mapper;

        //public ProductsController(IGenericRepository<Product>  genericRepos,
        //                          IGenericRepository<ProductType> typeRepo,
        //                          IGenericRepository<ProductBrand>brandRepo, IMapper mapper)
        public ProductsController(IUniteOfWork uniteOfWork,IMapper mapper)
        {
            //this.genericRepos = genericRepos;
            //this.typeRepo = typeRepo;
            //this.brandRepo = brandRepo;
            this.uniteOfWork = uniteOfWork;
            this.mapper = mapper;
        }
        
        [HttpGet]

        public async Task<ActionResult<Pagination<ProductRetunDto>>> GetAllProducts([FromQuery]ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            var products =  await uniteOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = mapper.Map<IReadOnlyList<ProductRetunDto>>(products);
            var countSpec = new CountProductsWithFilteration(productSpec);
            var count = await uniteOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
           //var products =  await genericRepos.GetAllAsync();
            return Ok(new Pagination<ProductRetunDto>(productSpec.PageIndex, productSpec.PageSize,count,data));
        }

        [ProducesResponseType(typeof(ProductRetunDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponses),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductRetunDto >> GetProduct(int id)
        {
            var spec = new ProductSpecification(id);

            var product = uniteOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null)
                return NotFound(new ApiErrorResponses(404));

            var moppedProduct = mapper.Map<ProductRetunDto>(product);

            //var product =  await genericRepos.GetByIdAsync(id);
            return Ok(moppedProduct);

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrandsAsync()
        {
            var brands = await uniteOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypesAsync()
        {
            var types = await uniteOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
