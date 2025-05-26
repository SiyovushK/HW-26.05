using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository(DataContext context) : IBaseRepository<Order, int>
{
    public async Task<int> AddAsync(Order entity)
    {
        await context.AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Order entity)
    {
        context.Remove(entity);
        return await context.SaveChangesAsync();

    }

    public Task<IQueryable<Order>> GetAll()
    {
        var order = context.Orders.AsQueryable();
        return Task.FromResult(order);
    }

    public async Task<Order?> GetByAsync(int id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order;
    }

    public async Task<int> UpdateAsync(Order entity)
    {
        context.Update(entity);
        return await context.SaveChangesAsync();
    }

}
