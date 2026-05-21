namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Model of a reverse Internet domain, used as a namespace identifier.
/// Value is a dot-separated reverse domain name, e.g. "org.openehr".
/// Corresponds to INTERNET_ID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class InternetId : Uid;
