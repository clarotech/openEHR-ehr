namespace OpenEHR.RM.Common.Generic;

/// <summary>
/// Party proxy representing the subject of the EHR.
/// Used to represent the subject of a <c>EHR_STATUS</c>, which is the patient.
/// Corresponds to PARTY_SELF in openEHR RM 1.1.0 common.generic.
/// </summary>
public sealed class PartySelf : PartyProxy;
