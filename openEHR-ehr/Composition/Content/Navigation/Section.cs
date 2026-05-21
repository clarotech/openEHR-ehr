namespace OpenEHR.RM.Composition.Content.Navigation;

/// <summary>
/// Represents a heading in a clinical record, used to organise entries
/// into logical sections (like paper record headings).
/// Corresponds to SECTION in openEHR RM 1.1.0 composition.content.navigation.
/// </summary>
public sealed class Section : ContentItem
{
    /// <summary>
    /// Ordered list of content items within this section.
    /// Can contain nested sections or entries.
    /// </summary>
    public IReadOnlyList<ContentItem>? Items { get; init; }
}
