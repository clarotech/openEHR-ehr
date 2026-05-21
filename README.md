# Clarotech.OpenEHR.RM

A C# implementation of the [openEHR Reference Model](https://specifications.openehr.org/releases/RM/Release-1.1.0/) Release 1.1.0, covering the EHR, Composition, Common, Support, and Data Structures packages.

This library complements [OpenEHR.RM.DataTypes](https://www.nuget.org/packages/OpenEHR.RM.DataTypes), which implements the openEHR Data Types specification.

## Packages

| NuGet | Description |
|-------|-------------|
| `Clarotech.OpenEHR.RM` | This library — EHR, Composition, Common, Support, DataStructures |
| `OpenEHR.RM.DataTypes` | Data Types (DvText, DvQuantity, DvDateTime, etc.) |

## Installation

```bash
dotnet add package Clarotech.OpenEHR.RM
```

## Namespaces

The namespace structure mirrors the openEHR RM package hierarchy:

| Namespace | RM Package | Key Types |
|-----------|-----------|-----------|
| `OpenEHR.RM.Support.Identification` | `support.identification` | `HierObjectId`, `ObjectVersionId`, `ObjectRef`, `LocatableRef` |
| `OpenEHR.RM.Common.Archetyped` | `common.archetyped` | `Locatable`, `Pathable`, `Archetyped`, `Link` |
| `OpenEHR.RM.Common.Generic` | `common.generic` | `PartyProxy`, `PartyIdentified`, `PartySelf`, `Participation`, `AuditDetails` |
| `OpenEHR.RM.Common.ChangeControl` | `common.change_control` | `VersionedObject<T>`, `OriginalVersion<T>`, `Contribution` |
| `OpenEHR.RM.DataStructures` | `data_structures` | `DataStructure`, `History<T>`, `Event<T>` |
| `OpenEHR.RM.DataStructures.ItemStructure` | `data_structures.item_structure` | `ItemStructure`, `ItemTree`, `ItemList`, `ItemTable`, `ItemSingle` |
| `OpenEHR.RM.DataStructures.Representation` | `data_structures.representation` | `Item`, `Cluster`, `Element` |
| `OpenEHR.RM.DataStructures.History` | `data_structures.history` | `History<T>`, `PointEvent<T>`, `IntervalEvent<T>` |
| `OpenEHR.RM.Ehr` | `ehr` | `Ehr`, `EhrStatus`, `EhrAccess`, `VersionedComposition` |
| `OpenEHR.RM.Composition` | `composition` | `Composition`, `EventContext` |
| `OpenEHR.RM.Composition.Content.Navigation` | `composition.content.navigation` | `Section` |
| `OpenEHR.RM.Composition.Content.Entry` | `composition.content.entry` | `Observation`, `Evaluation`, `Instruction`, `Action`, `AdminEntry` |

## Usage

### Building a Composition

```csharp
using OpenEHR.RM.Composition;
using OpenEHR.RM.Composition.Content.Entry;
using OpenEHR.RM.Common.Generic;
using OpenEHR.RM.DataStructures.History;
using OpenEHR.RM.DataStructures.ItemStructure;
using OpenEHR.RM.DataTypes.Text;
using IS = OpenEHR.RM.DataStructures.ItemStructure.ItemStructure;

var composer = new PartyIdentified { Name = "Dr Smith" };

var composition = new Composition
{
    Name = new DvText("Blood Pressure"),
    ArchetypeNodeId = "openEHR-EHR-COMPOSITION.encounter.v1",
    Language = new CodePhrase(new TerminologyId("ISO_639-1"), "en"),
    Territory = new CodePhrase(new TerminologyId("ISO_3166-1"), "GB"),
    Category = new DvCodedText("event", new CodePhrase(new TerminologyId("openehr"), "433")),
    Composer = composer,
    Context = new EventContext
    {
        StartTime = new DvDateTime("2024-01-15T10:30:00"),
        Setting = new DvCodedText("primary medical care",
                      new CodePhrase(new TerminologyId("openehr"), "228"))
    },
    Content =
    [
        new Observation
        {
            Name = new DvText("Blood pressure"),
            ArchetypeNodeId = "openEHR-EHR-OBSERVATION.blood_pressure.v1",
            Language = new CodePhrase(new TerminologyId("ISO_639-1"), "en"),
            Encoding = new CodePhrase(new TerminologyId("IANA_character-sets"), "UTF-8"),
            Subject = new PartySelf(),
            Data = new History<IS>
            {
                Name = new DvText("history"),
                ArchetypeNodeId = "at0001",
                Origin = new DvDateTime("2024-01-15T10:30:00"),
                Events =
                [
                    new PointEvent<IS>
                    {
                        Name = new DvText("Any event"),
                        ArchetypeNodeId = "at0006",
                        Time = new DvDateTime("2024-01-15T10:30:00"),
                        Data = new ItemTree
                        {
                            Name = new DvText("Tree"),
                            ArchetypeNodeId = "at0003",
                            Items =
                            [
                                new Element
                                {
                                    Name = new DvText("Systolic"),
                                    ArchetypeNodeId = "at0004",
                                    Value = new DvText("120 mmHg")
                                }
                            ]
                        }
                    }
                ]
            }
        }
    ]
};
```

### Validating Against RM Invariants

All types implement `IValidatableObject`. Call `Validate()` to check RM invariants:

```csharp
using System.ComponentModel.DataAnnotations;

var context = new ValidationContext(composition);
var results = composition.Validate(context).ToList();

if (results.Count > 0)
{
    foreach (var error in results)
        Console.WriteLine(error.ErrorMessage);
}
```

### Building an EHR

```csharp
using OpenEHR.RM.Ehr;
using OpenEHR.RM.Support.Identification;

var ehr = new Ehr
{
    SystemId = new HierObjectId { Value = "example.org" },
    EhrId   = new HierObjectId { Value = "550e8400-e29b-41d4-a716-446655440000" },
    TimeCreated = new DvDateTime("2024-01-15T09:00:00"),
    EhrStatus = new ObjectRef
    {
        Namespace = "local",
        Type = "EHR_STATUS",
        Id = new HierObjectId { Value = "status-1" }
    },
    EhrAccess = new ObjectRef
    {
        Namespace = "local",
        Type = "EHR_ACCESS",
        Id = new HierObjectId { Value = "access-1" }
    }
};
```

## Design Notes

- **Mutability**: All mandatory (`1..1`) attributes use C# `required` init-only properties. Optional (`0..1`) attributes are nullable init-only properties.
- **Validation**: RM invariants are checked on demand via `IValidatableObject.Validate()`, not enforced on construction. This keeps deserialisation and incremental construction clean.
- **Sealed classes**: All concrete leaf classes are `sealed`. Extensibility follows the RM pattern: use `other_details : ITEM_STRUCTURE` for additional data.
- **Naming**: openEHR `SCREAMING_SNAKE_CASE` names are mapped to C# `PascalCase` (e.g. `EHR_STATUS` → `EhrStatus`, `VERSIONED_COMPOSITION` → `VersionedComposition`).
- **Namespaces**: Mirror the RM package structure under `OpenEHR.RM.*`.
- **Serialisation**: Not included in this library. A companion package `Clarotech.OpenEHR.RM.Json` (planned) will provide flat-format JSON support.

## Versioning

Package versions follow the pattern `<rm_major>.<library_minor>.<library_patch>`:
- Major version (`1.x.x`) is locked to RM Release 1.x.
- Minor and patch increments are library-only changes.

## Specification

[openEHR Reference Model Release 1.1.0](https://specifications.openehr.org/releases/RM/Release-1.1.0/)

## Related

- [openEHR-datatypes](https://github.com/clarotech/openEHR-datatypes) — Data Types implementation
- [openEHR Foundation](https://www.openehr.org/)

## License

Apache-2.0
