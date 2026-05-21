using System.ComponentModel.DataAnnotations;

namespace OpenEHR.RM.Support.Identification;

/// <summary>
/// Model of ISO's Object Identifier (OID) scheme of identifiers.
/// Value is a string of dotted integers, e.g. "1.2.840.113549".
/// Corresponds to ISO_OID in openEHR RM 1.1.0 support.identification.
/// </summary>
public sealed class IsoOid : Uid
{
    private static readonly System.Text.RegularExpressions.Regex OidPattern =
        new(@"^\d+(\.\d+)*$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        if (!string.IsNullOrWhiteSpace(Value) && !OidPattern.IsMatch(Value))
            yield return new ValidationResult(
                "Invariant violated: IsoOid.Value must be a dotted integer sequence.",
                [nameof(Value)]);
    }
}
