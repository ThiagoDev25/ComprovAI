using Microsoft.EntityFrameworkCore;
using ComprovAI.Services;
using DotNetEnv;

Env.Load("./Environments/.env");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Program.cs

var accountEndPoint = DotNetEnv.Env.GetString("ACCOUNTENDPOINT");
var accountKey = DotNetEnv.Env.GetString("ACCOUNTKEY");



var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
var containerName = builder.Configuration["CosmosDb:ContainerName"];

builder.Services.AddDbContext<ComprovAI.Data.ApplicationDbContext>(options =>
    options.UseCosmos(accountEndPoint, accountKey, databaseName));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ComprovAI.Data.ApplicationDbContext>();
    
    // Força a criação do DB e Container. 
    // Usamos .Wait() ou .GetAwaiter().GetResult() APENAS no contexto do host/startup
    // para bloquear a inicialização até que o DB esteja pronto.
    context.Database.EnsureCreatedAsync().Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Payment}/{action=Index}/{id?}");

app.Run();