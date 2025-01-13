using Domain.Mapper;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Infrastructure;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Infrastructure.Interfaces;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class CategorieService : ICategorieService
    {
        private readonly IGebruikersTestRepository repository;
        private readonly IMapper<Categorie, CategorieDTO> mapper;

        public CategorieService(
            IGebruikersTestRepository repository,
            IMapper<Categorie,CategorieDTO> mapper
            )
        {
            this.repository = repository ?? throw new ArgumentNullException( nameof( repository ) );
            this.mapper = mapper ?? throw new ArgumentNullException(nameof( mapper ) );
            
        }

       

        public List<Categorie> HaalAlleCategorieënOp() 
        {
            var dtos = repository.HaalAlleCategorieënOp();
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList();

        }


        private List<Categorie> MapCategorieLijst(IEnumerable<CategorieDTO> dtos)
        {
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList() ;
        }


        public Categorie GetCategorieOpId(int id) 
        {
            return mapper.MapToDomain(repository.GetCategorieOnId(id));

        }

    }
}
