using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggreation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
    public class OrderService : IOrderSevices
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUniteOfWork uniteOfWork;
        private readonly IPaymentService paymentService;

        //private readonly IGenericRepository<Product> productRepo;
        //private readonly IGenericRepository<DeliveryMethod> deliveryRepo;
        //private readonly IGenericRepository<Order> orderRepo;

        public OrderService(IBasketRepository basketRepository, IUniteOfWork uniteOfWork,IPaymentService paymentService
          /* IGenericRepository<Product> productRepo,IGenericRepository<DeliveryMethod> deliveryRepo, IGenericRepository<Order> orderRepo*/)
        {
            this.basketRepository = basketRepository;
            this.uniteOfWork = uniteOfWork;
            this.paymentService = paymentService;
            //this.productRepo = productRepo;
            //this.deliveryRepo = deliveryRepo;
            //this.orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1- Get Basket from BasketRepository
            var basket = await basketRepository.GetBasketAsync(basketId);

            //2- Get Selected Items at  Basket from ProductRepo


            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await uniteOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem()
                    {
                        Product = productItemOrdered,
                        Price = product.Price,
                        Quantity = item.Quantity
                    };
                    orderItems.Add(orderItem);
                }

            }

            // 3- Calculate SubTotal
            var subTotal = orderItems.Sum(Item => Item.Price * Item.Quantity);

            //4- Get DeliveryMethod From DeliveryMethod Repo
            var deliveryMethod = await uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //5- Create Order
            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var existOrder = await uniteOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            if(existOrder is not null)
            {
                uniteOfWork.Repository<Order>().Delete(existOrder);

                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            //6- Add Order Locally
            await uniteOfWork.Repository<Order>().Add(order);

            //7-Save Changes to Orders Table in DataBase
            var result = await uniteOfWork.Complete();

            if (result < 1) return null;

            return order;




        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await uniteOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification( buyerEmail, orderId);
            var order = await uniteOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }
        
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail) ;
            var orders =await uniteOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
