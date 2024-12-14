using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Mapper;
using Domain.Vrijwilligerswerk_Test.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestBeheer : ITestBeheer
    {
        private readonly IGebruikersTestRepository categorieRepo;
        private readonly WerkMapper werkMapper;
        private readonly TestAlgoritme testAlgo;
        private readonly TestMapper testMapper;

        public TestBeheer(IGebruikersTestRepository categorieRepository, TestAlgoritme testAlgoritme, TestMapper testMapper, WerkMapper werkMapper)
        {
            categorieRepo = categorieRepository;
            testAlgo = testAlgoritme;
            this.testMapper = testMapper;
            this.werkMapper = werkMapper;
        }

        public List<VraagCategorie> HaalAlleVraagCategorieënOp()
        {
            var categorieDTOs = categorieRepo.HaalAlleCategorieënOp();
            var vraagCategorieën = new List<VraagCategorie>();

            // haal alle vragen op 
            var alleVragenDTOs = categorieRepo.HaalAlleTestVraagOp();

            foreach (var categorieDTO in categorieDTOs)
            {
                // Filter de vragen op categorie met lambda expressions
                var vragenDTOs = alleVragenDTOs.Where(vraag => vraag.CategorieId == categorieDTO.Id).ToList();
                var vragen = vragenDTOs.Select(testMapper.MapToTestVraag).ToList();

                // maak vraagCategorie met de verschillende vragen per categorie
                var vraagCategorie = new VraagCategorie(categorieDTO.Id, categorieDTO.Naam, vragen);
                vraagCategorieën.Add(vraagCategorie);
            }

            return vraagCategorieën;
        }

        public Categorie GetCategorieOnId(int id)
        {
            return testMapper.MapToCategorie(categorieRepo.GetCategorieOnId(id));
        }


        public List<VrijwilligersWerk> HaalAlleWerkOp()
        {
            return werkMapper.MapToWerkLijst();
        }

        public Dictionary<Categorie, int> BerekenTestScores(Dictionary<int, int> affiniteiten, Dictionary<int, int> antwoorden)
        {
            return testAlgo.BerekenTestScoresUsingIds(affiniteiten, antwoorden);
        }

        public List<VrijwilligersWerk> FilterWerk(List<VrijwilligersWerk> werk, Dictionary<Categorie, int> gesorteerdeScores)
        {
            return testAlgo.FilterEnSorteerWerk(werk, gesorteerdeScores);
        }

        public List<Categorie> HaalAlleCategorieënOp()
        {
            return testMapper.GetCategorieLijst();
        }
    }
}

