using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.ChangeControl;

/// <summary>
/// Documents a contribution of a set of versions added to a change-controlled repository.
/// Corresponds to CONTRIBUTION in openEHR RM 1.1.0 common.change_control.
/// </summary>
public sealed class Contribution : IValidatableObject
{
    /// <summary>Unique identifier of this contribution.</summary>
    public required HierObjectId Uid { get; init; }

    /// <summary>Set of references to versions in this contribution.</summary>
    public required IReadOnlySet<ObjectRef> Versions { get; init; }

    /// <summary>Audit trail of this contribution.</summary>
    public required AuditDetails Audit { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Uid is null)
            yield return new ValidationResult(
                "Invariant violated: Contribution.Uid must not be null.",
                [nameof(Uid)]);

        if (Versions is null || Versions.Count == 0)
            yield return new ValidationResult(
                "Invariant violated: Contribution.Versions must not be empty.",
                [nameof(Versions)]);

        if (Audit is null)
            yield return new ValidationResult(
                "Invariant violated: Contribution.Audit must not be null.",
                [nameof(Audit)]);
    }
}
