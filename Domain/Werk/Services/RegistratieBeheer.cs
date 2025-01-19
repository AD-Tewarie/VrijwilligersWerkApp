﻿using Domain.Common.Exceptions;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;

namespace Domain.Werk.Services
{
    public class RegistratieBeheer : IRegistratieBeheer
    {
        private readonly IWerkRegistratieRepository registratieRepository;
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IUserRepository userRepository;

        public RegistratieBeheer(
            IWerkRegistratieRepository registratieRepository,
            IVrijwilligersWerkBeheer werkBeheer,
            IUserRepository userRepository)
        {
            this.registratieRepository = registratieRepository ?? throw new ArgumentNullException(nameof(registratieRepository));
            this.werkBeheer = werkBeheer ?? throw new ArgumentNullException(nameof(werkBeheer));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void RegistreerGebruikerVoorWerk(int userId, int werkId)
        {
            var werk = werkBeheer.HaalWerkOpID(werkId) ?? 
                throw new DomainValidationException("Werk niet gevonden", new Dictionary<string, ICollection<string>> {
                    { "Werk", new[] { $"Werk met ID {werkId} niet gevonden." } }
                });

            var gebruiker = userRepository.GetUserOnId(userId) ?? 
                throw new DomainValidationException("Gebruiker niet gevonden", new Dictionary<string, ICollection<string>> {
                    { "Gebruiker", new[] { $"Gebruiker met ID {userId} niet gevonden." } }
                });

            // Check capaciteit voordat we de registratie proberen
            var huidigeRegistraties = registratieRepository.GetRegistratieCountForWerk(werkId);
            if (huidigeRegistraties >= werk.MaxCapaciteit)
            {
                throw new DomainValidationException("Maximum capaciteit bereikt", new Dictionary<string, ICollection<string>> {
                    { "Capaciteit", new[] { "Dit werk heeft het maximaal aantal registraties bereikt." } }
                });
            }

            // Check bestaande registratie
            if (registratieRepository.HeeftGebruikerRegistratie(werkId, userId))
            {
                throw new DomainValidationException("Dubbele registratie", new Dictionary<string, ICollection<string>> {
                    { "Registratie", new[] { "Je bent al geregistreerd voor dit werk." } }
                });
            }

            var registratie = WerkRegistratie.MaakNieuw(werk, gebruiker);
            registratieRepository.AddWerkRegistratie(registratie);
        }

        public void VerwijderRegistratie(int registratieId)
        {
            var registratie = HaalRegistratieOp(registratieId);
            var werk = werkBeheer.HaalWerkOpID(registratie.VrijwilligersWerk.WerkId);

            if (!registratieRepository.VerwijderWerkRegistratie(registratieId))
            {
                throw new DomainValidationException("Verwijderen mislukt", new Dictionary<string, ICollection<string>> {
                    { "Registratie", new[] { "Kon de registratie niet verwijderen." } }
                });
            }
        }

        public List<WerkRegistratie> HaalRegistratiesOp()
        {
            return registratieRepository.GetWerkRegistraties();
        }

        public WerkRegistratie HaalRegistratieOp(int registratieId)
        {
            return registratieRepository.GetRegistratieOnId(registratieId) ?? 
                throw new DomainValidationException("Registratie niet gevonden", new Dictionary<string, ICollection<string>> {
                    { "Registratie", new[] { $"Registratie met ID {registratieId} niet gevonden." } }
                });
        }

        public int HaalAantalRegistratiesOp(int werkId)
        {
            return registratieRepository.GetRegistratieCountForWerk(werkId);
        }

        public bool HeeftGebruikerRegistratie(int werkId, int gebruikerId)
        {
            return registratieRepository.HeeftGebruikerRegistratie(werkId, gebruikerId);
        }
    
        public WerkRegistratie GetRegistratieByWerkAndUser(int werkId, int gebruikerId)
        {
            return registratieRepository.GetRegistratieByWerkAndGebruiker(werkId, gebruikerId) ??
                throw new DomainValidationException("Registratie niet gevonden", new Dictionary<string, ICollection<string>> {
                    { "Registratie", new[] { $"Geen registratie gevonden voor werk {werkId} en gebruiker {gebruikerId}." } }
                });
        }
    }
}
