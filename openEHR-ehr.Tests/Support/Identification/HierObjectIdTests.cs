namespace OpenEHR.RM.Tests.Support.Identification;

public class HierObjectIdTests
{
    // ── Construction ────────────────────────────────────────────────────────

    [Fact]
    public void Valid_simple_root_constructs()
    {
        var id = new HierObjectId { Value = "550e8400-e29b-41d4-a716-446655440000" };
        id.Root.Should().Be("550e8400-e29b-41d4-a716-446655440000");
        id.Extension.Should().BeNull();
        id.HasExtension.Should().BeFalse();
    }

    [Fact]
    public void Valid_root_with_extension_constructs()
    {
        var id = new HierObjectId { Value = "1.2.840.113549:localExt" };
        id.Root.Should().Be("1.2.840.113549");
        id.Extension.Should().Be("localExt");
        id.HasExtension.Should().BeTrue();
    }

    [Fact]
    public void ToString_returns_value()
    {
        var id = new HierObjectId { Value = "abc" };
        id.ToString().Should().Be("abc");
    }

    // ── Invariant: Value_valid (RM: value_valid) ────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Invariant_Value_valid_fails_when_value_empty(string value)
    {
        var id = new HierObjectId { Value = value };
        var results = id.Validate(new ValidationContext(id)).ToList();
        results.Should().ContainSingle(r => r.MemberNames.Contains(nameof(HierObjectId.Value)));
    }
}
