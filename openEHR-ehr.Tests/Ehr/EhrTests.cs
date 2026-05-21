namespace OpenEHR.RM.Tests.Ehr;

/// <summary>
/// RM invariants for EHR:
///   system_id_valid     — SystemId must not be null.
///   ehr_id_valid        — EhrId must not be null.
///   time_created_valid  — TimeCreated must not be null.
///   ehr_status_valid    — EhrStatus must not be null.
///   ehr_access_valid    — EhrAccess must not be null.
/// </summary>
public class EhrTests
{
    [Fact]
    public void Valid_ehr_produces_no_validation_errors()
    {
        Builders.Ehr().Validate(new ValidationContext(Builders.Ehr())).Should().BeEmpty();
    }

    [Fact]
    public void Invariant_system_id_valid_fails_when_null()
    {
        var ehr = new global::OpenEHR.RM.Ehr.Ehr
        {
            SystemId = null!,
            EhrId = Builders.HierObjectId(),
            TimeCreated = Builders.DvDateTime(),
            EhrStatus = Builders.ObjectRef("local", "EHR_STATUS", "s1"),
            EhrAccess = Builders.ObjectRef("local", "EHR_ACCESS", "a1")
        };
        var results = ehr.Validate(new ValidationContext(ehr)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(global::OpenEHR.RM.Ehr.Ehr.SystemId)));
    }

    [Fact]
    public void Invariant_ehr_status_valid_fails_when_null()
    {
        var ehr = new global::OpenEHR.RM.Ehr.Ehr
        {
            SystemId = Builders.HierObjectId(),
            EhrId = Builders.HierObjectId(),
            TimeCreated = Builders.DvDateTime(),
            EhrStatus = null!,
            EhrAccess = Builders.ObjectRef("local", "EHR_ACCESS", "a1")
        };
        var results = ehr.Validate(new ValidationContext(ehr)).ToList();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(global::OpenEHR.RM.Ehr.Ehr.EhrStatus)));
    }
}
