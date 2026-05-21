using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.DataTypes.DateTime;
using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

namespace OpenEHR.RM.DataStructures.History;

/// <summary>
/// Abstract ancestor of point and interval event types.
/// Corresponds to EVENT&lt;T&gt; in openEHR RM 1.1.0 data_structures.history.
/// </summary>
public abstract class Event<T> : Locatable where T : IS
{
    /// <summary>Time of this event. For interval events, the start time.</summary>
    public required DvDateTime Time { get; init; }

    /// <summary>The data of this event.</summary>
    public required T Data { get; init; }

    /// <summary>Optional state information at the time of this event.</summary>
    public IS? State { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Time is null)
            yield return new ValidationResult(
                "Invariant violated: Event.Time must not be null.",
                [nameof(Time)]);

        if (Data is null)
            yield return new ValidationResult(
                "Invariant violated: Event.Data must not be null.",
                [nameof(Data)]);
    }
}
