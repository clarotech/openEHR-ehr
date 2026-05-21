using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.DateTime;

namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Defines the notion of a revision history of a versioned item, as a list of
/// <see cref="RevisionHistoryItem"/> objects, each containing version id and
/// audit trail information.
/// Corresponds to REVISION_HISTORY in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class RevisionHistory : IValidatableObject
{
    /// <summary>The items in this history in most-recent-first order.</summary>
    public required IReadOnlyList<RevisionHistoryItem> Items { get; init; }

    /// <summary>
    /// The version id string of the most recent version in this history.
    /// </summary>
    public string MostRecentVersion =>
        Items.Count > 0 ? Items[0].VersionId.Value : string.Empty;

    /// <summary>
    /// The time of committal of the most recent version in this history.
    /// </summary>
    public DvDateTime? MostRecentVersionTimeCommitted =>
        Items.Count > 0 && Items[0].Audits.Count > 0
            ? Items[0].Audits[0].TimeCommitted
            : null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Items is null || Items.Count == 0)
            yield return new ValidationResult(
                "Invariant violated: RevisionHistory.Items must not be empty.",
                [nameof(Items)]);
    }
}
