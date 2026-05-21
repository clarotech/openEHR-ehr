using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

namespace OpenEHR.RM.DataStructures.History;

/// <summary>
/// Defines the notion of a point in time event, i.e. a single observation
/// recorded at a specific moment.
/// Corresponds to POINT_EVENT&lt;T&gt; in openEHR RM 1.1.0 data_structures.history.
/// </summary>
public sealed class PointEvent<T> : Event<T> where T : IS;
