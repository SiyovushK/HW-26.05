using System.Net;
using AutoMapper;
using Domain.DTOs.Order;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class OrderService(IBaseRepository<Order, int> repository, IMapper mapper) : IOrderService
{
    public async Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request)
    {
        var order = mapper.Map<Order>(request);

        var result = await repository.AddAsync(order);
        if (result == 0)
        {
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "order not added!");
        }

        var data = mapper.Map<GetOrderDto>(order);

        return new Response<GetOrderDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var order = await repository.GetByAsync(Id);
        if (order == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, $"order with id {Id} not found");
        }

        var result = await repository.DeleteAsync(order);
        if (result == 0)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Order not deleted!");
        }

        return new Response<string>("Order deleted successfuly");
    }

    public async Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

        var order = await repository.GetAll();

        if (filter.UserId.HasValue)
        {
            order = order.Where(o => o.UserId == filter.UserId);
        }
        if (filter.ProductId != null)
        {
            order = order.Where(o => o.ProductId == filter.ProductId);

        }
        if (filter.From.HasValue)
        {
            order = order.Where(o => o.OrderDate <= filter.From.Value);
        }
        if (filter.To.HasValue)
        {
            order = order.Where(o => o.OrderDate >= filter.To.Value);
        }
        if (filter.Status != null)
        {
            order = order.Where(o => o.Status == filter.Status);
        }

        var mapped = mapper.Map<List<GetOrderDto>>(order);

        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetOrderDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
    }

    public async Task<Response<GetOrderDto>> GetByIdAsync(int Id)
    {
        var order = await repository.GetByAsync(Id);
        if (order == null)
        {
            return new Response<GetOrderDto>(HttpStatusCode.NotFound, $"order with id {Id} not found");
        }

        var data = mapper.Map<GetOrderDto>(order);

        return new Response<GetOrderDto>(data);
    }

    public async Task<Response<GetOrderDto>> UpDateAsync(int Id, UpdateOrderDto request)
    {
        var order = await repository.GetByAsync(Id);
        if (order == null)
        {
            return new Response<GetOrderDto>(HttpStatusCode.NotFound, $"order with id {Id} not found");
        }

        order.Quantity = request.Quantity;
        order.Status = request.Status;

        var result = await repository.UpdateAsync(order);
        if (result == 0)
        {
            return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "order not deleted!");
        }

        var data = mapper.Map<GetOrderDto>(order);

        return new Response<GetOrderDto>(data);
    }

}
