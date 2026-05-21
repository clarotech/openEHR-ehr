using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Encapsulated;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// Audit and provenance information for non-openEHR data committed to the EHR
/// as part of feeder system integration.
/// Corresponds to FEEDER_AUDIT in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public sealed class FeederAudit : IValidatableObject
{
    /// <summary>
    /// Identifiers used for the item in the originating system, e.g. lab sample identifiers.
    /// </summary>
    public IReadOnlyList<DvIdentifier>? OriginatingSystemItemIds { get; init; }

    /// <summary>
    /// Identifiers used for the item in the feeder system, where they differ from those
    /// in the originating system.
    /// </summary>
    public IReadOnlyList<DvIdentifier>? FeederSystemItemIds { get; init; }

    /// <summary>
    /// Optional representation of the original content as received, in whatever form this
    /// was — plain text, RTF, HTML, DICOM, etc.
    /// </summary>
    public DvEncapsulated? OriginalContent { get; init; }

    /// <summary>Audit details of the originating system.</summary>
    public required FeederAuditDetails OriginatingSystemAudit { get; init; }

    /// <summary>Audit details of the feeder system, if different from the originating system.</summary>
    public FeederAuditDetails? FeederSystemAudit { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OriginatingSystemAudit is null)
            yield return new ValidationResult(
                "Invariant violated: FeederAudit.OriginatingSystemAudit must not be null.",
                [nameof(OriginatingSystemAudit)]);
    }
}
