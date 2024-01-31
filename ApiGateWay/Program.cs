using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

IConfiguration config = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();

builder.Services.AddOcelot(config);
builder.Services.AddSwaggerForOcelot(config);

app.UseSwagger();
app.UseSwaggerForOcelotUI(opt =>
{
	opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.UseHttpsRedirection();
app.Run();
