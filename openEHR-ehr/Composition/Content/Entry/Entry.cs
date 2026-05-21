using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.Composition.Content;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.Support.Identification;

namespace OpenEHR.RM.Composition.Content.Entry;

/// <summary>
/// Abstract ancestor of all clinical entries. An entry is the atomic unit of
/// information in a composition.
/// Corresponds to ENTRY in openEHR RM 1.1.0 composition.content.entry.
/// </summary>
public abstract class Entry : ContentItem
{
    /// <summary>
    /// Language of the content of this entry. Must be a valid ISO 639-1 code.
    /// </summary>
    public required CodePhrase Language { get; init; }

    /// <summary>
    /// Character encoding of this entry's text, e.g. "UTF-8".
    /// </summary>
    public required CodePhrase Encoding { get; init; }

    /// <summary>
    /// The subject of this entry — the patient, or another party such as a foetus.
    /// </summary>
    public required PartyProxy Subject { get; init; }

    /// <summary>
    /// Optional provider who performed or is responsible for this entry.
    /// </summary>
    public PartyProxy? Provider { get; init; }

    /// <summary>
    /// Optional other participants in the activity documented by this entry.
    /// </summary>
    public IReadOnlyList<Participation>? OtherParticipations { get; init; }

    /// <summary>
    /// Optional protocol (method) followed when creating this entry.
    /// </summary>
    public ItemStructure? Protocol { get; init; }

    /// <summary>
    /// Optional reference to a guideline or protocol that was followed.
    /// </summary>
    public ObjectRef? GuidelineId { get; init; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Language is null)
            yield return new ValidationResult(
                "Invariant violated: Entry.Language must not be null.",
                [nameof(Language)]);

        if (Encoding is null)
            yield return new ValidationResult(
                "Invariant violated: Entry.Encoding must not be null.",
                [nameof(Encoding)]);

        if (Subject is null)
            yield return new ValidationResult(
                "Invariant violated: Entry.Subject must not be null.",
                [nameof(Subject)]);
    }
}
