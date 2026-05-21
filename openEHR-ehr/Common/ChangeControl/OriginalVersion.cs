using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.ChangeControl;

/// <summary>
/// A version created locally in an EHR system.
/// Corresponds to ORIGINAL_VERSION&lt;T&gt; in openEHR RM 1.1.0 common.change_control.
/// </summary>
public sealed class OriginalVersion<T> : Version<T>
{
    /// <inheritdoc/>
#pragma warning disable CS8765 // init nullability narrowed intentionally: OriginalVersion.Uid is always non-null
    public override required ObjectVersionId Uid { get; init; }
#pragma warning restore CS8765

    /// <inheritdoc/>
    public override ObjectVersionId? PrecedingVersionUid { get; init; }

    /// <inheritdoc/>
    public override T? Data { get; init; }

    /// <inheritdoc/>
#pragma warning disable CS8765 // init nullability narrowed intentionally: OriginalVersion.LifecycleState is always non-null
    public override required DvCodedText LifecycleState { get; init; }
#pragma warning restore CS8765

    /// <summary>
    /// Attestations of this version, if any. Typically used to record signing or witnessing.
    /// </summary>
    public IReadOnlyList<Attestation>? Attestations { get; init; }

    /// <summary>
    /// Identifiers of other versions merged in during this version's creation.
    /// </summary>
    public IReadOnlyList<ObjectVersionId>? OtherInputVersionUids { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Uid is null)
            yield return new ValidationResult(
                "Invariant violated: OriginalVersion.Uid must not be null.",
                [nameof(Uid)]);

        if (LifecycleState is null)
            yield return new ValidationResult(
                "Invariant violated: OriginalVersion.LifecycleState must not be null.",
                [nameof(LifecycleState)]);
    }
}
