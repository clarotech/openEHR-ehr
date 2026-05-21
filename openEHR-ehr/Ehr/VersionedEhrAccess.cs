using OpenEHR.RM.Common.ChangeControl;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// Versioned container for EHR_ACCESS instances.
/// Corresponds to VERSIONED_EHR_ACCESS in openEHR RM 1.1.0 ehr.
/// </summary>
public sealed class VersionedEhrAccess : VersionedObject<EhrAccess>;
