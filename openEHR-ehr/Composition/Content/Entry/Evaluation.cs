using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.ItemStructure;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Entry type for clinical assessments, diagnoses, problem statements, and plans.
/// Corresponds to EVALUATION in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class Evaluation : CareEntry
{
    /// <summary>The evaluation data — e.g. a diagnosis, assessment, or care plan.</summary>
    public required ItemStructure Data { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Data is null)
            yield return new ValidationResult(
                "Invariant violated: Evaluation.Data must not be null.",
                [nameof(Data)]);
    }
}
