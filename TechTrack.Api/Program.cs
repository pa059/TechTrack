using Microsoft.EntityFrameworkCore;
using TechTrack.Api.Data;
using TechTrack.Api.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GreetingServices>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "TechTrack API v1");
    });
}

//builder.Services.AddSingleton<GreetingServices>();

app.Use(async (context, next) =>
{
    Console.WriteLine(
        $"Request: {context.Request.Method} {context.Request.Path}"
    );

    await next();

    Console.WriteLine(
        $"Response: {context.Response.StatusCode}"
    );
});
app.MapGet("/", () => "Hello World!");
app.MapGet("greeting",(GreetingServices greeting) => 
    {
        return greeting.getGreeting();
});
app.MapControllers();
//app.MapGet("/hello",() =>"Hellp, Paul Joseph!");

app.Run();
