﻿﻿﻿using Domain.Common.Interfaces.Repository;
using Domain.Common.Exceptions;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;

namespace Domain.Werk.Services;

public class VrijwilligersWerkBeheer : IVrijwilligersWerkBeheer
{
    private readonly IVrijwilligersWerkRepository repository;

    public VrijwilligersWerkBeheer(IVrijwilligersWerkRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public void VoegWerkToe(string titel, string omschrijving, int maxCapaciteit, string locatie, int categorieId)
    {
        ValideerWerkDetails(titel, maxCapaciteit, omschrijving, locatie);

        var werk = VrijwilligersWerk.MaakNieuw(titel, omschrijving, maxCapaciteit, locatie);
        var werkId = repository.AddVrijwilligersWerk(werk); 
        repository.VoegWerkCategorieToeAanNieuweWerk(werkId, categorieId); 
    }

    public List<VrijwilligersWerk> BekijkAlleWerk()
    {
        return repository.GetVrijwilligersWerk();
    }

    public void VerwijderWerk(int werkId)
    {
        ValideerWerkId(werkId);
        HaalEnControleerWerk(werkId);
        repository.VerwijderVrijwilligersWerk(werkId);
    }

    public void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving, string nieuweLocatie)
    {
        ValideerWerkDetails(nieuweTitel, nieuweCapaciteit, nieuweBeschrijving, nieuweLocatie);
        var werk = HaalEnControleerWerk(werkId);

        werk.WijzigDetails(nieuweTitel, nieuweBeschrijving, nieuweCapaciteit, nieuweLocatie);
        repository.BewerkVrijwilligersWerk(werk);
    }

    public VrijwilligersWerk HaalWerkOpID(int id)
    {
        ValideerWerkId(id);
        return HaalEnControleerWerk(id);
    }

    private void ValideerWerkDetails(string titel, int capaciteit, string beschrijving, string locatie)
    {
        var fouten = new Dictionary<string, ICollection<string>>();

        if (string.IsNullOrWhiteSpace(titel))
            fouten.Add("Titel", new[] { "Titel is verplicht." });
        if (string.IsNullOrWhiteSpace(beschrijving))
            fouten.Add("Beschrijving", new[] { "Beschrijving is verplicht." });
        if (string.IsNullOrWhiteSpace(locatie))
            fouten.Add("Locatie", new[] { "Locatie is verplicht." });
        if (capaciteit <= 0)
            fouten.Add("Capaciteit", new[] { "Capaciteit moet groter zijn dan 0." });

        if (fouten.Any())
            throw new DomainValidationException("Validatie fouten opgetreden", fouten);
    }

    private void ValideerWerkId(int werkId)
    {
        if (werkId <= 0)
            throw new DomainValidationException("WerkId", "Werk ID moet groter zijn dan 0.");
    }

    private VrijwilligersWerk HaalEnControleerWerk(int werkId)
    {
        var werk = repository.GetWerkOnId(werkId);
        if (werk == null)
            throw new KeyNotFoundException($"Vrijwilligerswerk met ID {werkId} niet gevonden.");
        return werk;
    }
}
