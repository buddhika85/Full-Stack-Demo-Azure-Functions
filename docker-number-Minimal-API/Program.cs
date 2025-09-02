using docker_number_Minimal_API.Data;
using docker_number_Minimal_API.Models;
using Microsoft.EntityFrameworkCore;

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


app.MapGet("api/docker-numbers", async (AppDbContext context) =>
{
    var items = await context.NumItems.ToListAsync();
    return TypedResults.Ok<List<NumItem>>(items);
})
.WithName("GetAllNumbers")
.WithOpenApi(operation =>
{
    operation.Summary = "Get all numbers - From Azure Container Instance API";
    operation.Description = "Retrieves all number items from the database.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
});



app.MapGet("api/docker-numbers/{id:int}", async (AppDbContext context, int id) =>
{
    var item = await context.NumItems.FindAsync(id);
    if (item == null)
        return Results.NotFound();
    return TypedResults.Ok(item);
})
.WithName("GetNumberById")
.WithOpenApi(operation =>
{
    operation.Summary = "Get a number item by ID - From Azure Container Instance API";
    operation.Description = "Retrieves a single number item from the database using its unique ID.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
});

app.MapPost("api/docker-numbers", async (AppDbContext context, NumItem numItem) =>
{
    await context.NumItems.AddAsync(numItem);
    await context.SaveChangesAsync();

    return TypedResults.Created($"api/docker-numbers/{numItem.Id}", numItem);
})
.WithName("CreateNumItem")
.WithOpenApi(operation =>
{
    operation.Summary = "Create a new Num Item - On Azure Container Instance API";
    operation.Description = "New number entry saved.";
    operation.Tags = new[] { new Microsoft.OpenApi.Models.OpenApiTag { Name = "Numbers" } };
    return operation;
});

#endregion



app.Run();
