using MedicalScheduling.Application;
using MedicalScheduling.Infrastructure;
using MedicalScheduling.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

var app = builder.Build();
app.UsePresentation();
app.Run();

public partial class Program;
