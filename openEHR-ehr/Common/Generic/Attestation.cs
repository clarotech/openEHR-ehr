using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Encapsulated;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Record an attestation by a party of the content of a version.
/// Extends <see cref="AuditDetails"/> with attestation-specific fields.
/// Corresponds to ATTESTATION in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class Attestation : AuditDetails
{
    /// <summary>Optional visual representation of what was attested.</summary>
    public DvMultimedia? AttestedView { get; init; }

    /// <summary>Optional proof of attestation, e.g. a digital signature.</summary>
    public string? Proof { get; init; }

    /// <summary>Items attested, as EHR URIs.</summary>
    public IReadOnlyList<DvEhrUri>? Items { get; init; }

    /// <summary>Reason for attestation, e.g. "witnessed", "verified".</summary>
    public required DvText Reason { get; init; }

    /// <summary>True if this attestation is still pending.</summary>
    public required bool IsPending { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Reason is null)
            yield return new ValidationResult(
                "Invariant violated: Attestation.Reason must not be null.",
                [nameof(Reason)]);
    }
}
