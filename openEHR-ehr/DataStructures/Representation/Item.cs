using OpenEHR.RM.Common.Archetyped;

namespace OpenEHR.RM.DataStructures.Representation;

/// <summary>
/// Abstract parent class of all ITEM types, i.e. those that represent
/// leaf or intermediate data items in a data structure.
/// Corresponds to ITEM in openEHR RM 1.1.0 data_structures.representation.
/// </summary>
public abstract class Item : Locatable;
