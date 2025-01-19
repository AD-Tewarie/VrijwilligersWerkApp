# VrijwilligersWerkApp - Clean Architecture Implementatie

Deze applicatie is gebouwd volgens Clean Architecture principes, ook bekend als de "Onion Architecture". Het project is verdeeld in drie hoofdlagen:

## 1. Domain Layer (Kernlaag)
- Bevat de kern business logica
- Definieert interfaces en modellen
- Heeft geen externe afhankelijkheden
- Voorbeelden:
  - Domain/Gebruikers/Models/User.cs
  - Domain/Common/Interfaces/Repository/IUserRepository.cs

## 2. Application Layer (Applicatielaag)
- Implementeert use cases
- Coördineert tussen UI en domeinlogica
- Afhankelijk van Domain layer
- Bevat ViewModels en Mappers
- Voorbeelden:
  - Application/GebruikersTest/Services/TestBeheerService.cs
  - Application/GebruikersTest/Mappers/GebruikersTestMapper.cs

## 3. Infrastructure Layer (Infrastructuurlaag)
- Implementeert interfaces uit Domain layer
- Bevat technische details (database, externe services)
- Afhankelijk van Domain interfaces
- Voorbeelden:
  - Infrastructure/Repos DB/UserRepositoryDB.cs
  - Infrastructure/Repos DB/DatabaseService.cs

## Project Structuur en Naamgeving

### Naamgevingsconventies
- Interfaces beginnen met 'I': IUserRepository, ITestBeheer
- ViewModels eindigen op 'ViewModel': GebruikersTestViewModel
- Services eindigen op 'Service': TestBeheerService
- Repositories eindigen op 'Repository': UserRepository
- Mappers eindigen op 'Mapper': GebruikersTestMapper

### Mappenstructuur per Feature
```
Feature/
├── Interfaces/          # Interface definities
├── Models/             # Domain models
├── ViewModels/         # ViewModels voor de UI
├── Services/           # Service implementaties
└── Mappers/           # Mappers tussen models en viewmodels
```

### Voorbeelden van Feature-mappen
- GebruikersTest/
- VrijwilligersWerk/
- Authenticatie/
- GebruikersProfiel/

## Code Voorbeelden

### 1. Domain Model met Value Objects
```csharp
public class VrijwilligersWerk
{
    public int Id { get; private set; }
    public string Titel { get; private set; }
    public string Beschrijving { get; private set; }
    public WerkCategorie Categorie { get; private set; }
    public Status Status { get; private set; }

    private VrijwilligersWerk() { } // Voor EF Core

    public VrijwilligersWerk(string titel, string beschrijving, WerkCategorie categorie)
    {
        ValideerTitel(titel);
        Titel = titel;
        Beschrijving = beschrijving;
        Categorie = categorie;
        Status = Status.Beschikbaar;
    }

    private void ValideerTitel(string titel)
    {
        if (string.IsNullOrWhiteSpace(titel))
            throw new DomainValidationException("Titel mag niet leeg zijn");
    }
}
```

### 2. Repository Interface in Domain Layer
```csharp
public interface IVrijwilligersWerkRepository
{
    VrijwilligersWerk HaalOpOpId(int id);
    List<VrijwilligersWerk> HaalAlleOp();
    void Toevoegen(VrijwilligersWerk werk);
    void Bijwerken(VrijwilligersWerk werk);
    void Verwijderen(int id);
}
```

### 3. Application Service met Mapper
```csharp
public class WerkBeheerService : IWerkBeheerService
{
    private readonly IVrijwilligersWerkRepository repository;
    private readonly IViewModelMapper<WerkViewModel, VrijwilligersWerk> mapper;

    public WerkBeheerService(
        IVrijwilligersWerkRepository repository,
        IViewModelMapper<WerkViewModel, VrijwilligersWerk> mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public WerkViewModel HaalWerkOp(int id)
    {
        var werk = repository.HaalOpOpId(id);
        if (werk == null)
            throw new NotFoundException($"Werk niet gevonden met ID: {id}");
        return mapper.MapNaarViewModel(werk);
    }

    public List<WerkViewModel> HaalAlleWerkenOp()
    {
        var werken = repository.HaalAlleOp();
        return werken.Select(w => mapper.MapNaarViewModel(w)).ToList();
    }
}
```

### 4. Infrastructure Repository Implementatie
```csharp
public class VrijwilligersWerkRepositoryDB : IVrijwilligersWerkRepository
{
    private readonly IDatabaseService db;
    private readonly ILogger<VrijwilligersWerkRepositoryDB> logger;

    public VrijwilligersWerkRepositoryDB(
        IDatabaseService db,
        ILogger<VrijwilligersWerkRepositoryDB> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public VrijwilligersWerk HaalOpOpId(int id)
    {
        try
        {
            var query = "SELECT * FROM VrijwilligersWerk WHERE Id = @Id";
            return db.QuerySingleOrDefault<VrijwilligersWerk>(query, new { Id = id });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database fout bij ophalen werk {Id}", id);
            throw new RepositoryException("Fout bij ophalen werk uit database", ex);
        }
    }

    public List<VrijwilligersWerk> HaalAlleOp()
    {
        try
        {
            var query = "SELECT * FROM VrijwilligersWerk";
            return db.Query<VrijwilligersWerk>(query).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database fout bij ophalen alle werken");
            throw new RepositoryException("Fout bij ophalen werken uit database", ex);
        }
    }
}
```

## Foutafhandeling

### 1. Domain Exceptions
```csharp
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message) { }
}
```

### 2. Application Exceptions
```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message) { }
    public ServiceException(string message, Exception inner) : base(message, inner) { }
}
```

### 3. Infrastructure Exceptions
```csharp
public class RepositoryException : Exception
{
    public RepositoryException(string message) : base(message) { }
    public RepositoryException(string message, Exception inner) : base(message, inner) { }
}
```

## Testing Strategie

### 1. Domain Layer Tests
```csharp
public class VrijwilligersWerkTests
{
    [Fact]
    public void MaakWerk_MetLeegeTitel_GooitException()
    {
        // Arrange
        var titel = "";
        var beschrijving = "Test beschrijving";
        var categorie = new WerkCategorie("Test");

        // Act & Assert
        Assert.Throws<DomainValidationException>(() => 
            new VrijwilligersWerk(titel, beschrijving, categorie));
    }
}
```

### 2. Application Layer Tests
```csharp
public class WerkBeheerServiceTests
{
    private readonly Mock<IVrijwilligersWerkRepository> repositoryMock;
    private readonly Mock<IViewModelMapper<WerkViewModel, VrijwilligersWerk>> mapperMock;
    private readonly WerkBeheerService service;

    public WerkBeheerServiceTests()
    {
        repositoryMock = new Mock<IVrijwilligersWerkRepository>();
        mapperMock = new Mock<IViewModelMapper<WerkViewModel, VrijwilligersWerk>>();
        service = new WerkBeheerService(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public void HaalWerkOp_BestaandId_RetourneertViewModel()
    {
        // Arrange
        var werk = new VrijwilligersWerk("Test", "Beschrijving", new WerkCategorie("Test"));
        var viewModel = new WerkViewModel { Id = 1, Titel = "Test" };
        
        repositoryMock.Setup(r => r.HaalOpOpId(1)).Returns(werk);
        mapperMock.Setup(m => m.MapNaarViewModel(werk)).Returns(viewModel);

        // Act
        var result = service.HaalWerkOp(1);

        // Assert
        Assert.Equal(viewModel.Id, result.Id);
        Assert.Equal(viewModel.Titel, result.Titel);
    }
}
```

### 3. Infrastructure Layer Tests
```csharp
public class VrijwilligersWerkRepositoryDBTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture fixture;

    public VrijwilligersWerkRepositoryDBTests(TestDatabaseFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void HaalOpOpId_BestaandWerk_RetourneertWerk()
    {
        // Arrange
        var repository = new VrijwilligersWerkRepositoryDB(fixture.Db);
        
        // Act
        var werk = repository.HaalOpOpId(1);

        // Assert
        Assert.NotNull(werk);
        Assert.Equal("Test Werk", werk.Titel);
    }
}
```

## Test Richtlijnen

1. Domain Layer Tests
   - Focus op business rules en invarianten
   - Test domain model gedrag
   - Geen mocks nodig
   - Snelle unit tests

2. Application Layer Tests
   - Mock repositories en andere dependencies
   - Test orchestratie en coördinatie
   - Verifieer mapper gebruik
   - Test error handling

3. Infrastructure Layer Tests
   - Gebruik test database
   - Test SQL queries
   - Test externe service integratie
   - Langzamere integratie tests

4. End-to-End Tests
   - Test complete use cases
   - Gebruik echte database
   - Test UI flows
   - Langzaamste maar meest complete tests

## Ontwikkelrichtlijnen

1. Laag Afhankelijkheden
   - Domain layer mag GEEN referenties hebben naar andere lagen
   - Application layer mag alleen refereren naar Domain layer
   - Infrastructure layer mag alleen refereren naar Domain interfaces

2. Naamgeving
   - Gebruik Nederlandse namen voor business concepten
   - Gebruik Engelse namen voor technische concepten
   - Wees consistent in naamgeving binnen een context

3. Interfaces
   - Definieer interfaces in de Domain layer
   - Houd interfaces klein en specifiek
   - Documenteer het beoogde gebruik

4. ViewModels
   - Gebruik ViewModels voor alle UI interacties
   - Implementeer alleen properties die de UI nodig heeft
   - Gebruik mappers voor transformaties