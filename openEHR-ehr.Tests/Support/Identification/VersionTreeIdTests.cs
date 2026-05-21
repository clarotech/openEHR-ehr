namespace OpenEHR.RM.Tests.Support.Identification;

public class VersionTreeIdTests
{
    // ── Invariant: value_valid (trunk only) ─────────────────────────────────

    [Fact]
    public void Trunk_only_is_valid()
    {
        var id = new VersionTreeId { Value = "1" };
        id.Validate(new ValidationContext(id)).Should().BeEmpty();
        id.TrunkVersion.Should().Be("1");
        id.IsBranch.Should().BeFalse();
    }

    // ── Invariant: value_valid (branch form) ────────────────────────────────

    [Fact]
    public void Branch_form_is_valid()
    {
        var id = new VersionTreeId { Value = "1.2.3" };
        id.Validate(new ValidationContext(id)).Should().BeEmpty();
        id.TrunkVersion.Should().Be("1");
        id.BranchNumber.Should().Be("2");
        id.BranchVersion.Should().Be("3");
        id.IsBranch.Should().BeTrue();
    }

    // ── Invariant: value_valid fails ────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_value_fails(string value)
    {
        var id = new VersionTreeId { Value = value };
        id.Validate(new ValidationContext(id)).Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("1.2")]          // two parts — invalid
    [InlineData("1.2.3.4")]      // four parts — invalid
    public void Wrong_segment_count_fails(string value)
    {
        var id = new VersionTreeId { Value = value };
        id.Validate(new ValidationContext(id)).Should().NotBeEmpty();
    }

    [Fact]
    public void Non_integer_component_fails()
    {
        var id = new VersionTreeId { Value = "1.x.3" };
        id.Validate(new ValidationContext(id)).Should().NotBeEmpty();
    }
}
