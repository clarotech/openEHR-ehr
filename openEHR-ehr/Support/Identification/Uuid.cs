using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Model of the DCE Universal Unique Identifier (UUID) scheme of identifiers.
/// Value is a string of the form: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
/// Corresponds to UUID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class Uuid : Uid
{
    private static readonly System.Text.RegularExpressions.Regex UuidPattern =
        new(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (!string.IsNullOrWhiteSpace(Value) && !UuidPattern.IsMatch(Value))
            yield return new ValidationResult(
                "Invariant violated: Uuid.Value must match the pattern xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.",
                [nameof(Value)]);
    }
}
