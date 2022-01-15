using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// var myAllowSpecificOrigins = "_MyAllowSubdomainPolicy";

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: myAllowSpecificOrigins,
//         builder =>
//         {
//             _ = builder.AllowAnyOrigin()
//                    .AllowAnyHeader()
//                    .AllowAnyMethod();
//         });
// });

_ = builder.Services.AddHttpsRedirection(options => options.HttpsPort = 443);
_ = builder.Configuration.AddJsonFile("configuration.json", false, true);
_ = builder.Services.AddOcelot(builder.Configuration);
_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

_ = app.UseHttpsRedirection();
_ = app.UseAuthorization();
_ = app.MapControllers();

// app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseOcelot().Wait();
app.Run();
