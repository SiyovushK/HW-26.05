using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(DataContext context) : IBaseRepository<Category, int>
{
    public async Task<int> AddAsync(Category entity)
    {
        await context.AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Category entity)
    {
        context.Remove(entity);
        return await context.SaveChangesAsync();
    }

    public Task<IQueryable<Category>> GetAll()
    {
        var category = context.Categories.AsQueryable();
        return Task.FromResult(category);
    }

    public async Task<Category?> GetByAsync(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<int> UpdateAsync(Category entity)
    {
        context.Update(entity);
        return await context.SaveChangesAsync();
    }

}
