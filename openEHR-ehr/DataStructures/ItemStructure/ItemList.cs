using OpenEHR.RM.DataStructures.Representation;

namespace OpenEHR.RM.DataStructures.ItemStructure;

/// <summary>
/// ITEM_STRUCTURE that holds a flat list of ELEMENTs.
/// Corresponds to ITEM_LIST in openEHR RM 1.1.0 data_structures.item_structure.
/// </summary>
public sealed class ItemList : ItemStructure
{
    /// <summary>Ordered list of items in this structure. May be empty.</summary>
    public IReadOnlyList<Element>? Items { get; init; }

    /// <summary>Returns the count of elements.</summary>
    public int Count => Items?.Count ?? 0;

    /// <summary>Returns the element with the given name, or null.</summary>
    public Element? ElementAtName(string name) =>
        Items?.FirstOrDefault(e => e.Name?.Value == name);

    public override Cluster AsHierarchy() =>
        new()
        {
            Name = Name,
            ArchetypeNodeId = ArchetypeNodeId,
            Items = Items?.Cast<Item>().ToList() ?? []
        };
}
