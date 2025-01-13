using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public interface IMapper<TDomain, TDto>
    {
        TDto MapToDTO(TDomain domain);
        TDomain MapToDomain(TDto dto);
    }
}
