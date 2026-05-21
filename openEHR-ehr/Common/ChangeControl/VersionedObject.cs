using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.ChangeControl;

/// <summary>
/// Version-controlled container for instances of a particular type.
/// Manages the list of versions and provides derived queries over them.
/// Corresponds to VERSIONED_OBJECT&lt;T&gt; in openEHR RM 1.1.0 common.change_control.
/// </summary>
public abstract class VersionedObject<T> : IValidatableObject
{
    /// <summary>Unique identifier of this versioned object.</summary>
    public required HierObjectId Uid { get; init; }

    /// <summary>Reference to the EHR or other owning object.</summary>
    public required ObjectRef OwnerId { get; init; }

    /// <summary>Time at which this versioned object was created.</summary>
    public required DvDateTime TimeCreated { get; init; }

    /// <summary>All versions of this object, most recent first.</summary>
    public IReadOnlyList<Version<T>> Versions { get; init; } = [];

    /// <summary>Count of all versions.</summary>
    public int VersionCount => Versions.Count;

    /// <summary>Returns all version UIDs (non-null UIDs only).</summary>
    public IEnumerable<ObjectVersionId> AllVersionIds =>
        Versions
            .Select(v => v.Uid)
            .Where(uid => uid is not null)
            .Select(uid => uid!);

    /// <summary>
    /// Returns the most recent version.
    /// Returns null if no versions exist.
    /// </summary>
    public Version<T>? LatestVersion =>
        Versions.Count > 0 ? Versions[0] : null;

    /// <summary>
    /// Returns the most recent version whose trunk version is the highest.
    /// </summary>
    public Version<T>? LatestTrunkVersion =>
        Versions.FirstOrDefault(v => !v.IsBranch);

    /// <summary>Returns true if a version with the given uid exists.</summary>
    public bool HasVersionId(ObjectVersionId versionId) =>
        Versions.Any(v => v.Uid?.Value == versionId.Value);

    /// <summary>Returns true if a version valid at the given time exists.</summary>
    public bool HasVersionAtTime(DvDateTime time) =>
        Versions.Any(v => v.CommitAudit.TimeCommitted.Value == time.Value);

    /// <summary>Returns the version with the given uid, or null.</summary>
    public Version<T>? VersionWithId(ObjectVersionId versionId) =>
        Versions.FirstOrDefault(v => v.Uid?.Value == versionId.Value);

    /// <summary>Returns true if the version with the given uid is an original (not imported) version.</summary>
    public bool IsOriginalVersion(ObjectVersionId versionId) =>
        VersionWithId(versionId) is OriginalVersion<T>;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Uid is null)
            yield return new ValidationResult(
                "Invariant violated: VersionedObject.Uid must not be null.",
                [nameof(Uid)]);

        if (OwnerId is null)
            yield return new ValidationResult(
                "Invariant violated: VersionedObject.OwnerId must not be null.",
                [nameof(OwnerId)]);

        if (TimeCreated is null)
            yield return new ValidationResult(
                "Invariant violated: VersionedObject.TimeCreated must not be null.",
                [nameof(TimeCreated)]);

        // Invariant: all versions share the same owning object id
        foreach (var version in Versions)
        {
            var ownerId = version.OwnerId?.Value;
            if (ownerId is not null && Uid is not null && ownerId != Uid.Value)
                yield return new ValidationResult(
                    $"Invariant violated: All versions must have the same owner id as this VersionedObject (expected '{Uid.Value}', got '{ownerId}').",
                    [nameof(Versions)]);
        }
    }
}
