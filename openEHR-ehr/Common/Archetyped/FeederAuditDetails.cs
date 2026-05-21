using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.DataStructures.ItemStructure;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// Audit details for any system that has created or committed the item to a transfer
/// from the primary EHR system.
/// Corresponds to FEEDER_AUDIT_DETAILS in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public sealed class FeederAuditDetails : IValidatableObject
{
    /// <summary>Identifier of the system that originated this item.</summary>
    public required string SystemId { get; init; }

    /// <summary>Identifier of the location within the system, if any.</summary>
    public Generic.PartyIdentified? Location { get; init; }

    /// <summary>Identity of patient, as understood by the originating system.</summary>
    public Generic.PartyProxy? Subject { get; init; }

    /// <summary>Identity of the clinician or organisation entering the data.</summary>
    public Generic.PartyProxy? Provider { get; init; }

    /// <summary>Time of creation of the item in the originating system.</summary>
    public DvDateTime? Time { get; init; }

    /// <summary>
    /// Version identifier as used by the originating system.
    /// </summary>
    public string? VersionId { get; init; }

    /// <summary>Any other audit information not fitting the standard fields.</summary>
    public ItemStructure? OtherDetails { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(SystemId))
            yield return new ValidationResult(
                "Invariant violated: FeederAuditDetails.SystemId must not be empty.",
                [nameof(SystemId)]);
    }
}
