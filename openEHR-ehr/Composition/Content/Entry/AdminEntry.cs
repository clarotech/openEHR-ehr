using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.ItemStructure;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Entry sub-type for administrative information, i.e. information about setting up
/// the clinical process, but not about the clinical process itself.
/// Examples: appointments, demographic data.
/// Corresponds to ADMIN_ENTRY in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class AdminEntry : Entry
{
    /// <summary>Administrative content of this entry.</summary>
    public required ItemStructure Data { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Data is null)
            yield return new ValidationResult(
                "Invariant violated: AdminEntry.Data must not be null.",
                [nameof(Data)]);
    }
}
