using Microsoft.OpenApi.Models;
using PizzaStore.DB;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Todo API",
            Description = "Keep track of your tasks",
            Version = "v1"
        });
    });
}

var app = builder.Build();

// Configure middleware
app.UseCors("MyCorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas/{id}", (int id)=>PizzaDB.GetPizza(id));
app.MapGet("/pizzas", ()=> PizzaDB.GetPizzas());
app.MapPost("/pizzas", (Pizza pizza) =>PizzaDB.CreatePizza(pizza));
app.MapDelete("pizzas/{id}", (int id) => PizzaDB.RemovePizza(id)); 

app.Run();
