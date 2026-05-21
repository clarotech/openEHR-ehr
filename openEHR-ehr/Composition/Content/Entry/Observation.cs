using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.History;
using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Entry type for observations, measurements, or findings.
/// Corresponds to OBSERVATION in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public sealed class Observation : CareEntry
{
    /// <summary>
    /// The data of this observation — a time series of measurements.
    /// </summary>
    public required History<IS> Data { get; init; }

    /// <summary>
    /// Optional state information corresponding to the data events.
    /// </summary>
    public History<IS>? State { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Data is null)
            yield return new ValidationResult(
                "Invariant violated: Observation.Data must not be null.",
                [nameof(Data)]);
    }
}
