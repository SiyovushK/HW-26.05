using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Interfaces;
using System.Net;

namespace Infrastructure.Services;

public class UserService(IBaseRepository<User, int> repository, IMapper mapper) : IUserService
{
    public async Task<Response<GetUserDto>> CreateAsync(CreateUserDto request)
    {
        var user = mapper.Map<User>(request);

        var result = await repository.AddAsync(user);

        if (result == 0)
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not added!");

        var data = mapper.Map<GetUserDto>(user);
        
        return new Response<GetUserDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var user = await repository.GetByAsync(Id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, $"User with id {Id} not found");

        var result = await repository.DeleteAsync(user);
        if (result == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "User not deleted!");

        return new Response<string>("User deleted successfully");
    }

    public async Task<Response<List<GetUserDto>>> GetAllAsync(ValidFilter filter)
    {
        var users = await repository.GetAll();

        var mapped = mapper.Map<List<GetUserDto>>(users);
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return new PagedResponse<List<GetUserDto>>(data, filter.PageNumber, filter.PageSize, totalRecords);
    }

    public async Task<Response<GetUserDto>> GetByIdAsync(int Id)
    {
        var user = await repository.GetByAsync(Id);
        if (user == null)
            return new Response<GetUserDto>(HttpStatusCode.NotFound, $"User with id {Id} not found");

        var data = mapper.Map<GetUserDto>(user);
        return new Response<GetUserDto>(data);
    }

    public async Task<Response<GetUserDto>> UpdateAsync(int Id, UpdateUserDto request)
    {
        var user = await repository.GetByAsync(Id);
        if (user == null)
            return new Response<GetUserDto>(HttpStatusCode.NotFound, $"User with id {Id} not found");

        user.Username = request.UserName;
        user.Email = request.Email;
        user.PasswordHash = request.Password;

        var result = await repository.UpdateAsync(user);
        if (result == 0)
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not updated!");

        var data = mapper.Map<GetUserDto>(user);
        return new Response<GetUserDto>(data);
    }
}
