using OpenEHR.RM.Common.Archetyped;

namespace OpenEHR.RM.DataStructures;

/// <summary>
/// Abstract parent class of all openEHR data structure types.
/// Corresponds to DATA_STRUCTURE in openEHR RM 1.1.0 data_structures.
/// </summary>
public abstract class DataStructure : Locatable;
