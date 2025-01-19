using Domain.Common.Interfaces;
using Domain.Werk.Interfaces;
using MySqlConnector;

namespace Infrastructure.Repos_DB
{
    public class WerkCategorieRepositoryDB : IWerkCategorieRepository
    {
        private readonly IDatabaseService databaseService;

        public WerkCategorieRepositoryDB(IDatabaseService databaseService)
        {
            this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public List<string> HaalCategorieënOpVoorWerk(int werkId)
        {
            var categorieën = new List<string>();
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection, @"
                SELECT c.name
                FROM work_categories wc
                JOIN categories c ON wc.category_id = c.id
                WHERE wc.work_id = @werkId");
            
            command.AddParameter("@werkId", werkId);
            
            using var reader = (MySqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                var naam = reader.GetString("name");
                if (!string.IsNullOrEmpty(naam))
                {
                    categorieën.Add(naam);
                }
            }
            
            return categorieën;
        }

        public void VoegCategorieToe(int werkId, int categorieId)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection,
                "INSERT INTO work_categories (work_id, category_id) VALUES (@werkId, @categorieId)");
            
            command.AddParameter("@werkId", werkId);
            command.AddParameter("@categorieId", categorieId);
            
            command.ExecuteNonQuery();
        }
    }
}