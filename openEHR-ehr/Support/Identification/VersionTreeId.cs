using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Identifier of a version in a version tree.
/// Value format: &lt;trunk_version&gt;[.&lt;branch_number&gt;.&lt;branch_version&gt;]
/// e.g. "1", "1.1.1", "2.1.3".
/// Corresponds to VERSION_TREE_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class VersionTreeId : IValidatableObject
{
    /// <summary>The string value of the version tree identifier.</summary>
    public required string Value { get; init; }

    private string[] Parts => Value?.Split('.') ?? [];

    /// <summary>The trunk version number, e.g. "1" in "1.1.3".</summary>
    public string TrunkVersion => Parts.Length > 0 ? Parts[0] : string.Empty;

    /// <summary>Returns true if this version is a branch version.</summary>
    public bool IsBranch => Parts.Length == 3;

    /// <summary>
    /// The branch number component, e.g. "1" in "1.1.3". Null if not a branch.
    /// </summary>
    public string? BranchNumber => IsBranch ? Parts[1] : null;

    /// <summary>
    /// The branch version component, e.g. "3" in "1.1.3". Null if not a branch.
    /// </summary>
    public string? BranchVersion => IsBranch ? Parts[2] : null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            yield return new ValidationResult(
                "Invariant violated: VersionTreeId.Value must not be empty.",
                [nameof(Value)]);
            yield break;
        }

        var parts = Parts;
        if (parts.Length != 1 && parts.Length != 3)
            yield return new ValidationResult(
                "Invariant violated: VersionTreeId.Value must be of form '<trunk>' or '<trunk>.<branch_number>.<branch_version>'.",
                [nameof(Value)]);

        foreach (var part in parts)
            if (!int.TryParse(part, out _))
                yield return new ValidationResult(
                    $"Invariant violated: VersionTreeId component '{part}' must be an integer.",
                    [nameof(Value)]);
    }

    public override string ToString() => Value;
}
