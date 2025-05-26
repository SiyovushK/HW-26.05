using Domain.DTOs.User;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<GetUserDto>> CreateAsync(CreateUserDto request);
    Task<Response<string>> DeleteAsync(int Id);
    Task<Response<List<GetUserDto>>> GetAllAsync(ValidFilter filter);
    Task<Response<GetUserDto>> GetByIdAsync(int Id);
    Task<Response<GetUserDto>> UpdateAsync(int Id, UpdateUserDto request);
}