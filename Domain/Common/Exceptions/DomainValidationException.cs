﻿using System.Collections.Generic;

namespace Domain.Common.Exceptions;

public class DomainValidationException : Exception
{
    private readonly Dictionary<string, IReadOnlyCollection<string>> validatieFouten;
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> ValidatieFouten => validatieFouten;
    
    public DomainValidationException(string bericht) : base(bericht)
    {
        validatieFouten = new Dictionary<string, IReadOnlyCollection<string>>();
    }

    public DomainValidationException(string eigenschap, string foutmelding) 
        : this($"Validatie mislukt voor {eigenschap}")
    {
        validatieFouten = new Dictionary<string, IReadOnlyCollection<string>>
        {
            { eigenschap, new[] { foutmelding } }
        };
    }

    public DomainValidationException(string bericht, IDictionary<string, ICollection<string>> validatieFouten) 
        : base(bericht)
    {
        this.validatieFouten = new Dictionary<string, IReadOnlyCollection<string>>();
        foreach (var fout in validatieFouten)
        {
            this.validatieFouten[fout.Key] = fout.Value.ToList().AsReadOnly();
        }
    }

    public DomainValidationException(string bericht, string eigenschap, IEnumerable<string> foutmeldingen) 
        : base(bericht)
    {
        validatieFouten = new Dictionary<string, IReadOnlyCollection<string>>
        {
            { eigenschap, foutmeldingen.ToList().AsReadOnly() }
        };
    }

    public bool HeeftFoutVoor(string eigenschap)
    {
        return validatieFouten.ContainsKey(eigenschap);
    }

    public IReadOnlyCollection<string> HaalFoutenOp(string eigenschap)
    {
        return validatieFouten.TryGetValue(eigenschap, out var fouten) 
            ? fouten 
            : Array.Empty<string>();
    }
}
