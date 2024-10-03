using Microsoft.AspNetCore.SignalR;
using WebSocket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();
builder.Services.AddSingleton<ClientManager>();
var AllowSpecificOrigins = "localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                          .WithOrigins("http://localhost:3000", "https://classbon.com", "https://staging.classbon.com", "https://nafisexpress.com")
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowSpecificOrigins);

app.MapGet("/connected-client-count", (ClientManager clientManager) =>
{
    return Results.Ok(clientManager.GetConnectedClientCount());
});

app.MapHub<MainHub>("/hub");

app.Run();

