using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// An entry in a <see cref="RevisionHistory"/>, representing the audits for a single version.
/// Corresponds to REVISION_HISTORY_ITEM in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class RevisionHistoryItem : IValidatableObject
{
    /// <summary>Version identifier of this version in the version tree.</summary>
    public required ObjectVersionId VersionId { get; init; }

    /// <summary>The audits for this version, in time order.</summary>
    public required IReadOnlyList<AuditDetails> Audits { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (VersionId is null)
            yield return new ValidationResult(
                "Invariant violated: RevisionHistoryItem.VersionId must not be null.",
                [nameof(VersionId)]);

        if (Audits is null || Audits.Count == 0)
            yield return new ValidationResult(
                "Invariant violated: RevisionHistoryItem.Audits must contain at least one entry.",
                [nameof(Audits)]);
    }
}
