using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Abstract ancestor of identifiers based on ISO/IEC 11578:1996.
/// Value has the form: &lt;root&gt;[:&lt;extension&gt;] where root is a UID.
/// Corresponds to UID_BASED_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public abstract class UidBasedId : ObjectId
{
    /// <summary>
    /// The identifier of the conceptual namespace in which the object exists.
    /// Derived from the root part of <see cref="ObjectId.Value"/>.
    /// </summary>
    public string Root => Value.Contains(':', StringComparison.Ordinal)
        ? Value[..Value.IndexOf(':', StringComparison.Ordinal)]
        : Value;

    /// <summary>
    /// Optional local identifier of the object within the namespace indicated by <see cref="Root"/>.
    /// Derived from the extension part of <see cref="ObjectId.Value"/>.
    /// </summary>
    public string? Extension => Value.Contains(':', StringComparison.Ordinal)
        ? Value[(Value.IndexOf(':', StringComparison.Ordinal) + 1)..]
        : null;

    /// <summary>Returns true if this identifier has an extension.</summary>
    public bool HasExtension => Extension is not null;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (!string.IsNullOrWhiteSpace(Root) && Root.Length == 0)
            yield return new ValidationResult(
                "Invariant violated: UidBasedId root must not be empty.",
                [nameof(Value)]);
    }
}
