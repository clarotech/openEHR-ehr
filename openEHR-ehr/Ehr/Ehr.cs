using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// The EHR object is the root object and access point of an EHR for a subject of care.
/// Corresponds to EHR in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class Ehr : IValidatableObject
{
    /// <summary>
    /// The identifier of the logical EHR management system in which this EHR was created.
    /// </summary>
    public required HierObjectId SystemId { get; init; }

    /// <summary>The unique identifier of this EHR.</summary>
    public required HierObjectId EhrId { get; init; }

    /// <summary>Time at which the EHR was created.</summary>
    public required DvDateTime TimeCreated { get; init; }

    /// <summary>
    /// List of contributions to this EHR. Each contribution references the versions
    /// committed as part of a single transaction.
    /// </summary>
    public IReadOnlyList<ObjectRef>? Contributions { get; init; }

    /// <summary>Reference to the EHR_STATUS object for this EHR.</summary>
    public required ObjectRef EhrStatus { get; init; }

    /// <summary>Reference to the EHR_ACCESS object for this EHR.</summary>
    public required ObjectRef EhrAccess { get; init; }

    /// <summary>
    /// Master list of all compositions in this EHR, as references.
    /// </summary>
    public IReadOnlyList<ObjectRef>? Compositions { get; init; }

    /// <summary>
    /// Optional reference to the first folder in the directory structure of this EHR.
    /// </summary>
    public ObjectRef? Directory { get; init; }

    /// <summary>
    /// Optional list of references to additional folder structures in this EHR.
    /// </summary>
    public IReadOnlyList<ObjectRef>? Folders { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SystemId is null)
            yield return new ValidationResult(
                "Invariant violated: Ehr.SystemId must not be null.",
                [nameof(SystemId)]);

        if (EhrId is null)
            yield return new ValidationResult(
                "Invariant violated: Ehr.EhrId must not be null.",
                [nameof(EhrId)]);

        if (TimeCreated is null)
            yield return new ValidationResult(
                "Invariant violated: Ehr.TimeCreated must not be null.",
                [nameof(TimeCreated)]);

        if (EhrStatus is null)
            yield return new ValidationResult(
                "Invariant violated: Ehr.EhrStatus must not be null.",
                [nameof(EhrStatus)]);

        if (EhrAccess is null)
            yield return new ValidationResult(
                "Invariant violated: Ehr.EhrAccess must not be null.",
                [nameof(EhrAccess)]);
    }
}
