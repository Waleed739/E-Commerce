using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>>GetCustomerBasket(string Id)
        {
            var basket =await basketRepository.GetBasketAsync(Id);
            
            return basket == null ? new CustomerBasket(Id) : basket;
        }

        [HttpPost]
        public async Task <ActionResult<CustomerBasketDto>> CreateOrUpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
            var createOrUpdateBasket= await basketRepository.UpdateBasketAsync(basket);
            return createOrUpdateBasket is null ? BadRequest(new ApiErrorResponses(400)) : Ok(createOrUpdateBasket);
        }
        [HttpDelete]

        public async Task <ActionResult<bool>> DeleteBasket(string Id)
        {
            return await basketRepository.DeleteBasketAsync(Id); 
        }
        
    }
}
