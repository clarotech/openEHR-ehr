namespace OpenEHR.RM.Tests.Composition.Content.Entry;

/// <summary>
/// RM invariants for OBSERVATION:
///   data_valid — Data must not be null.
/// Also covers ENTRY base invariants (language, encoding, subject).
/// </summary>
public class ObservationTests
{
    [Fact]
    public void Valid_observation_passes()
    {
        var obs = Builders.Observation();
        obs.Validate(new ValidationContext(obs)).Should().BeEmpty();
    }

    // ── Invariant: data_valid ────────────────────────────────────────────────

    [Fact]
    public void Invariant_data_valid_fails_when_data_null()
    {
        var obs = new Observation
        {
            Name = Builders.DvText("BP"),
            ArchetypeNodeId = "openEHR-EHR-OBSERVATION.blood_pressure.v1",
            Language = Builders.CodePhrase(),
            Encoding = Builders.CodePhrase("IANA_character-sets", "UTF-8"),
            Subject = Builders.PartySelf(),
            Data = null!
        };
        var results = obs.Validate(new ValidationContext(obs)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(Observation.Data)));
    }

    // ── Entry base: subject_valid ─────────────────────────────────────────────

    [Fact]
    public void Invariant_subject_valid_fails_when_subject_null()
    {
        var obs = new Observation
        {
            Name = Builders.DvText("BP"),
            ArchetypeNodeId = "openEHR-EHR-OBSERVATION.blood_pressure.v1",
            Language = Builders.CodePhrase(),
            Encoding = Builders.CodePhrase("IANA_character-sets", "UTF-8"),
            Subject = null!,
            Data = Builders.Observation().Data
        };
        var results = obs.Validate(new ValidationContext(obs)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(Observation.Subject)));
    }
}
