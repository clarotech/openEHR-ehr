using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Globally unique identifier for one version of a versioned object.
/// Value format: &lt;object_id&gt;::&lt;creating_system_id&gt;::&lt;version_tree_id&gt;
/// e.g. "F7C3A031-CE71-4F37-8F89::example.org::1".
/// Corresponds to OBJECT_VERSION_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class ObjectVersionId : UidBasedId
{
    private string[] Segments => Value?.Split("::", StringSplitOptions.None) ?? [];

    /// <summary>Unique identifier for the logical object being versioned.</summary>
    public string ObjectId => Segments.Length >= 1 ? Segments[0] : string.Empty;

    /// <summary>Identifier of the system that created this version.</summary>
    public string CreatingSystemId => Segments.Length >= 2 ? Segments[1] : string.Empty;

    /// <summary>Version tree identifier for this version within the version tree.</summary>
    public VersionTreeId VersionTreeId => new() { Value = Segments.Length >= 3 ? Segments[2] : string.Empty };

    /// <summary>Returns true if this version is on a branch.</summary>
    public bool IsBranch => VersionTreeId.IsBranch;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Skip UidBasedId base validation — format is different
        if (string.IsNullOrWhiteSpace(Value))
        {
            yield return new ValidationResult(
                "Invariant violated: ObjectVersionId.Value must not be empty.",
                [nameof(Value)]);
            yield break;
        }

        var segments = Segments;
        if (segments.Length != 3)
            yield return new ValidationResult(
                "Invariant violated: ObjectVersionId.Value must be of form '<object_id>::<creating_system_id>::<version_tree_id>'.",
                [nameof(Value)]);

        if (segments.Length >= 3)
        {
            var vtvCtx = new ValidationContext(this) { MemberName = nameof(Value) };
            var vtv = new VersionTreeId { Value = segments[2] };
            foreach (var result in vtv.Validate(vtvCtx))
                yield return result;
        }
    }
}
