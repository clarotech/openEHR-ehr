using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Abstract concept of a proxy description of a party, including an optional link to data
/// for that party in a demographic or other identity service.
/// Corresponds to PARTY_PROXY in openEHR RM 1.1.0 common.generic.
/// </summary>
public abstract class PartyProxy : IValidatableObject
{
    /// <summary>
    /// Optional reference to data about the party in a demographic or other service.
    /// </summary>
    public PartyRef? ExternalRef { get; init; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }
}
