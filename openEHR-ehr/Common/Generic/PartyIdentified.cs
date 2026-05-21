using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Basic;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Proxy data for an identified party other than the subject of the EHR, such as
/// a doctor, nurse, or organisation.
/// Corresponds to PARTY_IDENTIFIED in openEHR RM 1.1.0 common.generic.
/// </summary>
public class PartyIdentified : PartyProxy
{
    /// <summary>Optional human-readable name of the identified party.</summary>
    public string? Name { get; init; }

    /// <summary>
    /// One or more formal identifiers of the party in its home system, e.g. NHS number.
    /// </summary>
    public IReadOnlyList<DvIdentifier>? Identifiers { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        // Invariant: at least one of Name or Identifiers or ExternalRef must be set
        if (Name is null && (Identifiers is null || Identifiers.Count == 0) && ExternalRef is null)
            yield return new ValidationResult(
                "Invariant violated: PartyIdentified must have at least one of Name, Identifiers, or ExternalRef.",
                [nameof(Name), nameof(Identifiers), nameof(ExternalRef)]);
    }
}
