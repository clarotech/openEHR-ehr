using OpenEHR.RM.Common.ChangeControl;
using OpenEHR.RM.Composition;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// Versioned container for COMPOSITION instances.
/// Corresponds to VERSIONED_COMPOSITION in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class VersionedComposition : VersionedObject<Composition.Composition>
{
    /// <summary>
    /// Returns true if the latest version of this composition is persistent
    /// (i.e. its category is "persistent").
    /// </summary>
    public bool IsPersistent =>
        LatestVersion?.Data?.IsPersistent ?? false;
}
