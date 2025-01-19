﻿using Domain.GebruikersTest.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ITestSessieBeheer
    {
        TestSessie MaakNieuweSessie(int gebruikerId);
        TestSessie HaalOp(int gebruikerId);
        void SlaAntwoordOp(int gebruikerId, int vraagId, int antwoord);
        void SlaAffiniteitOp(int gebruikerId, int categorieId, int score);
        bool IsVoltooid(int gebruikerId);
        void Reset(int gebruikerId);
        (string vraagTekst, bool isKlaar) HaalVolgendeVraag(TestSessie sessie);
        int BerekenVoortgang(TestSessie sessie);
        void Opslaan(int gebruikerId, TestSessie sessie);
        void Verwijder(int gebruikerId);
    }
}
