using Discount.Api.Data;
using Discount.Api.Entities;
using Discount.Api.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace Discount.Api.Extensions
{
    public static class HostExtensions 
    {
        /// <summary>
        /// using (var scope = host.Services.CreateScope())
        /// host.Services: host refers to an instance of IHost Services is
        // a property of IHost that provides access to the application's service container or dependency injection (DI) container.
        // CreateScope(): This method creates a new scope within the DI container. Scopes are used to group a set of services that share the same lifetime and configuration.
        // They also manage the lifetime of transient services within the scope of the operation.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="retry"></param>
        /// <returns>IHost</returns>
        public static IHost MigrateDatabase<TContext>(this IHost host,string tablename  ,int? retry = 0)
        {
            int retryForAvailability = retry ?? retry.Value;
            
            using (var scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                //  get and request from  DI container 
                IConfiguration configuration = services.GetRequiredService<IConfiguration>(); 
                ILogger<TContext>  logger = services.GetRequiredService<ILogger<TContext>>();
                IDiscountDbContext NpgsqlDb  = services.GetRequiredService<IDiscountDbContext>();
                try
                {
                    logger.LogInformation("Migrating Postgresql Database . ");

                    using var connection =(NpgsqlConnection)NpgsqlDb.DbConnection;
                    connection.Open();
                    using var command = NpgsqlDb.Command;

                    command.CommandText = $"DROP TABLE IF EXIST {tablename} ";
                    int affectedCommand = command.ExecuteNonQuery();

                    command.CommandText = @"create table Cupon(
                                           ID serial primary key Not null ,
                                           ProductName varchar(24) not null , 
                                           Description text ,
                                            Amount int  )";
                    affectedCommand = command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                    affectedCommand = command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                    affectedCommand = command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database.");

                }
                catch (NpgsqlException ex )
                {
                    //  here we will use Polly  Fault Handling / Resiliency / Retry Policy / Circuit Breaker => later 


                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(
                            host,ReflectionHelper.GetClassName<Coupon>() ,retryForAvailability);
                    }
                }
            }
            return host;
        }



    }
}
