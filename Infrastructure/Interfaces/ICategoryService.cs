using Domain.DTOs.Category;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICategoryService
{
    Task<Response<List<GetCategoryDto>>> GetAllAsync(CategoryFilter filter);
    Task<Response<GetCategoryDto>> CraeteAsync(CreateCategoryDto request); 
    Task<Response<GetCategoryDto>> GetByIdAsync(int Id);
    Task<Response<GetCategoryDto>> UpdateAsync(int Id, UpdateCategoryDto request);
    Task<Response<string>> DeleteAsync(int Id);
}