using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Vrijwilligerswerk_Test.Mapper
{
    public class TestMapper
    {

        private readonly IGebruikersTestRepository categorieRepos;

        public TestMapper(IGebruikersTestRepository categorieRepos)
        {
            this.categorieRepos = categorieRepos;
        }

        public List<Categorie> GetCategorieLijst()
        {
            var categorieDTOs = categorieRepos.HaalAlleCategorieënOp();
            return categorieDTOs.Select(MapToCategorie).ToList();
        }

        public Categorie MapToCategorie(CategorieDTO dto)
        {
            return new Categorie(dto.Id, dto.Naam);
        }

        public CategorieDTO MapToCategorieDTO(Categorie categorie)
        {
            return new CategorieDTO(categorie.Id, categorie.Naam);
        }

        public TestVraag MapToTestVraag(TestVraagDTO dto)
        {
            return new TestVraag(dto.Id, dto.Tekst, dto.CategorieId);
        }

        public TestVraagDTO MapToVraagDTO(TestVraag vraag)
        {
            return new TestVraagDTO(vraag.Id, vraag.Tekst, vraag.CategorieId);
        }
    }
}
