using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Model of a transition in the Instruction State Machine (ISM), used to record
/// the execution state of an instruction.
/// Corresponds to ISM_TRANSITION in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class IsmTransition : Pathable, IValidatableObject
{
    /// <summary>
    /// The current state of the instruction, coded from the openEHR
    /// <c>instruction states</c> terminology.
    /// </summary>
    public required DvCodedText CurrentState { get; init; }

    /// <summary>
    /// Optional transition from a prior state to the current state, coded from the
    /// openEHR <c>instruction transitions</c> terminology.
    /// </summary>
    public DvCodedText? Transition { get; init; }

    /// <summary>
    /// Optional clinical workflow step that this transition represents,
    /// defined by the archetype.
    /// </summary>
    public DvCodedText? CareflowStep { get; init; }

    /// <summary>Optional reasons for this state transition.</summary>
    public IReadOnlyList<DvText>? Reason { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (CurrentState is null)
            yield return new ValidationResult(
                "Invariant violated: IsmTransition.CurrentState must not be null.",
                [nameof(CurrentState)]);
    }
}
