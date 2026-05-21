namespace OpenEHR.RM.Tests.Support.Identification;

public class ObjectVersionIdTests
{
    private const string ValidValue = "550e8400-e29b-41d4-a716-446655440000::example.org::1";

    // ── Construction ────────────────────────────────────────────────────────

    [Fact]
    public void Valid_value_parses_segments_correctly()
    {
        var id = new ObjectVersionId { Value = ValidValue };
        id.ObjectId.Should().Be("550e8400-e29b-41d4-a716-446655440000");
        id.CreatingSystemId.Should().Be("example.org");
        id.VersionTreeId.TrunkVersion.Should().Be("1");
        id.IsBranch.Should().BeFalse();
    }

    [Fact]
    public void Branch_version_detected()
    {
        var id = new ObjectVersionId { Value = "abc::sys::1.1.1" };
        id.IsBranch.Should().BeTrue();
        id.VersionTreeId.BranchNumber.Should().Be("1");
        id.VersionTreeId.BranchVersion.Should().Be("1");
    }

    // ── Invariant: value_valid (RM: value must have 3 ::-separated parts) ──

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Invariant_value_valid_fails_when_empty(string value)
    {
        var id = new ObjectVersionId { Value = value };
        var results = id.Validate(new ValidationContext(id)).ToList();
        results.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("onlyone")]
    [InlineData("two::parts")]
    [InlineData("four::parts::too::many")]
    public void Invariant_value_valid_fails_when_wrong_segment_count(string value)
    {
        var id = new ObjectVersionId { Value = value };
        var results = id.Validate(new ValidationContext(id)).ToList();
        // One or more errors expected — exact count varies (e.g. 4-segment value also fails VersionTreeId)
        results.Should().Contain(r => r.MemberNames.Contains(nameof(ObjectVersionId.Value)));
    }

    [Fact]
    public void Valid_value_produces_no_validation_errors()
    {
        var id = new ObjectVersionId { Value = ValidValue };
        var results = id.Validate(new ValidationContext(id)).ToList();
        results.Should().BeEmpty();
    }
}
