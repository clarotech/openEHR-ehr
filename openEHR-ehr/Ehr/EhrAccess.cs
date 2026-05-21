namespace OpenEHR.RM.Ehr;

/// <summary>
/// EHR-wide access control record.
/// The <see cref="Settings"/> object is vendor-defined; this class provides the
/// structural wrapper and the <see cref="Scheme"/> derived function.
/// Corresponds to EHR_ACCESS in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class EhrAccess : Common.Archetyped.Locatable
{
    /// <summary>
    /// Optional vendor-specific access control settings for this EHR.
    /// </summary>
    public AccessControlSettings? Settings { get; init; }

    /// <summary>
    /// Returns the access control scheme name from the settings object,
    /// or an empty string if no settings are present.
    /// </summary>
    public string Scheme => Settings?.ArchetypeNodeId ?? string.Empty;
}
