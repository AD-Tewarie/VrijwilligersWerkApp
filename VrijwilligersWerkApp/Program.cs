using Application.DI;
using Domain.DI;
using Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

// ASP.NET Core services
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HTTP Context Accessor voor sessie beheer
builder.Services.AddHttpContextAccessor();

// Registreer services volgens Clean Architecture principes
// Clean Architecture bestaat uit concentrische lagen:
// 1. Domain Layer (kern)
//    - Bevat business logica en regels
//    - Heeft geen afhankelijkheden van andere lagen
//    - Definieert interfaces voor repositories
builder.Services
    .VoegDomainToe(builder.Configuration)
    // 2. Application Layer
    //    - Implementeert use cases
    //    - Coördineert tussen UI en domeinlogica
    //    - Afhankelijk van Domain layer
    .VoegApplicationToe(builder.Configuration)
    // 3. Infrastructure Layer (buitenste laag)
    //    - Implementeert interfaces uit Domain layer
    //    - Bevat database toegang, externe services
    //    - Afhankelijk van Domain interfaces
    .VoegInfrastructuurToe(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Middleware configuratie
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

// Standaard route
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/LoginGebruiker");
    return Task.CompletedTask;
});

app.Run();