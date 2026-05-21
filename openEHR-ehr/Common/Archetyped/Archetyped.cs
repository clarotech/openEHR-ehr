using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Common.Archetyped;

/// <summary>
/// Archetypes act as the constraint rules for instances of information classes.
/// This class records archetype and template information for a <see cref="Locatable"/> object.
/// Corresponds to ARCHETYPED in openEHR RM 1.1.0 common.archetyped.
/// </summary>
public sealed class Archetyped : IValidatableObject
{
    /// <summary>Identifier of the archetype used to create this object.</summary>
    public required ArchetypeId ArchetypeId { get; init; }

    /// <summary>Identifier of the template used, if one was used.</summary>
    public TemplateId? TemplateId { get; init; }

    /// <summary>
    /// Version of the openEHR reference model used to create this object,
    /// e.g. "1.1.0".
    /// </summary>
    public required string RmVersion { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ArchetypeId is null)
            yield return new ValidationResult(
                "Invariant violated: Archetyped.ArchetypeId must not be null.",
                [nameof(ArchetypeId)]);

        if (string.IsNullOrWhiteSpace(RmVersion))
            yield return new ValidationResult(
                "Invariant violated: Archetyped.RmVersion must not be empty.",
                [nameof(RmVersion)]);
    }
}
