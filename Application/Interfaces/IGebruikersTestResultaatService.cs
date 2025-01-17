using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGebruikersTestResultaatService
    {
        GebruikersTestResultaatViewModel HaalResultaatOp(int gebruikerId, string presentatieType = "top");
    }
}
