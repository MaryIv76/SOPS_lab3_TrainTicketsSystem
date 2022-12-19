using TrainTicketsServer.Services;
using Microsoft.EntityFrameworkCore;
using TrainTicketsServer.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CancelTicketService>();
app.MapGrpcService<CartService>();
app.MapGrpcService<ChangePassengerService>();
app.MapGrpcService<LoginService>();
app.MapGrpcService<PaymentService>();
app.MapGrpcService<SeatsInfoService>();
app.MapGrpcService<TrainsInfoService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
