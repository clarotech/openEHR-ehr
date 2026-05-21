namespace OpenEHR.RM.Tests.Common.Generic;

/// <summary>
/// RM invariant: basic_validity — at least one of Name, Identifiers, or ExternalRef must be set.
/// </summary>
public class PartyIdentifiedTests
{
    // ── Invariant: basic_validity ────────────────────────────────────────────

    [Fact]
    public void Invariant_basic_validity_passes_with_name_only()
    {
        var party = new PartyIdentified { Name = "Dr Smith" };
        party.Validate(new ValidationContext(party)).Should().BeEmpty();
    }

    [Fact]
    public void Invariant_basic_validity_passes_with_identifiers_only()
    {
        var party = new PartyIdentified
        {
            Identifiers = [new DvIdentifier("12345", "NHS", "NHS", "NHSNumber")]
        };
        party.Validate(new ValidationContext(party)).Should().BeEmpty();
    }

    [Fact]
    public void Invariant_basic_validity_passes_with_external_ref_only()
    {
        var party = new PartyIdentified
        {
            ExternalRef = new PartyRef
            {
                Namespace = "demographic",
                Type = "PERSON",
                Id = new HierObjectId { Value = "abc" }
            }
        };
        party.Validate(new ValidationContext(party)).Should().BeEmpty();
    }

    [Fact]
    public void Invariant_basic_validity_fails_when_all_null()
    {
        var party = new PartyIdentified();
        var results = party.Validate(new ValidationContext(party)).ToList();
        results.Should().NotBeEmpty();
    }
}
