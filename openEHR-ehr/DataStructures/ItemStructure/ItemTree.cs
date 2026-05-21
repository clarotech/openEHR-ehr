using OpenEHR.RM.DataStructures.Representation;

namespace OpenEHR.RM.DataStructures.ItemStructure;

/// <summary>
/// ITEM_STRUCTURE that holds a tree of ITEM objects (CLUSTERs and ELEMENTs).
/// Corresponds to ITEM_TREE in openEHR RM 1.1.0 data_structures.item_structure.
/// </summary>
public sealed class ItemTree : ItemStructure
{
    /// <summary>
    /// Ordered list of items constituting this tree. May be a mix of CLUSTERs and ELEMENTs.
    /// </summary>
    public IReadOnlyList<Item>? Items { get; init; }

    public override Cluster AsHierarchy() =>
        new()
        {
            Name = Name,
            ArchetypeNodeId = ArchetypeNodeId,
            Items = Items?.ToList() ?? []
        };
}
