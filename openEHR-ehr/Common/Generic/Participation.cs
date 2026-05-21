using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Model of a participation of a party in an activity. Used to record past and
/// planned participations of parties (clinicians, devices) in activities.
/// Corresponds to PARTICIPATION in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class Participation : IValidatableObject
{
    /// <summary>
    /// The party participating in the activity, e.g. a clinician or device.
    /// </summary>
    public required PartyProxy Performer { get; init; }

    /// <summary>
    /// The function of the performer in this participation, e.g. "primary surgeon".
    /// If the party is the subject of the EHR, the value should be coded from the
    /// openEHR terminology.
    /// </summary>
    public required DvText Function { get; init; }

    /// <summary>
    /// The mode of the performer, e.g. "physically present", "present by video".
    /// Coded from the openEHR <c>participation mode</c> terminology.
    /// </summary>
    public DvCodedText? Mode { get; init; }

    /// <summary>Optional time interval during which this participation occurred.</summary>
    public DvInterval<DataTypes.DateTime.DvDateTime>? Time { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Performer is null)
            yield return new ValidationResult(
                "Invariant violated: Participation.Performer must not be null.",
                [nameof(Performer)]);

        if (Function is null)
            yield return new ValidationResult(
                "Invariant violated: Participation.Function must not be null.",
                [nameof(Function)]);
    }
}
