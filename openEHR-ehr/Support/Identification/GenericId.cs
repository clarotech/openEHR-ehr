using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Generic identifier type for identifiers whose format is not known to openEHR.
/// Corresponds to GENERIC_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class GenericId : ObjectId
{
    /// <summary>
    /// The identifier of the scheme within which the identifier <see cref="ObjectId.Value"/> is unique.
    /// </summary>
    public required string Scheme { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (string.IsNullOrWhiteSpace(Scheme))
            yield return new ValidationResult(
                "Invariant violated: GenericId.Scheme must not be empty.",
                [nameof(Scheme)]);
    }
}
