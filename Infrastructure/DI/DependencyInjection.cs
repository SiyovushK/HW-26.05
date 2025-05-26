using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DI;

public static class DependencyInjection
{
   public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBaseRepository<Order, int>, IOrderRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordHasher<Order>, PasswordHasher<Order>>();
    }
}