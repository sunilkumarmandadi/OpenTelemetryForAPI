using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics.Metrics;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var resource = ResourceBuilder.CreateDefault().AddService("OTConsole");
var meterName = "OTMeter";
var OTmeter = new Meter(meterName, "V 1.0");
var requestCounter = OTmeter.CreateCounter<long>("Requests");
var requestHistogram = OTmeter.CreateHistogram<long>("Requests");

//var builder = Microsoft.AspNetCore. WebApplication.cre
//var builder = WebApplication.CreateBuilder(args);