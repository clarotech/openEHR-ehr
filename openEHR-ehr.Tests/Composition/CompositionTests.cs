using Comp = OpenEHR.RM.Composition.Composition;

namespace OpenEHR.RM.Tests.Composition;

/// <summary>
/// RM invariants for COMPOSITION:
///   language_valid    — Language must not be null.
///   territory_valid   — Territory must not be null.
///   category_valid    — Category must not be null.
///   composer_valid    — Composer must not be null.
///   is_persistent_validity — Event compositions must have a context.
///   category_classification — IsPersistent / IsEvent / IsEpisodic are mutually exclusive.
/// </summary>
public class CompositionTests
{
    [Fact]
    public void Valid_event_composition_passes()
    {
        var comp = Builders.Composition();
        comp.Validate(new ValidationContext(comp)).Should().BeEmpty();
    }

    // ── Invariant: is_persistent_validity ────────────────────────────────────

    [Fact]
    public void Invariant_event_composition_requires_context()
    {
        var comp = new Comp
        {
            Name = Builders.DvText("Encounter"),
            ArchetypeNodeId = "openEHR-EHR-COMPOSITION.encounter.v1",
            Language = Builders.CodePhrase(),
            Territory = Builders.CodePhrase("ISO_3166-1", "GB"),
            Category = Builders.DvCodedText("event", "openehr", "433"),
            Composer = Builders.PartyIdentified(),
            Context = null     // missing — should fail for event category
        };
        var results = comp.Validate(new ValidationContext(comp)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(Comp.Context)));
    }

    [Fact]
    public void Persistent_composition_without_context_passes()
    {
        var comp = new Comp
        {
            Name = Builders.DvText("Problem list"),
            ArchetypeNodeId = "openEHR-EHR-COMPOSITION.problem_list.v1",
            Language = Builders.CodePhrase(),
            Territory = Builders.CodePhrase("ISO_3166-1", "GB"),
            Category = Builders.DvCodedText("persistent", "openehr", "431"),
            Composer = Builders.PartyIdentified(),
            Context = null
        };
        comp.Validate(new ValidationContext(comp)).Should().BeEmpty();
    }

    // ── Category classification helpers ──────────────────────────────────────

    [Fact]
    public void IsPersistent_true_for_code_431()
    {
        var comp = new Comp
        {
            Name = Builders.DvText("Problem list"),
            ArchetypeNodeId = "openEHR-EHR-COMPOSITION.problem_list.v1",
            Language = Builders.CodePhrase(),
            Territory = Builders.CodePhrase("ISO_3166-1", "GB"),
            Category = Builders.DvCodedText("persistent", "openehr", "431"),
            Composer = Builders.PartyIdentified()
        };
        comp.IsPersistent.Should().BeTrue();
        comp.IsEvent.Should().BeFalse();
    }

    [Fact]
    public void IsEvent_true_for_code_433()
    {
        Builders.Composition().IsEvent.Should().BeTrue();
        Builders.Composition().IsPersistent.Should().BeFalse();
    }

    // ── Invariant: composer_valid ─────────────────────────────────────────────

    [Fact]
    public void Invariant_composer_valid_fails_when_null()
    {
        var comp = new Comp
        {
            Name = Builders.DvText(),
            ArchetypeNodeId = "openEHR-EHR-COMPOSITION.encounter.v1",
            Language = Builders.CodePhrase(),
            Territory = Builders.CodePhrase("ISO_3166-1", "GB"),
            Category = Builders.DvCodedText("event", "openehr", "433"),
            Composer = null!,
            Context = Builders.EventContext()
        };
        var results = comp.Validate(new ValidationContext(comp)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(Comp.Composer)));
    }
}
