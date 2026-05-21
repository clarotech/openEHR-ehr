using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// Abstract ancestor of all classes whose instances are reachable by path traversal
/// and which know their parent.
/// Corresponds to PATHABLE in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public abstract class Pathable : IValidatableObject
{
    /// <summary>
    /// Returns the parent of this item in the compositional hierarchy.
    /// Returns null if this is the root item.
    /// </summary>
    public virtual Pathable? Parent { get; protected set; }

    /// <summary>
    /// Returns the item at the given path, if it exists; otherwise null.
    /// Path is an openEHR ADL path expression.
    /// </summary>
    public virtual object? ItemAtPath(string path) => null;

    /// <summary>
    /// Returns all items at the given path. Returns empty if path does not exist.
    /// </summary>
    public virtual IEnumerable<object> ItemsAtPath(string path) => [];

    /// <summary>Returns true if the given path exists in this structure.</summary>
    public virtual bool PathExists(string path) => ItemAtPath(path) is not null;

    /// <summary>
    /// Returns true if the path resolves to a unique item (cardinality 0..1 or 1..1).
    /// </summary>
    public virtual bool PathUnique(string path) => true;

    /// <summary>Returns the path of the given item relative to this object.</summary>
    public virtual string PathOfItem(Pathable item) => string.Empty;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }
}
