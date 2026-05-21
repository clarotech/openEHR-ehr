using OpenEHR.RM.DataStructures.ItemStructure;

namespace OpenEHR.RM.Tests.DataStructures;

/// <summary>
/// RM invariants for HISTORY:
///   origin_valid  — Origin must not be null.
///   events_valid  — Events, if present, must contain at least one entry.
/// </summary>
public class HistoryTests
{
    // ── Invariant: origin_valid ───────────────────────────────────────────────

    [Fact]
    public void Valid_history_with_events_passes()
    {
        var h = new History<ItemTree>
        {
            Name = Builders.DvText("history"),
            ArchetypeNodeId = "at0001",
            Origin = Builders.DvDateTime(),
            Events =
            [
                new PointEvent<ItemTree>
                {
                    Name = Builders.DvText("event"),
                    ArchetypeNodeId = "at0002",
                    Time = Builders.DvDateTime(),
                    Data = Builders.ItemTree()
                }
            ]
        };
        h.Validate(new ValidationContext(h)).Should().BeEmpty();
    }

    [Fact]
    public void Valid_history_without_events_passes()
    {
        var h = new History<ItemTree>
        {
            Name = Builders.DvText("history"),
            ArchetypeNodeId = "at0001",
            Origin = Builders.DvDateTime()
        };
        h.Validate(new ValidationContext(h)).Should().BeEmpty();
    }

    // ── Invariant: events_valid ───────────────────────────────────────────────

    [Fact]
    public void Invariant_events_valid_fails_when_events_empty_list()
    {
        var h = new History<ItemTree>
        {
            Name = Builders.DvText("history"),
            ArchetypeNodeId = "at0001",
            Origin = Builders.DvDateTime(),
            Events = []
        };
        var results = h.Validate(new ValidationContext(h)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(History<ItemTree>.Events)));
    }

    // ── IsPeriodic ────────────────────────────────────────────────────────────

    [Fact]
    public void IsPeriodic_true_when_period_set()
    {
        var h = new History<ItemTree>
        {
            Name = Builders.DvText("history"),
            ArchetypeNodeId = "at0001",
            Origin = Builders.DvDateTime(),
            Period = new DvDuration("PT1S")
        };
        h.IsPeriodic.Should().BeTrue();
    }

    [Fact]
    public void IsPeriodic_false_when_period_absent()
    {
        var h = new History<ItemTree>
        {
            Name = Builders.DvText("history"),
            ArchetypeNodeId = "at0001",
            Origin = Builders.DvDateTime()
        };
        h.IsPeriodic.Should().BeFalse();
    }
}
