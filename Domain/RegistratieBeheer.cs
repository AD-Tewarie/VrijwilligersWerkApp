﻿using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RegistratieBeheer : IRegistratieBeheer
    {



        private readonly UserRepositoryDB userRepository = new UserRepositoryDB();
        private readonly WerkRegistratieRepositoryDB registratieRepository = new WerkRegistratieRepositoryDB();
        private readonly VrijwilligersWerkRepositoryDB werkRepository = new VrijwilligersWerkRepositoryDB();



        public int NieweRegistratieId()
        {
            List<WerkRegistratie> registraties = RegistratieMapper.MapToDomainList();
            int maxId = 0;
            foreach (var reg in registraties)
            {
                if (reg.RegistratieId > maxId)
                {
                    maxId = reg.RegistratieId;
                }
            }
            return maxId + 1;

        }

        public List<string> HaalRegistratiesOp() 
        {
            
                var regLijst = new List<string>();


                foreach (var registraties in RegistratieMapper.MapToDomainList())
                {
                    regLijst.Add($"RegistratieID: {registraties.RegistratieId} | Vrijwilligerswerk: {registraties.VrijwilligersWerk.Titel} | Geregistreerde Gebruiker: {registraties.User.Naam}  {registraties.User.AchterNaam}");
                }
                return regLijst;
           

        }


        public void RegistreerGebruikerVoorWerk(int gebruikerId, int werkId)
        {
            // Haal registraties op
            var registraties = RegistratieMapper.MapToDomainList();

            // Haal gebruiker op
            var getUser = userRepository.GetUserOnId(gebruikerId);
            var gebruiker = UserMapper.MapToUser(getUser);
            if (gebruiker == null)
            {
                throw new KeyNotFoundException("Gebruiker met opgegeven ID bestaat niet.");
            }

            // Haal vrijwilligerswerk op
            var werk = WerkMapper.MapToVrijwilligerswerk( werkRepository.GetWerkOnId(werkId));
            if (werk == null)
            {
                throw new KeyNotFoundException("Vrijwilligerswerk met opgegeven ID bestaat niet.");
            }

            // Controleer capaciteit
            if (werk.Aantalregistraties >= werk.MaxCapaciteit)
            {
                throw new InvalidOperationException("Maximale capaciteit voor dit vrijwilligerswerk is bereikt.");
            }

            // Controleer op dubbele registratie
            foreach (var reg in registraties)
            {
                if (reg.VrijwilligersWerk.WerkId == werkId && reg.User.UserId == gebruikerId)
                {
                    throw new InvalidOperationException("Deze gebruiker is al geregistreerd voor dit vrijwilligerswerk.");
                }
            }

            // Maak en voeg de registratie toe
            var registratie = new WerkRegistratie(werk, gebruiker, NieweRegistratieId() );
            registratieRepository.AddWerkRegistratie(RegistratieMapper.MapToDTO( registratie));

            // Update aantal registraties
            int wijziging = 1;
            werkRepository.BewerkAantalRegistraties(werkId, wijziging);

            
        }

        public void VerwijderRegistratie(int registratieId)
        {
            // Haal registratie op via de repository
            var registratie = registratieRepository.GetRegistratieOnId(registratieId);
            if (registratie == null)
            {
                throw new InvalidOperationException("Registratie niet gevonden.");
                
            }

            // Haal het gekoppelde vrijwilligerswerk op via de WerkId in de registratie
            var werk = werkRepository.GetWerkOnId(registratie.VrijwilligersWerk.WerkId);
            if (werk != null)
            {
                // Verminder het aantal registraties
                int wijziging = -1;
                werkRepository.BewerkAantalRegistraties(werk.WerkId, wijziging);
            }
            else
            {
                throw new InvalidOperationException("Gekoppeld vrijwilligerswerk niet gevonden.");
            }

            // Verwijder registratie via de repository
            registratieRepository.VerwijderWerkRegistratie(registratieId);
            
        }

       



    }

}
