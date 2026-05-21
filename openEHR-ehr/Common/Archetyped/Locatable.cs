using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// Root structural class of all information model classes that can be archetyped.
/// Has a <see cref="Name"/>, a <see cref="ArchetypeNodeId"/> and optionally a <see cref="Uid"/>.
/// Corresponds to LOCATABLE in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public abstract class Locatable : Pathable
{
    /// <summary>
    /// Runtime name of this fragment, used to build runtime paths.
    /// </summary>
    public required DvText Name { get; init; }

    /// <summary>
    /// Design-time archetype node identifier of this node in the archetype tree.
    /// </summary>
    public required string ArchetypeNodeId { get; init; }

    /// <summary>Optional unique identifier of this object.</summary>
    public UidBasedId? Uid { get; init; }

    /// <summary>Optional links to other items in the same or other EHRs.</summary>
    public IReadOnlySet<Link>? Links { get; init; }

    /// <summary>
    /// Details of the archetype used to create this instance.
    /// Present only on root nodes (archetype roots).
    /// </summary>
    public Archetyped? ArchetypeDetails { get; init; }

    /// <summary>Audit trail from non-openEHR systems.</summary>
    public FeederAudit? FeederAudit { get; init; }

    /// <summary>
    /// The concept name of the archetype as a whole, derived from the archetype node id
    /// of the root node.
    /// </summary>
    public string Concept => ArchetypeNodeId;

    /// <summary>
    /// Returns true if this node is the root of an archetyped structure,
    /// i.e. <see cref="ArchetypeDetails"/> is not null.
    /// </summary>
    public bool IsArchetypeRoot => ArchetypeDetails is not null;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Name is null)
            yield return new ValidationResult(
                "Invariant violated: Locatable.Name must not be null.",
                [nameof(Name)]);

        if (string.IsNullOrWhiteSpace(ArchetypeNodeId))
            yield return new ValidationResult(
                "Invariant violated: Locatable.ArchetypeNodeId must not be empty.",
                [nameof(ArchetypeNodeId)]);

        // Invariant: archetype_details present implies archetype_node_id is set
        if (ArchetypeDetails is not null && string.IsNullOrWhiteSpace(ArchetypeNodeId))
            yield return new ValidationResult(
                "Invariant violated: ArchetypeNodeId must be set when ArchetypeDetails is present.",
                [nameof(ArchetypeNodeId)]);
    }
}
