using CarSnapScore.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<NameGenerator>();
WebApplication app = builder.Build();

app.MapGet("/api/GetCarName", (NameGenerator nameGenerator) => nameGenerator.GetRandomCarName());

app.Run();
