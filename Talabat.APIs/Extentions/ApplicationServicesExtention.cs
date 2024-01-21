using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            #region DI of Repositories classes by Interfaces in controllers
            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();


            Services.AddScoped<IOrderSevices, OrderService>();
            Services.AddScoped<IUniteOfWork, UniteOfWork>();
            Services.AddScoped<IPaymentService, PaymentService>();
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));

            #endregion

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count() > 0)
                                                         .SelectMany(a => a.Value.Errors)
                                                         .Select(r => r.ErrorMessage).ToList();
                    var ValidationErrors = new ApiValidationErrorResponses()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrors);
                };


            });

           
            return Services;
        }
    }
}
