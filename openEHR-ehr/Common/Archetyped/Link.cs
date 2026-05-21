using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// The <c>LINK</c> type defines a logical relationship between two items, such as two
/// <see cref="Locatable"/> instances. Links can be used across EHR compositions.
/// Corresponds to LINK in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public sealed class Link : IValidatableObject
{
    /// <summary>Used defined meaning of the link; can be any DV_TEXT value.</summary>
    public required DvText Meaning { get; init; }

    /// <summary>
    /// The type of link, constrained to a controlled terminology, e.g. "problem", "issue".
    /// </summary>
    public required DvText Type { get; init; }

    /// <summary>The linked item.</summary>
    public required DvEhrUri Target { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Meaning is null)
            yield return new ValidationResult(
                "Invariant violated: Link.Meaning must not be null.",
                [nameof(Meaning)]);

        if (Type is null)
            yield return new ValidationResult(
                "Invariant violated: Link.Type must not be null.",
                [nameof(Type)]);

        if (Target is null)
            yield return new ValidationResult(
                "Invariant violated: Link.Target must not be null.",
                [nameof(Target)]);
    }
}
