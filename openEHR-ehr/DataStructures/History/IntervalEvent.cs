using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataTypes.Text;
using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

namespace OpenEHR.RM.DataStructures.History;

/// <summary>
/// Defines the notion of a period of time event, e.g. average blood pressure
/// over 24 hours.
/// Corresponds to INTERVAL_EVENT&lt;T&gt; in openEHR RM 1.1.0 data_structures.history.
/// </summary>
public sealed class IntervalEvent<T> : Event<T> where T : IS
{
    /// <summary>Duration of this event. Must be positive.</summary>
    public required DvDuration Width { get; init; }

    /// <summary>
    /// Mathematical function used to compute the event data, e.g. "mean", "maximum".
    /// Coded from the openEHR <c>event math function</c> terminology.
    /// </summary>
    public required DvCodedText MathFunction { get; init; }

    /// <summary>Optional count of individual samples from which this interval event was computed.</summary>
    public int? SampleCount { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Width is null)
            yield return new ValidationResult(
                "Invariant violated: IntervalEvent.Width must not be null.",
                [nameof(Width)]);

        if (MathFunction is null)
            yield return new ValidationResult(
                "Invariant violated: IntervalEvent.MathFunction must not be null.",
                [nameof(MathFunction)]);

        if (SampleCount.HasValue && SampleCount.Value < 1)
            yield return new ValidationResult(
                "Invariant violated: IntervalEvent.SampleCount must be at least 1 if set.",
                [nameof(SampleCount)]);
    }
}
