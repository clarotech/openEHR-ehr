using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.ChangeControl;

/// <summary>
/// Represents a version whose content was imported from another EHR system.
/// The imported item is wrapped in an <see cref="OriginalVersion{T}"/> from the source system.
/// Corresponds to IMPORTED_VERSION&lt;T&gt; in openEHR RM 1.1.0 common.change_control.
/// </summary>
public sealed class ImportedVersion<T> : Version<T>
{
    /// <summary>The original version being imported.</summary>
    public required OriginalVersion<T> Item { get; init; }

    // Computed from Item — the init accessor is inherited from Version<T> but should not be used.
    /// <inheritdoc/>
    public override ObjectVersionId? Uid => Item.Uid;

    /// <inheritdoc/>
    public override ObjectVersionId? PrecedingVersionUid => Item.PrecedingVersionUid;

    /// <inheritdoc/>
    public override T? Data => Item.Data;

    /// <inheritdoc/>
    public override DvCodedText? LifecycleState => Item.LifecycleState;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Item is null)
            yield return new ValidationResult(
                "Invariant violated: ImportedVersion.Item must not be null.",
                [nameof(Item)]);
    }
}
