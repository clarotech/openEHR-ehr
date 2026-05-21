namespace OpenEHR.RM.Tests.DataStructures;

/// <summary>
/// RM invariants for ELEMENT:
///   value_xor_null_flavour — Value and NullFlavour are mutually exclusive.
///   value_or_null_flavour   — At least one of Value or NullFlavour must be set.
///   null_reason_requires_null_flavour — NullReason requires NullFlavour.
/// </summary>
public class ElementTests
{
    // ── Invariant: value_or_null_flavour ─────────────────────────────────────

    [Fact]
    public void Valid_element_with_value_passes()
    {
        var el = Builders.Element();
        el.Validate(new ValidationContext(el)).Should().BeEmpty();
    }

    [Fact]
    public void Valid_element_with_null_flavour_passes()
    {
        var el = new Element
        {
            Name = Builders.DvText("weight"),
            ArchetypeNodeId = "at0001",
            NullFlavour = Builders.DvCodedText("no information", "openehr", "271")
        };
        el.Validate(new ValidationContext(el)).Should().BeEmpty();
    }

    [Fact]
    public void Invariant_value_or_null_flavour_fails_when_both_null()
    {
        var el = new Element
        {
            Name = Builders.DvText("weight"),
            ArchetypeNodeId = "at0001"
        };
        var results = el.Validate(new ValidationContext(el)).ToList();
        results.Should().Contain(r =>
            r.MemberNames.Contains(nameof(Element.Value)) ||
            r.MemberNames.Contains(nameof(Element.NullFlavour)));
    }

    // ── Invariant: value_xor_null_flavour ────────────────────────────────────

    [Fact]
    public void Invariant_value_xor_null_flavour_fails_when_both_set()
    {
        var el = new Element
        {
            Name = Builders.DvText("weight"),
            ArchetypeNodeId = "at0001",
            Value = new DvText("72"),
            NullFlavour = Builders.DvCodedText("no information", "openehr", "271")
        };
        var results = el.Validate(new ValidationContext(el)).ToList();
        results.Should().Contain(r =>
            r.MemberNames.Contains(nameof(Element.Value)) &&
            r.MemberNames.Contains(nameof(Element.NullFlavour)));
    }

    // ── Invariant: null_reason_requires_null_flavour ─────────────────────────

    [Fact]
    public void Invariant_null_reason_requires_null_flavour_fails_when_only_reason_set()
    {
        var el = new Element
        {
            Name = Builders.DvText("weight"),
            ArchetypeNodeId = "at0001",
            Value = new DvText("72"),
            NullReason = Builders.DvText("patient refused")
        };
        var results = el.Validate(new ValidationContext(el)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(Element.NullReason)));
    }

    [Fact]
    public void Null_reason_with_null_flavour_passes()
    {
        var el = new Element
        {
            Name = Builders.DvText("weight"),
            ArchetypeNodeId = "at0001",
            NullFlavour = Builders.DvCodedText("no information", "openehr", "271"),
            NullReason = Builders.DvText("patient refused")
        };
        el.Validate(new ValidationContext(el)).Should().BeEmpty();
    }
}
