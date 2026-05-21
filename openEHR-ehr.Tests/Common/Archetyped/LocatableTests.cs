namespace OpenEHR.RM.Tests.Common.Archetyped;

/// <summary>
/// Tests for Locatable invariants (exercised through EhrStatus as a concrete subclass).
/// RM invariant: name_valid — Name must not be null.
/// RM invariant: archetype_node_id_valid — ArchetypeNodeId must not be empty.
/// RM invariant: archetype_details_is_archetype_root — ArchetypeDetails present implies ArchetypeNodeId set.
/// </summary>
public class LocatableTests
{
    // ── Invariant: name_valid ────────────────────────────────────────────────

    [Fact]
    public void Invariant_name_valid_fails_when_name_null()
    {
        var status = new EhrStatus
        {
            Name = null!,
            ArchetypeNodeId = "at0001",
            Subject = Builders.PartySelf(),
            IsQueryable = true,
            IsModifiable = true
        };

        var results = status.Validate(new ValidationContext(status)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(EhrStatus.Name)));
    }

    // ── Invariant: archetype_node_id_valid ───────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Invariant_archetype_node_id_valid_fails_when_empty(string nodeId)
    {
        var status = new EhrStatus
        {
            Name = Builders.DvText(),
            ArchetypeNodeId = nodeId,
            Subject = Builders.PartySelf(),
            IsQueryable = true,
            IsModifiable = true
        };

        var results = status.Validate(new ValidationContext(status)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(EhrStatus.ArchetypeNodeId)));
    }

    // ── IsArchetypeRoot ──────────────────────────────────────────────────────

    [Fact]
    public void IsArchetypeRoot_true_when_ArchetypeDetails_present()
    {
        var status = new EhrStatus
        {
            Name = Builders.DvText("EHR Status"),
            ArchetypeNodeId = "openEHR-EHR-EHR_STATUS.generic.v1",
            Subject = Builders.PartySelf(),
            IsQueryable = true,
            IsModifiable = true,
            ArchetypeDetails = Builders.Archetyped()
        };
        status.IsArchetypeRoot.Should().BeTrue();
    }

    [Fact]
    public void IsArchetypeRoot_false_when_ArchetypeDetails_absent()
    {
        Builders.EhrStatus().IsArchetypeRoot.Should().BeFalse();
    }

    // ── Valid instance ───────────────────────────────────────────────────────

    [Fact]
    public void Valid_locatable_produces_no_validation_errors()
    {
        var status = Builders.EhrStatus();
        status.Validate(new ValidationContext(status)).Should().BeEmpty();
    }
}
