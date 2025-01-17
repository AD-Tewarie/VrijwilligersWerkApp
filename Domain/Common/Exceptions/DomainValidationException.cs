namespace Domain.Common.Exceptions
{
    public class DomainValidationException : Exception
    {
        public IEnumerable<string> Fouten { get; }

        public DomainValidationException(string message, IEnumerable<string> fouten)
            : base(message)
        {
            Fouten = fouten;
        }
    }
}
