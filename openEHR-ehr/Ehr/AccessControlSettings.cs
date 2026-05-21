using OpenEHR.RM.Common.Archetyped;

namespace OpenEHR.RM.Ehr;

/// <summary>
/// Abstract class defining the concept of access control settings for an EHR.
/// Concrete subtypes are defined by EHR system vendors.
/// Corresponds to ACCESS_CONTROL_SETTINGS in openEHR RM 1.1.0 ehr.
/// </summary>
public abstract class AccessControlSettings : Locatable;
