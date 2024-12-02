using Domain.Interfaces;
using Domain;
using Domain.Mapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddSession(); 
builder.Services.AddDistributedMemoryCache(); 

builder.Services.AddScoped<IUserBeheer, UserBeheer>();
builder.Services.AddScoped<IVrijwilligersWerkBeheer, VrijwilligersWerkBeheer>();
builder.Services.AddScoped<IRegistratieBeheer, RegistratieBeheer>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<WerkMapper>();
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
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/LoginGebruiker");
    return Task.CompletedTask;
});

app.Run();
