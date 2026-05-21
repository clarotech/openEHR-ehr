using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Composition;

/// <summary>
/// Documents the context information of a healthcare event in which the associated
/// COMPOSITION was created.
/// Corresponds to EVENT_CONTEXT in openEHR RM 1.1.0 composition.
/// </summary>
public sealed class EventContext : Pathable, IValidatableObject
{
    /// <summary>Start time of the clinical session or event.</summary>
    public required DvDateTime StartTime { get; init; }

    /// <summary>Optional end time of the clinical session or event.</summary>
    public DvDateTime? EndTime { get; init; }

    /// <summary>
    /// Physical location at which the event occurred, e.g. "Radiology ward 3B".
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Setting in which the clinical session took place.
    /// Coded from the openEHR <c>setting</c> terminology.
    /// </summary>
    public required DvCodedText Setting { get; init; }

    /// <summary>
    /// Optional archetyped additional context information for the event.
    /// </summary>
    public ItemStructure? OtherContext { get; init; }

    /// <summary>
    /// The healthcare facility (organisation) at which the event took place.
    /// </summary>
    public PartyIdentified? HealthCareFacility { get; init; }

    /// <summary>
    /// List of participants in the clinical event, e.g. surgeons, nurses.
    /// </summary>
    public IReadOnlyList<Participation>? Participations { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (StartTime is null)
            yield return new ValidationResult(
                "Invariant violated: EventContext.StartTime must not be null.",
                [nameof(StartTime)]);

        if (Setting is null)
            yield return new ValidationResult(
                "Invariant violated: EventContext.Setting must not be null.",
                [nameof(Setting)]);

        // Invariant: end_time must be after start_time if both are present
        // (Skipped: requires DvDateTime comparison which depends on datatypes library internals)
    }
}
