using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.DataStructures.Representation;

/// <summary>
/// The grouping variant of ITEM, for structurally complex data.
/// Can contain a mix of ELEMENT and CLUSTER children.
/// Corresponds to CLUSTER in openEHR RM 1.1.0 data_structures.representation.
/// </summary>
public sealed class Cluster : Item
{
    /// <summary>Ordered list of items — either ELEMENTs or nested CLUSTERs.</summary>
    public required IReadOnlyList<Item> Items { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Items is null || Items.Count == 0)
            yield return new ValidationResult(
                "Invariant violated: Cluster.Items must not be empty.",
                [nameof(Items)]);
    }
}
