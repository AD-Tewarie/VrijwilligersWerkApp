using Domain.Interfaces;
using Domain;
using Domain.Mapper;
using Infrastructure.Repos_DB;
using Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddSession(); 
builder.Services.AddDistributedMemoryCache();




// configure conn string
builder.Services.AddSingleton(new DBSettings
{
    DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
});


// Dependency Injection db
builder.Services.AddScoped<IUserRepository, UserRepositoryDB>();
builder.Services.AddScoped<IVrijwilligersWerkRepository, VrijwilligersWerkRepositoryDB>();
builder.Services.AddScoped<IWerkRegistratieRepository, WerkRegistratieRepositoryDB>();

//Dependency Injection domain
builder.Services.AddScoped<IUserBeheer, UserBeheer>();
builder.Services.AddScoped<IVrijwilligersWerkBeheer, VrijwilligersWerkBeheer>();
builder.Services.AddScoped<IRegistratieBeheer, RegistratieBeheer>();

// DI mappers
builder.Services.AddScoped<WerkMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<RegistratieMapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session middleware
app.UseAuthorization();

app.MapRazorPages();

// temp 
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/LoginGebruiker");
    return Task.CompletedTask;
});

app.Run();
