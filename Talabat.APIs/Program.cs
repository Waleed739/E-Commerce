using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Controllers;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helper;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Identity;
using Talabat.Repository.Identity.Data;
using Talabat.Repository.Identity.Data.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.

            builder.Services.AddControllers();
            #region CLR Generate object of dbContext <StoreContext> DI

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            #endregion

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddApplicationServices();
            //ApplicationServicesExtention.AddApplicationServices(builder.Services);

            IdentityServicesExtentions.AddIdentityServices(builder.Services, builder.Configuration);
            //builder.Services.AddIdentityServices(builder.configuration);



            builder.Services.AddSwaggerServices();      //extention


            var app = builder.Build();

            #region UpdateDataBase
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var LogggerFactory = services.GetRequiredService<ILoggerFactory>();
            
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();    // ask CLR to Create object from StoreContext Explicitly
                await dbContext.Database.MigrateAsync();                        //Update dataBase
               await  StoreContextDataSeed.SeedAsync(dbContext);


                var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();                        //Update dataBase

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = LogggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error during Apply Migration");
            }



            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare();          //extention
            }
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseHttpsRedirection();

           

            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

	    app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}