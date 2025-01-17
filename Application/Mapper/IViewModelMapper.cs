using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public interface IViewModelMapper<TViewModel, TDomain>
    {
        TViewModel MapNaarViewModel(TDomain domainModel);
        TDomain MapNaarDomainModel(TViewModel viewModel);
    }
}
