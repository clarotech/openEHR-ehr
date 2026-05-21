using System.ComponentModel.DataAnnotations;
using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataStructures.Representation;

/// <summary>
/// The leaf variant of ITEM, which carries a value of type DATA_VALUE.
/// Either <see cref="Value"/> or <see cref="NullFlavour"/> must be set, but not both.
/// Corresponds to ELEMENT in openEHR RM 1.1.0 data_structures.representation.
/// </summary>
public sealed class Element : Item
{
    /// <summary>
    /// The data value carried by this element. Null if this element is null-flavoured.
    /// </summary>
    public DataValue? Value { get; init; }

    /// <summary>
    /// Flavour of null value, e.g. "no information", "masked", "not applicable".
    /// Must be set if <see cref="Value"/> is null.
    /// Coded from the openEHR <c>null flavours</c> terminology.
    /// </summary>
    public DvCodedText? NullFlavour { get; init; }

    /// <summary>
    /// Optional reason for null value, e.g. "patient refused".
    /// </summary>
    public DvText? NullReason { get; init; }

    /// <summary>Returns true if this element has a null flavour.</summary>
    public bool IsNull => NullFlavour is not null;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
            yield return result;

        // Invariant: value and null_flavour are mutually exclusive
        if (Value is not null && NullFlavour is not null)
            yield return new ValidationResult(
                "Invariant violated: Element.Value and Element.NullFlavour are mutually exclusive.",
                [nameof(Value), nameof(NullFlavour)]);

        // Invariant: at least one of value or null_flavour must be set
        if (Value is null && NullFlavour is null)
            yield return new ValidationResult(
                "Invariant violated: Element must have either a Value or a NullFlavour.",
                [nameof(Value), nameof(NullFlavour)]);

        // Invariant: null_reason only allowed when null_flavour is set
        if (NullReason is not null && NullFlavour is null)
            yield return new ValidationResult(
                "Invariant violated: Element.NullReason requires NullFlavour to be set.",
                [nameof(NullReason), nameof(NullFlavour)]);
    }
}
