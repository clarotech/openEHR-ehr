using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

namespace OpenEHR.RM.DataStructures.History;

/// <summary>
/// Root object of a linear time series, where each entry is an EVENT of some kind.
/// Corresponds to HISTORY&lt;T&gt; in openEHR RM 1.1.0 data_structures.history.
/// </summary>
public sealed class History<T> : DataStructure where T : IS
{
    /// <summary>Time origin of this history, i.e. time of first event.</summary>
    public required DvDateTime Origin { get; init; }

    /// <summary>
    /// The events in this history. If set, must contain at least one event.
    /// </summary>
    public IReadOnlyList<Event<T>>? Events { get; init; }

    /// <summary>
    /// Optional period between samples in a periodic history, e.g. 1 second.
    /// Only valid when <see cref="IsPeriodic"/> is true.
    /// </summary>
    public DvDuration? Period { get; init; }

    /// <summary>Optional total duration of this history.</summary>
    public DvDuration? Duration { get; init; }

    /// <summary>Optional summary of the entire history, e.g. a 24-hour average.</summary>
    public IS? Summary { get; init; }

    /// <summary>Returns true if this is a periodic history (has a period).</summary>
    public bool IsPeriodic => Period is not null;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Origin is null)
            yield return new ValidationResult(
                "Invariant violated: History.Origin must not be null.",
                [nameof(Origin)]);

        if (Events is not null && Events.Count == 0)
            yield return new ValidationResult(
                "Invariant violated: History.Events must contain at least one event if set.",
                [nameof(Events)]);
    }
}
