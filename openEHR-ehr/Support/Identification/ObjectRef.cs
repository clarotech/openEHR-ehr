using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Class describing a reference to another object, using a namespace, type, and identifier.
/// Corresponds to OBJECT_REF in openEHR RM 1.1.0 support.identification.
/// </summary>
public class ObjectRef : IValidatableObject
{
    /// <summary>
    /// Namespace to which the identifier belongs, e.g. "local", "terminology:LOINC".
    /// </summary>
    public required string Namespace { get; init; }

    /// <summary>
    /// Name of the class of the referenced object, e.g. "COMPOSITION", "PARTY".
    /// The type name "ANY" can be used to indicate that any type is acceptable.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>Global identifier of the object.</summary>
    public required ObjectId Id { get; init; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Namespace))
            yield return new ValidationResult(
                "Invariant violated: ObjectRef.Namespace must not be empty.",
                [nameof(Namespace)]);

        if (string.IsNullOrWhiteSpace(Type))
            yield return new ValidationResult(
                "Invariant violated: ObjectRef.Type must not be empty.",
                [nameof(Type)]);

        if (Id is null)
            yield return new ValidationResult(
                "Invariant violated: ObjectRef.Id must not be null.",
                [nameof(Id)]);
    }
}
