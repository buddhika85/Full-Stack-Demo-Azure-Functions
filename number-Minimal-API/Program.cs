using Microsoft.EntityFrameworkCore;
using number_Minimal_API.Data;
using number_Minimal_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("NumbersDb"));


builder.Services.AddEndpointsApiExplorer();     // open API middleware will be enabled due to this
builder.Services.AddSwaggerGen();







var app = builder.Build();






app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();



#region EndPoints


app.MapGet("api/numbers", async (AppDbContext context) =>
{
    var items = await context.NumItems.ToListAsync();
    return TypedResults.Ok<List<NumItem>>(items);
})
.WithName("GetAllNumbers")
.WithOpenApi(operation =>
{
    operation.Summary = "Get all numbers";
    operation.Description = "Retrieves all number items from the database.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
});



app.MapGet("api/numbers/{id:int}", async (AppDbContext context, int id) =>
{
    var item = await context.NumItems.FindAsync(id);
    if (item == null)
        return Results.NotFound();
    return TypedResults.Ok(item);
})
.WithName("GetNumberById")
.WithOpenApi(operation =>
{
    operation.Summary = "Get a number item by ID";
    operation.Description = "Retrieves a single number item from the database using its unique ID.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
});

app.MapPost("api/numbers", async (AppDbContext context, NumItem numItem) =>
{
    await context.NumItems.AddAsync(numItem);
    await context.SaveChangesAsync();

    return TypedResults.Created($"api/numbers/{numItem.Id}", numItem);
})
.WithName("CreateNumItem")
.WithOpenApi(operation =>
{
    operation.Summary = "Create a new Num Item";
    operation.Description = "New number entry saved.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
}); ;

#endregion



app.Run();
