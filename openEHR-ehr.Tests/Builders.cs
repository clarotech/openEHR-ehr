using OpenEHR.RM.DataStructures.ItemStructure;
using Comp = OpenEHR.RM.Composition.Composition;

/// <summary>
/// Centralised factory helpers that produce minimal valid instances of RM types.
/// Tests use these to avoid repeating boilerplate. Each factory clearly documents
/// the minimum required fields per the spec.
/// </summary>
internal static class Builders
{
    // ── Support.Identification ──────────────────────────────────────────────

    public static HierObjectId HierObjectId(string value = "550e8400-e29b-41d4-a716-446655440000") =>
        new() { Value = value };

    public static ObjectVersionId ObjectVersionId(
        string objectId = "550e8400-e29b-41d4-a716-446655440000",
        string systemId = "example.org",
        string versionTreeId = "1") =>
        new() { Value = $"{objectId}::{systemId}::{versionTreeId}" };

    public static ObjectRef ObjectRef(
        string ns = "local",
        string type = "COMPOSITION",
        string idValue = "abc123") =>
        new() { Namespace = ns, Type = type, Id = new HierObjectId { Value = idValue } };

    // ── DataTypes helpers ───────────────────────────────────────────────────
    // Note: datatypes library uses constructor-based construction, not init-only.

    public static DvText DvText(string value = "test") =>
        new(value);

    public static DvCodedText DvCodedText(
        string value = "test",
        string terminology = "openehr",
        string code = "433") =>
        new(value, CodePhrase(terminology, code));

    public static CodePhrase CodePhrase(
        string terminology = "ISO_639-1",
        string code = "en") =>
        new(new TerminologyId(terminology), code);

    public static DvText DvTextValue(string value) => new(value);

    public static DvDateTime DvDateTime(string value = "2024-01-15T10:00:00") =>
        new(value);

    public static DvIdentifier DvIdentifier(
        string id,
        string? issuer = null,
        string? assignee = null,
        string? type = null) =>
        new(id, issuer, assignee, type);

    // ── Common.Archetyped helpers ───────────────────────────────────────────

    public static Archetyped Archetyped(
        string archetypeId = "openEHR-EHR-OBSERVATION.blood_pressure.v1",
        string rmVersion = "1.1.0") =>
        new()
        {
            ArchetypeId = new ArchetypeId { Value = archetypeId },
            RmVersion = rmVersion
        };

    // ── Common.Generic helpers ──────────────────────────────────────────────

    public static PartySelf PartySelf() => new();

    public static PartyIdentified PartyIdentified(string name = "Dr Smith") =>
        new() { Name = name };

    public static AuditDetails AuditDetails(string systemId = "example.org") =>
        new()
        {
            SystemId = systemId,
            Committer = PartyIdentified(),
            TimeCommitted = DvDateTime(),
            ChangeType = DvCodedText("creation", "openehr", "249")
        };

    // ── DataStructures helpers ──────────────────────────────────────────────

    public static Element Element(string name = "value", string dataValue = "normal") =>
        new()
        {
            Name = DvText(name),
            ArchetypeNodeId = "at0001",
            Value = new DvText(dataValue)
        };

    public static Cluster Cluster(string name = "cluster") =>
        new()
        {
            Name = DvText(name),
            ArchetypeNodeId = "at0001",
            Items = [Element()]
        };

    public static ItemTree ItemTree(string name = "tree") =>
        new()
        {
            Name = DvText(name),
            ArchetypeNodeId = "at0001",
            Items = [Element()]
        };

    // ── Ehr helpers ─────────────────────────────────────────────────────────

    public static EhrStatus EhrStatus() =>
        new()
        {
            Name = DvText("EHR Status"),
            ArchetypeNodeId = "openEHR-EHR-EHR_STATUS.generic.v1",
            Subject = PartySelf(),
            IsQueryable = true,
            IsModifiable = true
        };

    public static OpenEHR.RM.Ehr.Ehr Ehr(string ehrIdValue = "550e8400-e29b-41d4-a716-446655440000") =>
        new()
        {
            SystemId = HierObjectId("example.org"),
            EhrId = HierObjectId(ehrIdValue),
            TimeCreated = DvDateTime(),
            EhrStatus = ObjectRef("local", "EHR_STATUS", "status-1"),
            EhrAccess = ObjectRef("local", "EHR_ACCESS", "access-1")
        };

    // ── Composition helpers ─────────────────────────────────────────────────

    public static Comp Composition(string categoryCode = "433", string categoryValue = "event") =>
        new()
        {
            Name = DvText("Blood pressure"),
            ArchetypeNodeId = "openEHR-EHR-COMPOSITION.encounter.v1",
            Language = CodePhrase("ISO_639-1", "en"),
            Territory = CodePhrase("ISO_3166-1", "GB"),
            Category = DvCodedText(categoryValue, "openehr", categoryCode),
            Composer = PartyIdentified("Dr Smith"),
            Context = EventContext()
        };

    public static EventContext EventContext() =>
        new()
        {
            StartTime = DvDateTime(),
            Setting = DvCodedText("primary medical care", "openehr", "228")
        };

    // ── Entry helpers ───────────────────────────────────────────────────────

    public static Observation Observation() =>
        new()
        {
            Name = DvText("Blood pressure"),
            ArchetypeNodeId = "openEHR-EHR-OBSERVATION.blood_pressure.v1",
            Language = CodePhrase(),
            Encoding = CodePhrase("IANA_character-sets", "UTF-8"),
            Subject = PartySelf(),
            Data = new History<IS>
            {
                Name = DvText("History"),
                ArchetypeNodeId = "at0001",
                Origin = DvDateTime(),
                Events =
                [
                    new PointEvent<IS>
                    {
                        Name = DvText("Any event"),
                        ArchetypeNodeId = "at0006",
                        Time = DvDateTime(),
                        Data = ItemTree("data")   // ItemTree : IS, valid as IS
                    }
                ]
            }
        };
}
