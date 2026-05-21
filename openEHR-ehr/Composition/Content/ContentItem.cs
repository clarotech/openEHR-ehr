using OpenEHR.RM.Common.Archetyped;

namespace OpenEHR.RM.Composition.Content;

/// <summary>
/// Abstract ancestor of all clinical content classes.
/// Corresponds to CONTENT_ITEM in openEHR RM 1.1.0 composition.content.
/// </summary>
public abstract class ContentItem : Locatable;
