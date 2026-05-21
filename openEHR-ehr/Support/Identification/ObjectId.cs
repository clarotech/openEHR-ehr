using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Abstract ancestor of all identifier types.
/// Corresponds to OBJECT_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public abstract class ObjectId : IValidatableObject
{
    /// <summary>The string value of the identifier.</summary>
    public required string Value { get; init; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Value))
            yield return new ValidationResult(
                "Invariant violated: ObjectId.Value must not be empty.",
                [nameof(Value)]);
    }

    public override string ToString() => Value;
}
