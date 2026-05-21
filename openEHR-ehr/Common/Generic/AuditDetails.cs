using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// The set of attributes required to document the committal of an information item
/// to a repository.
/// Corresponds to AUDIT_DETAILS in openEHR RM 1.1.0 common.generic.
/// </summary>
public class AuditDetails : IValidatableObject
{
    /// <summary>Identifier of the logical EHR system where the change was committed.</summary>
    public required string SystemId { get; init; }

    /// <summary>Identity and optional reference to the committing party.</summary>
    public required PartyProxy Committer { get; init; }

    /// <summary>Time of committal of the item.</summary>
    public required DvDateTime TimeCommitted { get; init; }

    /// <summary>
    /// Type of change: creation, amendment, correction, synthesis, unknown, deleted.
    /// Coded from the openEHR <c>audit change type</c> terminology.
    /// </summary>
    public required DvCodedText ChangeType { get; init; }

    /// <summary>Reason for committal, if any.</summary>
    public DvText? Description { get; init; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(SystemId))
            yield return new ValidationResult(
                "Invariant violated: AuditDetails.SystemId must not be empty.",
                [nameof(SystemId)]);

        if (Committer is null)
            yield return new ValidationResult(
                "Invariant violated: AuditDetails.Committer must not be null.",
                [nameof(Committer)]);

        if (TimeCommitted is null)
            yield return new ValidationResult(
                "Invariant violated: AuditDetails.TimeCommitted must not be null.",
                [nameof(TimeCommitted)]);

        if (ChangeType is null)
            yield return new ValidationResult(
                "Invariant violated: AuditDetails.ChangeType must not be null.",
                [nameof(ChangeType)]);
    }
}
