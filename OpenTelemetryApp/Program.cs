using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);
var resource = ResourceBuilder.CreateDefault().AddService("Web App", "SampleApp");

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddOpenTelemetryTracing((builder) => builder
    .AddZipkinExporter(o => o.HttpClientFactory = () =>
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value");
        return client;
    }));

builder.Services.AddOpenTelemetryTracing(b =>
{
    // uses the default Jaeger settings
    b.AddJaegerExporter();

    // receive traces from our own custom sources
    b.AddSource("OpenTelemetry App");

    // decorate our service name so we can find it when we look inside Jaeger
    b.SetResourceBuilder(resource);

    // receive traces from built-in sources
    b.AddHttpClientInstrumentation();
    b.AddAspNetCoreInstrumentation();
    
});

builder.Logging.AddOpenTelemetry(options => {
    options.SetResourceBuilder(resource);
    
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
