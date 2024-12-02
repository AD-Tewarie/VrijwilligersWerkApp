using Domain.Interfaces;
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
    public class VrijwilligersWerkBeheer : IVrijwilligersWerkBeheer
    {


        private readonly VrijwilligersWerkRepositoryDB dbRepos = new VrijwilligersWerkRepositoryDB();
        private List<VrijwilligersWerk> werkLijst = new List<VrijwilligersWerk>();




        public int GetNieweWerkId()
        {
            werkLijst = WerkMapper.MapToWerkLijst();
            int maxId = 0;
            foreach (var werk in werkLijst)
            {
                if (werk.WerkId > maxId)
                {
                    maxId = werk.WerkId;
                }
            }
            maxId = maxId + 1;
            return maxId;

        }

        // Methode om een nieuw werk toe te voegen
        public void VoegWerkToe(VrijwilligersWerk werk)
        {
            var nieuwWerk = werk;
            werkLijst.Add(nieuwWerk);


            var werkDTO = WerkMapper.MapToDTO(nieuwWerk);
            dbRepos.AddVrijwilligersWerk(werkDTO);

            
        }

        // Methode voor het ophalen van een lijst van alle vrijwilligerswerk 

        public List<VrijwilligersWerk> BekijkAlleWerk()
        {
            var werkLijst = WerkMapper.MapToWerkLijst();
            
            return werkLijst;
        }
        

        // Methode om een werk te verwijderen
        public void VerwijderWerk(int werkId)
        {
            dbRepos.VerwijderVrijwilligersWerk(werkId);
        }




        public void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving)
        {
            // Haal het bestaande vrijwilligerswerk op
            var bestaandWerk = dbRepos.GetWerkOnId(werkId);
            if (bestaandWerk == null)
            {
                throw new KeyNotFoundException("Vrijwilligerswerk met opgegeven ID bestaat niet.");
            }

            // Voer wijzigingen door
            bestaandWerk.Titel = nieuweTitel;
            bestaandWerk.MaxCapaciteit = nieuweCapaciteit;
            bestaandWerk.Omschrijving = nieuweBeschrijving;



            // Sla de wijzigingen op via de repository
            dbRepos.BewerkVrijwilligersWerk(bestaandWerk);

            
        }


        public VrijwilligersWerk HaalwerkOpID(int id)
        {
            werkLijst = WerkMapper.MapToWerkLijst();

            for (int i = 0; i < werkLijst.Count; i++)
            {
                if (werkLijst[i].WerkId == id)
                {
                    return werkLijst[i];
                }

            }
            return null;
        }

    }

}

