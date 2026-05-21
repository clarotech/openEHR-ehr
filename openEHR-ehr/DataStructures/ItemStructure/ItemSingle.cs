using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.Representation;

namespace OpenEHR.RM.DataStructures.ItemStructure;

/// <summary>
/// ITEM_STRUCTURE that holds a single ELEMENT.
/// Corresponds to ITEM_SINGLE in openEHR RM 1.1.0 data_structures.item_structure.
/// </summary>
public sealed class ItemSingle : ItemStructure
{
    /// <summary>The single element held by this structure.</summary>
    public required Element Item { get; init; }

    public override Cluster AsHierarchy() =>
        new()
        {
            Name = Name,
            ArchetypeNodeId = ArchetypeNodeId,
            Items = [Item]
        };

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Item is null)
            yield return new ValidationResult(
                "Invariant violated: ItemSingle.Item must not be null.",
                [nameof(Item)]);
    }
}
