using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.ChangeControl;

/// <summary>
/// Abstract model of a version of an item in a <see cref="VersionedObject{T}"/>.
/// Corresponds to VERSION&lt;T&gt; in openEHR RM 1.1.0 common.change_control.
/// </summary>
public abstract class Version<T> : IValidatableObject
{
    /// <summary>Audit trail corresponding to the committal of this version.</summary>
    public required AuditDetails CommitAudit { get; init; }

    /// <summary>Contribution in which this version was committed.</summary>
    public required ObjectRef Contribution { get; init; }

    /// <summary>Optional OpenPGP digital signature or digest of content.</summary>
    public string? Signature { get; init; }

    // Virtual with init so OriginalVersion<T> can override with required init,
    // and ImportedVersion<T> can override with a computed getter.
    /// <summary>Unique identifier of this version.</summary>
    public virtual ObjectVersionId? Uid { get; init; }

    /// <summary>Identifier of the version preceding this one in the same version tree.</summary>
    public virtual ObjectVersionId? PrecedingVersionUid { get; init; }

    /// <summary>The data of this version.</summary>
    public virtual T? Data { get; init; }

    /// <summary>
    /// Lifecycle state of this version, coded from the openEHR
    /// <c>version lifecycle state</c> terminology.
    /// </summary>
    public virtual DvCodedText? LifecycleState { get; init; }

    /// <summary>The identifier of the owning <see cref="VersionedObject{T}"/>.</summary>
    public HierObjectId? OwnerId => Uid is not null
        ? new() { Value = Uid.ObjectId }
        : null;

    /// <summary>Returns true if this version is on a branch.</summary>
    public bool IsBranch => Uid?.IsBranch ?? false;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CommitAudit is null)
            yield return new ValidationResult(
                "Invariant violated: Version.CommitAudit must not be null.",
                [nameof(CommitAudit)]);

        if (Contribution is null)
            yield return new ValidationResult(
                "Invariant violated: Version.Contribution must not be null.",
                [nameof(Contribution)]);
    }
}
