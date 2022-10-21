using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetryForAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// shared Resource to use for both OTel metrics AND tracing
var resource = ResourceBuilder.CreateDefault().AddService("Web API", "SampleApp");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(b =>
{
    // uses the default Jaeger settings
    b.AddJaegerExporter();

    // receive traces from our own custom sources
    b.AddSource(TelemetryConstants.MyAppSource);

    // decorate our service name so we can find it when we look inside Jaeger
    b.SetResourceBuilder(resource);

    // receive traces from built-in sources
    b.AddHttpClientInstrumentation();
    b.AddAspNetCoreInstrumentation();
    b.AddSqlClientInstrumentation();
});

builder.Services.AddOpenTelemetryMetrics(b =>
{
    // add prometheus exporter
    b.AddPrometheusExporter();

    // receive metrics from our own custom sources
    b.AddMeter(TelemetryConstants.MyAppSource);

    // decorate our service name so we can find it when we look inside Prometheus
    b.SetResourceBuilder(resource);

    // receive metrics from built-in sources
    b.AddHttpClientInstrumentation();
    b.AddAspNetCoreInstrumentation();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// add the /metrics endpoint which will be scraped by Prometheus
//app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
