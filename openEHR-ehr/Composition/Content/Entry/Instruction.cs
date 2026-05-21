using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataTypes.Encapsulated;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Entry type for orders, prescriptions, or other instructions to be carried out.
/// Corresponds to INSTRUCTION in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class Instruction : CareEntry
{
    /// <summary>
    /// Human-readable narrative description of the instruction as a whole,
    /// which must be comprehensible to a clinician without the structured content.
    /// </summary>
    public required DvText Narrative { get; init; }

    /// <summary>
    /// Ordered list of activities that make up this instruction.
    /// </summary>
    public IReadOnlyList<Activity>? Activities { get; init; }

    /// <summary>
    /// Optional date/time after which this instruction should no longer be executed.
    /// </summary>
    public DvDateTime? ExpiryTime { get; init; }

    /// <summary>
    /// Optional machine-processable workflow definition for this instruction,
    /// expressed in a formal workflow language.
    /// </summary>
    public DvParsable? WfDefinition { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Narrative is null)
            yield return new ValidationResult(
                "Invariant violated: Instruction.Narrative must not be null.",
                [nameof(Narrative)]);
    }
}
