using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host
    .UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]!))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName!)
            .ReadFrom.Configuration(context.Configuration);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
