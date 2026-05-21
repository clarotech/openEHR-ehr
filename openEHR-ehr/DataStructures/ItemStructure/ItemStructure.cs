using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.DataStructures.Representation;

namespace OpenEHR.RM.DataStructures.ItemStructure;

/// <summary>
/// Abstract parent class of all item structure types.
/// Corresponds to ITEM_STRUCTURE in openEHR RM 1.1.0 data_structures.item_structure.
/// </summary>
public abstract class ItemStructure : DataStructure
{
    /// <summary>
    /// Conversion of the item structure to a single CLUSTER representation.
    /// </summary>
    public abstract Cluster AsHierarchy();
}
