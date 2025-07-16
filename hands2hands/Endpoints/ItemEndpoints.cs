using hands2hands.DTOs;
using hands2hands.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        app.MapGet("/items",
            async ([FromServices] IItemService svc) =>
                Results.Ok(await svc.GetAllAsync()));

        app.MapGet("/items/{id:guid}",
            async (Guid id, [FromServices] IItemService svc) =>
                await svc.GetByIdAsync(id) is ItemResponseDto dto
                    ? Results.Ok(dto)
                    : Results.NotFound());

        app.MapPost("/items",
            async ([FromBody] ItemRequestDto dto,
                   [FromServices] IItemService svc) =>
            {
                var created = await svc.CreateAsync(dto);
                return Results.Created($"/items/{created.Id}", created);
            })
           .Accepts<ItemRequestDto>("application/json")
           .Produces<ItemResponseDto>(StatusCodes.Status201Created);

        app.MapPut("/items/{id:guid}",
            async (Guid id, [FromBody] ItemRequestDto dto, [FromServices] IItemService svc) =>
            {
                try
                {
                    var updated = await svc.UpdateAsync(id, dto);
                    return Results.Ok(updated);
                }
                catch (Exception ex)
                {
                    return Results.NotFound(new { message = ex.Message });
                }
            })
           .Accepts<ItemRequestDto>("application/json")
           .Produces<ItemResponseDto>(StatusCodes.Status200OK);

        app.MapDelete("/items/{id:guid}",
            async (Guid id, [FromServices] IItemService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

        app.MapGet("/categories/{categoryId:int}/items",
            async (int categoryId, [FromServices] IItemService svc) =>
                Results.Ok(await svc.GetByCategoryIdAsync(categoryId)));
    }
}
