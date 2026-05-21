using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Defines a single activity within an INSTRUCTION, such as "administer 500mg paracetamol".
/// Corresponds to ACTIVITY in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class Activity : Locatable
{
    /// <summary>
    /// Structured description of the activity, e.g. medication details.
    /// </summary>
    public required ItemStructure Description { get; init; }

    /// <summary>
    /// Optional timing specification for this activity, expressed in a formal language
    /// (e.g. HL7 PIVL or EIVL expressions).
    /// </summary>
    public DvParsable? Timing { get; init; }

    /// <summary>
    /// Identifier of the archetype for the Action that will document execution of this activity.
    /// </summary>
    public required string ActionArchetypeId { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Description is null)
            yield return new ValidationResult(
                "Invariant violated: Activity.Description must not be null.",
                [nameof(Description)]);

        if (string.IsNullOrWhiteSpace(ActionArchetypeId))
            yield return new ValidationResult(
                "Invariant violated: Activity.ActionArchetypeId must not be empty.",
                [nameof(ActionArchetypeId)]);
    }
}
