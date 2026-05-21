using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.Common.Archetyped;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.Composition.Content;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.Composition;

/// <summary>
/// One version of the archetyped content, authored and committed to an EHR.
/// A composition is the unit of clinical information committed to the EHR.
/// Corresponds to COMPOSITION in openEHR RM 1.1.0 composition.
/// </summary>
public sealed class Composition : Locatable
{
    /// <summary>
    /// Language of the content, using the ISO 639-1 two-letter code.
    /// </summary>
    public required DataTypes.Text.CodePhrase Language { get; init; }

    /// <summary>
    /// Territory where the content was recorded, using the ISO 3166-1 two-letter country code.
    /// </summary>
    public required DataTypes.Text.CodePhrase Territory { get; init; }

    /// <summary>
    /// Categorises the composition: "event", "persistent", or "episodic".
    /// Coded from the openEHR <c>composition category</c> terminology.
    /// </summary>
    public required DvCodedText Category { get; init; }

    /// <summary>The person or organisation that authored and is responsible for this composition.</summary>
    public required PartyProxy Composer { get; init; }

    /// <summary>
    /// The clinical context of this composition. Required for event compositions.
    /// </summary>
    public EventContext? Context { get; init; }

    /// <summary>The clinical content of this composition.</summary>
    public IReadOnlyList<ContentItem>? Content { get; init; }

    /// <summary>Returns true if this is an event (non-persistent) composition.</summary>
    public bool IsEvent =>
        Category.DefiningCode?.CodeString == "433" ||
        Category.Value?.Equals("event", StringComparison.OrdinalIgnoreCase) == true;

    /// <summary>Returns true if this is a persistent composition.</summary>
    public bool IsPersistent =>
        Category.DefiningCode?.CodeString == "431" ||
        Category.Value?.Equals("persistent", StringComparison.OrdinalIgnoreCase) == true;

    /// <summary>Returns true if this is an episodic composition.</summary>
    public bool IsEpisodic =>
        Category.DefiningCode?.CodeString == "432" ||
        Category.Value?.Equals("episodic", StringComparison.OrdinalIgnoreCase) == true;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (Language is null)
            yield return new ValidationResult(
                "Invariant violated: Composition.Language must not be null.",
                [nameof(Language)]);

        if (Territory is null)
            yield return new ValidationResult(
                "Invariant violated: Composition.Territory must not be null.",
                [nameof(Territory)]);

        if (Category is null)
            yield return new ValidationResult(
                "Invariant violated: Composition.Category must not be null.",
                [nameof(Category)]);

        if (Composer is null)
            yield return new ValidationResult(
                "Invariant violated: Composition.Composer must not be null.",
                [nameof(Composer)]);

        // Invariant: event compositions must have a context
        if (IsEvent && Context is null)
            yield return new ValidationResult(
                "Invariant violated: Event compositions must have a Context.",
                [nameof(Context)]);
    }
}
