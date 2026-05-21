using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Identifier for archetypes. Instances of this class are immutable.
/// Value format: &lt;rm_originator&gt;-&lt;rm_name&gt;-&lt;rm_entity&gt;.&lt;domain_concept&gt;.v&lt;version_id&gt;
/// e.g. "openEHR-EHR-OBSERVATION.blood_pressure.v1"
/// Corresponds to ARCHETYPE_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class ArchetypeId : ObjectId
{
    private string[] QualifiedParts => Value?.Split('.') ?? [];
    private string[] EntityParts => QualifiedParts.Length > 0
        ? QualifiedParts[0].Split('-')
        : [];

    /// <summary>
    /// Organisation who created the reference model on which the archetype is based,
    /// e.g. "openEHR".
    /// </summary>
    public string RmOriginator => EntityParts.Length >= 1 ? EntityParts[0] : string.Empty;

    /// <summary>Name of the reference model, e.g. "EHR".</summary>
    public string RmName => EntityParts.Length >= 2 ? EntityParts[1] : string.Empty;

    /// <summary>Name of the RM class in snake_case, e.g. "OBSERVATION".</summary>
    public string RmEntity => EntityParts.Length >= 3 ? EntityParts[2] : string.Empty;

    /// <summary>Name of the domain concept represented by this archetype, e.g. "blood_pressure".</summary>
    public string DomainConcept => QualifiedParts.Length >= 2 ? QualifiedParts[1] : string.Empty;

    /// <summary>Version of the archetype, e.g. "v1".</summary>
    public string VersionId => QualifiedParts.Length >= 3 ? QualifiedParts[2] : string.Empty;

    /// <summary>Qualified name of the RM entity in the form "&lt;rm_originator&gt;-&lt;rm_name&gt;-&lt;rm_entity&gt;".</summary>
    public string QualifiedRmEntity => QualifiedParts.Length >= 1 ? QualifiedParts[0] : string.Empty;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (!string.IsNullOrWhiteSpace(Value))
        {
            if (EntityParts.Length < 3)
                yield return new ValidationResult(
                    "Invariant violated: ArchetypeId qualified entity section must have form '<rm_originator>-<rm_name>-<rm_entity>'.",
                    [nameof(Value)]);

            if (QualifiedParts.Length < 3)
                yield return new ValidationResult(
                    "Invariant violated: ArchetypeId must have form '<qualified_entity>.<domain_concept>.v<version>'.",
                    [nameof(Value)]);
        }
    }
}
