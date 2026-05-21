using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Abstract parent of classes whose instances are unique identifiers of information entities.
/// Corresponds to UID in openEHR RM 1.1.0 support.identification.
/// </summary>
public abstract class Uid : IValidatableObject
{
    /// <summary>The string value of this UID.</summary>
    public required string Value { get; init; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Value))
            yield return new ValidationResult(
                "Invariant violated: Uid.Value must not be empty.",
                [nameof(Value)]);
    }

    public override string ToString() => Value;
}
