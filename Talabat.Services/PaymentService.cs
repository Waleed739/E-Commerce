using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggreation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUniteOfWork uniteOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository, IUniteOfWork uniteOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.uniteOfWork = uniteOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSetting:SecretKey"];
            var basket = await basketRepository.GetBasketAsync(BasketId);
            if (basket == null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingCost = deliveryMethod.Cost;
            }

            if (basket?.Items?.Count > 0) //Make sure that the items in he basket are available products
            {
                foreach (var item in basket.Items)
                {
                    var product = await uniteOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)  //Make sure that the price of the baslet item is the same of product price 
                        item.Price = product.Price;

                }
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;


            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount =(long) (basket.Items.Sum(Item => Item.Price * Item.Quantity*100) + shippingPrice*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            
       // update an existing payment intent
            else  
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(basket.Items.Sum(Item => Item.Price * Item.Quantity * 100) + shippingPrice * 100),

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }




            await basketRepository.UpdateBasketAsync(basket);

            return basket;

        }

        
    }
}
