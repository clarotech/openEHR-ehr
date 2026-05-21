using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.DataStructures.ItemStructure;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// Single object per EHR, containing key summary status information about the EHR as a whole.
/// Corresponds to EHR_STATUS in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class EhrStatus : Locatable
{
    /// <summary>
    /// The subject of this EHR. The external_ref attribute is used to link to a patient
    /// record in a demographic or identity management service.
    /// </summary>
    public required PartySelf Subject { get; init; }

    /// <summary>
    /// If true, this EHR is included in population queries. Default is true.
    /// </summary>
    public required bool IsQueryable { get; init; }

    /// <summary>
    /// If true, the EHR can be modified. Default is true.
    /// Set to false to create a read-only EHR (e.g. for deceased patients).
    /// </summary>
    public required bool IsModifiable { get; init; }

    /// <summary>
    /// Optional additional status information in archetyped form.
    /// </summary>
    public ItemStructure? OtherDetails { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Subject is null)
            yield return new ValidationResult(
                "Invariant violated: EhrStatus.Subject must not be null.",
                [nameof(Subject)]);
    }
}
