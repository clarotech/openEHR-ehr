using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Proxy for a party known by relationship to the subject of the EHR, e.g. "mother", "brother".
/// Corresponds to PARTY_RELATED in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class PartyRelated : PartyIdentified
{
    /// <summary>
    /// Relationship of this party to the subject of the EHR, e.g. "mother", "carer".
    /// Should be coded, e.g. from the openEHR <c>subject relationship</c> terminology.
    /// </summary>
    public required DvCodedText Relationship { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Relationship is null)
            yield return new ValidationResult(
                "Invariant violated: PartyRelated.Relationship must not be null.",
                [nameof(Relationship)]);
    }
}
