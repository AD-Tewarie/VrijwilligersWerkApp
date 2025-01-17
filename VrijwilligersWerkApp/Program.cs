using Application.Interfaces;
using Application.Mapper;
using Application.Service;
using Application.ViewModels;
using Domain.Common.Interfaces.Repository;
using Domain.Gebruikers.Interfaces;
using Domain.Gebruikers.Services;
using Domain.Gebruikers.Services.WachtwoordStrategy;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.ScoreStrategy.Interfaces;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.ScoreStrategy;
using Domain.Vrijwilligerswerk_Test.Services;
using Domain.Vrijwilligerswerk_Test.WerkScore;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;
using Domain.Werk.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Database Settings
builder.Services.Configure<DBSettings>(
   builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton(sp =>
   sp.GetRequiredService<IOptions<DBSettings>>().Value);

// Infrastructure Layer
builder.Services.AddScoped<IUserRepository, UserRepositoryDB>();
builder.Services.AddScoped<IVrijwilligersWerkRepository, VrijwilligersWerkRepositoryDB>();
builder.Services.AddScoped<IWerkRegistratieRepository, WerkRegistratieRepositoryDB>();
builder.Services.AddScoped<IGebruikersTestRepository, GebruikersTestRepositoryDB>();

// Domain Layer - Core Services
builder.Services.AddScoped<IUserBeheer, UserBeheer>();
builder.Services.AddScoped<IVrijwilligersWerkBeheer, VrijwilligersWerkBeheer>();
builder.Services.AddScoped<IRegistratieBeheer, RegistratieBeheer>();
builder.Services.AddScoped<ITestBeheer, TestBeheer>();

// Domain Layer - Test Related Services
builder.Services.AddScoped<IScoreStrategy, StandaardScoreStrategy>();
builder.Services.AddScoped<IWerkPresentatieService, WerkPresentatieService>();
builder.Services.AddScoped<IWerkScoreService, WerkScoreService>();
builder.Services.AddScoped<ICategorieService, CategorieService>();
builder.Services.AddScoped<ITestVraagService, TestVraagService>();
builder.Services.AddScoped<IGebruikersTestService, GebruikersTestService>();
builder.Services.AddSingleton<ITestSessieBeheer, TestSessieBeheer>();  // Singleton omdat we state willen behouden

// Application Layer Services
builder.Services.AddScoped<IVrijwilligersWerkService, VrijwilligersWerkService>();
builder.Services.AddScoped<IGebruikersTestResultaatService, GebruikersTestResultaatService>();



// View Mappers
builder.Services.AddScoped<IViewModelMapper<VrijwilligersWerkViewModel, VrijwilligersWerk>,
   VrijwilligersWerkViewMapper>();



// Wachtwoord Strategy
builder.Services.AddScoped<IWachtwoordStrategy, DefaultWachtwoordStrategy>();



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
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login/LoginGebruiker");
    return Task.CompletedTask;
});

app.Run();