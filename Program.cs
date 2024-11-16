using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Disable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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

// Use CORS
app.UseCors();

app.Map("/stream", async context =>
{
    context.Response.ContentType = "text/event-stream";
    await using var writer = new StreamWriter(context.Response.Body, Encoding.UTF8, leaveOpen: true);

    for (int i = 0; i < 20; i++)
    {
        await writer.WriteLineAsync($"data: {i + 1} \n");
        await writer.WriteLineAsync();
        await writer.FlushAsync();
        await Task.Delay(1000);
    }
}).WithOpenApi().WithName("Stream");

app.Run();





