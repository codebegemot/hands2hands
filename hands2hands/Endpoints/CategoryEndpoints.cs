using System;
using hands2hands.DTOs;
using hands2hands.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hands2hands.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories",
            async ([FromServices] ICategoryService svc) =>
                Results.Ok(await svc.GetAllAsync()));

        app.MapGet("/categories/{id:int}",
            async (int id, [FromServices] ICategoryService svc) =>
                await svc.GetByIdAsync(id) is CategoryResponseDto dto
                    ? Results.Ok(dto)
                    : Results.NotFound());

        app.MapPost("/categories",
            async ([FromBody] CategoryRequestDto dto,
                   [FromServices] ICategoryService svc) =>
        {
            var created = await svc.CreateAsync(dto);
            return Results.Created($"/categories/{created.Id}", created);
        })
       .Accepts<CategoryRequestDto>("application/json")
       .Produces<CategoryResponseDto>(StatusCodes.Status201Created);

        app.MapPut("/categories/{id:int}",
            async (int id, [FromBody] CategoryRequestDto dto, [FromServices] ICategoryService svc) =>
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
        .Accepts<CategoryRequestDto>("application/json")
        .Produces<CategoryResponseDto>(StatusCodes.Status200OK);

        app.MapDelete("/categories/{id:int}",
            async (int id, [FromServices] ICategoryService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });
    }
}
