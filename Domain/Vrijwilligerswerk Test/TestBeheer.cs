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
    public class TestBeheer
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

            foreach (var categorieDTO in categorieDTOs)
            {
                // Fetch test questions for the category
                var vragenDTOs = categorieRepo.GetTestVraagOpCategorieId(categorieDTO.Id);
                var vragen = vragenDTOs.Select(testMapper.MapToTestVraag).ToList();

                var vraagCategorie = new VraagCategorie(categorieDTO.Id, categorieDTO.Naam, vragen);
                vraagCategorieën.Add(vraagCategorie);
            }

            return vraagCategorieën;
        }

        public List<VrijwilligersWerk> HaalAlleWerkOp()
        {
            return werkMapper.MapToWerkLijst();
        }

        public Dictionary<Categorie, int> BerekenTestScores(Dictionary<Categorie, int> affiniteiten, Dictionary<TestVraag, int> antwoorden)
        {
            return testAlgo.BerekenScores(affiniteiten, antwoorden);
        }

        public List<VrijwilligersWerk> FilterWerk(List<VrijwilligersWerk> werk, Dictionary<Categorie, int> gesorteerdeScores)
        {
            return testAlgo.FilterEnSorteerWerk(werk, gesorteerdeScores);
        }

    }
}

