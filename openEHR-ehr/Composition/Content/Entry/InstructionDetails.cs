using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Used to record details of the Instruction that caused an Action to be performed.
/// Corresponds to INSTRUCTION_DETAILS in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class InstructionDetails : Pathable, IValidatableObject
{
    /// <summary>Reference to the instruction that initiated this action.</summary>
    public required LocatableRef InstructionId { get; init; }

    /// <summary>
    /// Identifier of the activity within the instruction that this action corresponds to.
    /// </summary>
    public required string ActivityId { get; init; }

    /// <summary>Optional workflow details archetype.</summary>
    public ItemStructure? WfDetails { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (InstructionId is null)
            yield return new ValidationResult(
                "Invariant violated: InstructionDetails.InstructionId must not be null.",
                [nameof(InstructionId)]);

        if (string.IsNullOrWhiteSpace(ActivityId))
            yield return new ValidationResult(
                "Invariant violated: InstructionDetails.ActivityId must not be empty.",
                [nameof(ActivityId)]);
    }
}
