using NumberClassificationApi.Interface;
using NumberClassificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<INumberService, NumberService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
  app.Map("/health", appBuilder => appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 200; // Return OK status
            await context.Response.WriteAsync("Healthy");
        }));
app.Run();
