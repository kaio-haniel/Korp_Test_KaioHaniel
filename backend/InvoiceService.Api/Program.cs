using InvoiceService.Api.Data;
using InvoiceService.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var stockServiceUrl = builder.Configuration.GetValue<string>("Services:StockServiceUrl") 
                      ?? "https://localhost:5001";

builder.Services.AddHttpClient<IStockServiceClient, StockServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAngular");
app.MapControllers();

app.Run();