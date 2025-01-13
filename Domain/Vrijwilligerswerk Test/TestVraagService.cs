using Domain.Mapper;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Domain.Vrijwilligerswerk_Test.Models;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestVraagService : ITestVraagService
    {
        private readonly IGebruikersTestRepository repository;
        private readonly IMapper<TestVraag, TestVraagDTO> mapper;

        public TestVraagService(
            IGebruikersTestRepository repository,
            IMapper<TestVraag, TestVraagDTO> mapper
            )
        {
            this.repository = repository ?? throw new ArgumentNullException( nameof( repository ) );
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        public List<TestVraag> HaalAlleTestVragenOp() 
        {
            var dtos = repository.HaalAlleTestVraagOp();
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList();
        }


        public TestVraag GetTestVraagOpId(int id) 
        {
            return mapper.MapToDomain(repository.GetTestVraagOnId(id));
        }


    }
}
