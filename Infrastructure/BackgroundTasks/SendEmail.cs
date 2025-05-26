using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundTasks;

public class SendEmail(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Order, int>>();

            var orders = await dataContext.Orders.ToListAsync(stoppingToken);


            foreach (var order in orders)
            {
                var now = DateTime.UtcNow;

                if (order.DeliveryDeadline < DateTime.Now && !order.IsDiscountApplied)
                {
                    var updateOrder = order;
                    updateOrder.IsDiscountApplied = true;
                    updateOrder.Price = order.Price - (order.Price * 0.2);

                    await orderRepository.UpdateAsync(updateOrder);

                    var emailDto = new EmailDTO()
                    {
                        To = order.CustomerEmail,
                        Subject = "Delivery overdue",
                        Body = $"Sorry, your order is beign late.\n\n A discount of 20% has been applied to your order"
                    };

                    await emailService.SendEmailAsync(emailDto);
                }
                else
                {
                    var emailDto = new EmailDTO()
                    {
                        To = order.CustomerEmail,
                        Subject = "Delivery info",
                        Body = $"Your order is on the way."
                    };

                    await emailService.SendEmailAsync(emailDto);
                }
            }
    
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }

    }
}