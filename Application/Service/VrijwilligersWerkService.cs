using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Mapper;
using Application.ViewModels;
using Domain;
using Domain.GebruikersTest.Interfaces;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;


namespace Application.Service
{
    public class VrijwilligersWerkService : IVrijwilligersWerkService
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IRegistratieBeheer registratieBeheer;
        private readonly IViewModelMapper<VrijwilligersWerkViewModel, VrijwilligersWerk> viewMapper;
        private readonly ICategorieService categorieService;

        public VrijwilligersWerkService(
            IVrijwilligersWerkBeheer werkBeheer,
            IRegistratieBeheer registratieBeheer,
            IViewModelMapper<VrijwilligersWerkViewModel, VrijwilligersWerk> viewMapper,
            ICategorieService categorieService)
        {
            this.werkBeheer = werkBeheer;
            this.registratieBeheer = registratieBeheer;
            this.viewMapper = viewMapper;
            this.categorieService = categorieService;
        }

        public List<VrijwilligersWerkViewModel> HaalAlleWerkenOp()
        {
            var werken = werkBeheer.BekijkAlleWerk();
            return werken.Select(werk => viewMapper.MapNaarViewModel(werk)).ToList();
        }

        public bool RegistreerVoorWerk(int werkId, int gebruikerId)
        {
            try
            {
                if (!IsWerkGeldig(werkId))
                    return false;

                if (IsGebruikerAlGeregistreerd(werkId, gebruikerId))
                    return false;

                if (IsWerkVol(werkId))
                    return false;

                VoerRegistratieUit(werkId, gebruikerId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsWerkGeldig(int werkId)
        {
            var werk = werkBeheer.HaalWerkOpID(werkId);
            return werk != null;
        }

        private bool IsGebruikerAlGeregistreerd(int werkId, int gebruikerId)
        {
            var bestaandeRegistraties = registratieBeheer.HaalRegistratiesOp();
            return bestaandeRegistraties.Any(r =>
                r.VrijwilligersWerk.WerkId == werkId &&
                r.User.UserId == gebruikerId);
        }

        private bool IsWerkVol(int werkId)
        {
            var werk = werkBeheer.HaalWerkOpID(werkId);
            return werk.AantalRegistraties >= werk.MaxCapaciteit;
        }

        private void VoerRegistratieUit(int werkId, int gebruikerId)
        {
            registratieBeheer.RegistreerGebruikerVoorWerk(gebruikerId, werkId);
        }


        public bool VerwijderRegistratie(int registratieId)
        {
            try
            {
                if (!RegistratieBestaatEnIsGeldig(registratieId))
                    return false;

                VerwijderRegistratieUitDatabase(registratieId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool RegistratieBestaatEnIsGeldig(int registratieId)
        {
            var registratie = registratieBeheer.HaalRegistratieOp(registratieId);
            return registratie != null;
        }

        private void VerwijderRegistratieUitDatabase(int registratieId)
        {
            registratieBeheer.VerwijderRegistratie(registratieId);
        }
    }
}
