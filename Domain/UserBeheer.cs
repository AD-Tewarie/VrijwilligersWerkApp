using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Domain.WachtwoordStrategy;
using Infrastructure.DTO;
using Infrastructure.Interfaces;

namespace Domain
{
    public class UserBeheer : IUserBeheer
    {
        private readonly IUserRepository repository;
        private readonly IMapper<User, UserDTO> mapper;
        private readonly IWachtwoordStrategy wachtwoordStrategy;

        public UserBeheer(
            IUserRepository repository,
            IMapper<User, UserDTO> mapper,
            IWachtwoordStrategy wachtwoordStrategy)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.wachtwoordStrategy = wachtwoordStrategy ?? throw new ArgumentNullException(nameof(wachtwoordStrategy));
        }

        public void VoegGebruikerToe(string naam, string achterNaam, string email, string wachtwoord)
        {
            ValideerUniekeEmail(email);
            ValideerGebruikerGegevens(naam, achterNaam, email, wachtwoord);

            var user = MaakNieuweGebruiker(naam, achterNaam, email, wachtwoord);
            SlaGebruikerOp(user);
        }

        private void ValideerGebruikerGegevens(string naam, string achterNaam, string email, string wachtwoord)
        {
            var fouten = new List<string>();

            if (string.IsNullOrWhiteSpace(naam))
                fouten.Add("Naam is verplicht.");
            if (string.IsNullOrWhiteSpace(achterNaam))
                fouten.Add("Achternaam is verplicht.");
            if (string.IsNullOrWhiteSpace(email))
                fouten.Add("Email is verplicht.");
            if (string.IsNullOrWhiteSpace(wachtwoord))
                fouten.Add("Wachtwoord is verplicht.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

        private void ValideerUniekeEmail(string email)
        {
            var bestaandeUser = repository.GetUserByEmail(email);
            if (bestaandeUser != null)
            {
                throw new InvalidOperationException("Er bestaat al een gebruiker met dit emailadres.");
            }
        }

        private User MaakNieuweGebruiker(string naam, string achterNaam, string email, string wachtwoord)
        {
            return User.MaakNieuw(naam, achterNaam, email, wachtwoord, wachtwoordStrategy);
        }

        private void SlaGebruikerOp(User user)
        {
            var userDto = mapper.MapToDTO(user);
            repository.AddUser(userDto);
        }

        public List<User> HaalAlleGebruikersOp()
        {
            var dtos = repository.GetUsers();
            return MapGebruikers(dtos);
        }

        private List<User> MapGebruikers(IEnumerable<UserDTO> dtos)
        {
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList();
        }

        public User HaalGebruikerOpID(int userId)
        {
            ValideerGebruikerID(userId);
            var dto = repository.GetUserOnId(userId);
            return dto == null ? null : mapper.MapToDomain(dto);
        }

        private void ValideerGebruikerID(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0.");
        }

        public User HaalGebruikerOpEmail(string email)
        {
            ValideerEmail(email);
            var dto = repository.GetUserByEmail(email);
            return dto == null ? null : mapper.MapToDomain(dto);
        }

        private void ValideerEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email mag niet leeg zijn.");
        }

        public bool ValideerGebruiker(string email, string wachtwoord)
        {
            ValideerInlogGegevens(email, wachtwoord);
            var user = HaalGebruikerOpEmail(email);
            return user?.ValideerWachtwoord(wachtwoord) ?? false;
        }

        private void ValideerInlogGegevens(string email, string wachtwoord)
        {
            var fouten = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
                fouten.Add("Email is verplicht.");
            if (string.IsNullOrWhiteSpace(wachtwoord))
                fouten.Add("Wachtwoord is verplicht.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

        public void VerwijderGebruiker(int userId)
        {
            ValideerGebruikerID(userId);
            ControleerGebruikerBestaat(userId);
            repository.VerwijderUser(userId);
        }

        private void ControleerGebruikerBestaat(int userId)
        {
            var user = HaalGebruikerOpID(userId);
            if (user == null)
                throw new KeyNotFoundException($"Gebruiker met ID {userId} niet gevonden.");
        }

    }
}
