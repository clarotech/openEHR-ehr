using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Entry type for documenting the execution of an instruction.
/// Records what was done, when, and the resulting instruction state transition.
/// Corresponds to ACTION in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class Action : CareEntry
{
    /// <summary>Time at which this action was performed.</summary>
    public required DvDateTime Time { get; init; }

    /// <summary>
    /// Structured description of the action that was performed.
    /// </summary>
    public required ItemStructure Description { get; init; }

    /// <summary>
    /// The ISM transition that occurred as a result of performing this action.
    /// </summary>
    public required IsmTransition IsmTransition { get; init; }

    /// <summary>
    /// Optional details linking this action to the instruction that caused it.
    /// </summary>
    public InstructionDetails? InstructionDetails { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Time is null)
            yield return new ValidationResult(
                "Invariant violated: Action.Time must not be null.",
                [nameof(Time)]);

        if (Description is null)
            yield return new ValidationResult(
                "Invariant violated: Action.Description must not be null.",
                [nameof(Description)]);

        if (IsmTransition is null)
            yield return new ValidationResult(
                "Invariant violated: Action.IsmTransition must not be null.",
                [nameof(IsmTransition)]);
    }
}
