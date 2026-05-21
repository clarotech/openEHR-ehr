using OpenEHR.RM.Common.ChangeControl;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// Versioned container for EHR_STATUS instances.
/// Corresponds to VERSIONED_EHR_STATUS in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class VersionedEhrStatus : VersionedObject<EhrStatus>;
