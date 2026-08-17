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
                      ?? "https://localhost:7001";

builder.Services.AddHttpClient<IStockServiceClient, StockServiceClient>(client =>
{
    client.BaseAddress = new Uri(stockServiceUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();