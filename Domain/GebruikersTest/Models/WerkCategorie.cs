﻿namespace Domain.GebruikersTest.Models
{
    public class WerkCategorie
    {
        public int WerkId { get; private set; }
        public int CategorieId { get; private set; }

        private WerkCategorie(int werkId, int categorieId)
        {
            WerkId = werkId;
            CategorieId = categorieId;
        }

        public static WerkCategorie Maak(int werkId, int categorieId)
        {
            return new WerkCategorie(werkId, categorieId);
        }
    }
}
