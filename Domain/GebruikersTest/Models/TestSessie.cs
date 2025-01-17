using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class TestSessie
    {
        public int HuidigeVraagNummer { get; private set; }
        public Dictionary<int, int> Affiniteiten { get; private set; }
        public Dictionary<int, int> Antwoorden { get; private set; }

        private TestSessie()
        {
            HuidigeVraagNummer = 0;
            Affiniteiten = new Dictionary<int, int>();
            Antwoorden = new Dictionary<int, int>();
        }

        public static TestSessie Start()
        {
            return new TestSessie();
        }

        public void VoegAntwoordToe(int vraagId, int antwoord)
        {
            if (HuidigeVraagNummer < Affiniteiten.Count)
            {
                Affiniteiten[vraagId] = antwoord;
            }
            else
            {
                Antwoorden[vraagId] = antwoord;
            }
            HuidigeVraagNummer++;
        }

      

        public void Reset()
        {
            HuidigeVraagNummer = 0;
            Affiniteiten.Clear();
            Antwoorden.Clear();
        }

        public bool HeeftAntwoorden()
        {
            return HuidigeVraagNummer > 0 || Affiniteiten.Any() || Antwoorden.Any();
        }
    }
}
