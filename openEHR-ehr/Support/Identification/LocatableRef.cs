using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Reference to a <c>LOCATABLE</c> instance inside the top-level content structure
/// inside a <c>VERSION&lt;T&gt;</c>, with optional path to a specific item.
/// Corresponds to LOCATABLE_REF in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class LocatableRef : ObjectRef
{
    /// <summary>
    /// The path to the referred item within the top-level content object.
    /// If null, the reference is to the top-level object itself.
    /// </summary>
    public string? Path { get; init; }

    /// <summary>
    /// Returns a <see cref="DvEhrUri"/> built from the id and path of this reference.
    /// </summary>
    public DvEhrUri AsUri()
    {
        var uri = Path is not null
            ? $"ehr://{Id.Value}/{Path}"
            : $"ehr://{Id.Value}";
        return new DvEhrUri(uri);
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;
    }
}
