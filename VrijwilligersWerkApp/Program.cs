using Domain.Interfaces;
using Domain;
using Domain.Mapper;
using Infrastructure.Repos_DB;
using Infrastructure.Interfaces;
using Domain.WachtwoordStrategy;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Domain.Vrijwilligerswerk_Test.ScoreStrategy;
using VrijwilligersWerkApp.Services;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Mapper;
using Domain.Vrijwilligerswerk_Test.WerkScore;
using Infrastructure.DTO;
using Infrastructure;
using Microsoft.Extensions.Options;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;

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

// Wachtwoord Strategy
builder.Services.AddScoped<IWachtwoordStrategy, DefaultWachtwoordStrategy>();

// Mappers
builder.Services.AddScoped<IMapper<VrijwilligersWerk, VrijwilligersWerkDTO>, WerkMapper>();
builder.Services.AddScoped<IMapper<User, UserDTO>, UserMapper>();
builder.Services.AddScoped<IMapper<WerkRegistratie, WerkRegistratieDTO>, RegistratieMapper>();
builder.Services.AddScoped<IMapper<Categorie, CategorieDTO>, CategorieMapper>();
builder.Services.AddScoped<IMapper<TestVraag, TestVraagDTO>, TestVraagMapper>();
builder.Services.AddScoped<IMapper<WerkCategorie, WerkCategorieDTO>, WerkCategorieMapper>();


// Application Services
builder.Services.AddScoped<ITestSessionService, TestSessionService>();

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