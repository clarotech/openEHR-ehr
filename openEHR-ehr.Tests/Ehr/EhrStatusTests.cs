namespace OpenEHR.RM.Tests.Ehr;

/// <summary>
/// RM invariants for EHR_STATUS:
///   subject_valid       — Subject must not be null.
///   is_queryable_valid  — IsQueryable is a required boolean (always set with required keyword).
///   is_modifiable_valid — IsModifiable is a required boolean (always set with required keyword).
/// </summary>
public class EhrStatusTests
{
    [Fact]
    public void Valid_ehr_status_produces_no_validation_errors()
    {
        var status = Builders.EhrStatus();
        status.Validate(new ValidationContext(status)).Should().BeEmpty();
    }

    // ── Invariant: subject_valid ─────────────────────────────────────────────

    [Fact]
    public void Invariant_subject_valid_fails_when_subject_null()
    {
        var status = new EhrStatus
        {
            Name = Builders.DvText("EHR Status"),
            ArchetypeNodeId = "at0001",
            Subject = null!,
            IsQueryable = true,
            IsModifiable = true
        };
        var results = status.Validate(new ValidationContext(status)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(EhrStatus.Subject)));
    }

    // ── IsQueryable / IsModifiable (required booleans) ───────────────────────

    [Theory]
    [InlineData(true,  true)]
    [InlineData(true,  false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void All_combinations_of_queryable_and_modifiable_are_valid(bool queryable, bool modifiable)
    {
        var status = new EhrStatus
        {
            Name = Builders.DvText(),
            ArchetypeNodeId = "at0001",
            Subject = Builders.PartySelf(),
            IsQueryable = queryable,
            IsModifiable = modifiable
        };
        status.Validate(new ValidationContext(status)).Should().BeEmpty();
    }
}
