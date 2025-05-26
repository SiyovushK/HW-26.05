using System.Security.Authentication.ExtendedProtection;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(DataContext context) : IBaseRepository<Product, int>
{
    public async Task<int> AddAsync(Product entity)
    {
        await context.AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Product entity)
    {
        context.Remove(entity);
        return await context.SaveChangesAsync();
    }

    public  Task<IQueryable<Product>> GetAll()
    {
        var product = context.Products.AsQueryable();
        return Task.FromResult(product);
    }

    public async Task<Product?> GetByAsync(int id)
    {
        var prod = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        return prod;
    }

    public async Task<int> UpdateAsync(Product entity)
    {
        context.Update(entity);
        return await context.SaveChangesAsync();
    }

}
