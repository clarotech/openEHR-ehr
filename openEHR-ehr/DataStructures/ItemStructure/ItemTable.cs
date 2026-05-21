using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataStructures.Representation;

namespace OpenEHR.RM.DataStructures.ItemStructure;

/// <summary>
/// ITEM_STRUCTURE that represents a tabular structure with named columns and typed rows.
/// Rows are CLUSTERs, columns are ELEMENTs within each row CLUSTER.
/// Corresponds to ITEM_TABLE in openEHR RM 1.1.0 data_structures.item_structure.
/// </summary>
public sealed class ItemTable : ItemStructure
{
    /// <summary>
    /// Ordered list of rows in the table. Each row is a CLUSTER whose items
    /// represent the column values.
    /// </summary>
    public IReadOnlyList<Cluster>? Rows { get; init; }

    /// <summary>Number of rows.</summary>
    public int RowCount => Rows?.Count ?? 0;

    /// <summary>
    /// Number of columns, derived from the item count of the first row.
    /// Returns 0 if there are no rows.
    /// </summary>
    public int ColumnCount => Rows?.Count > 0 ? Rows[0].Items.Count : 0;

    /// <summary>Returns true if a row with the given name exists.</summary>
    public bool HasRowWithName(string name) =>
        Rows?.Any(r => r.Name?.Value == name) ?? false;

    /// <summary>Returns the row with the given name, or null.</summary>
    public Cluster? NamedRow(string name) =>
        Rows?.FirstOrDefault(r => r.Name?.Value == name);

    public override Cluster AsHierarchy() =>
        new()
        {
            Name = Name,
            ArchetypeNodeId = ArchetypeNodeId,
            Items = Rows?.Cast<Item>().ToList() ?? []
        };

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        // Invariant: all rows must have the same number of columns
        if (Rows is { Count: > 1 })
        {
            var expectedCols = Rows[0].Items.Count;
            for (var i = 1; i < Rows.Count; i++)
                if (Rows[i].Items.Count != expectedCols)
                    yield return new ValidationResult(
                        $"Invariant violated: ItemTable row {i} has {Rows[i].Items.Count} columns, expected {expectedCols}.",
                        [nameof(Rows)]);
        }
    }
}
