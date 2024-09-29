namespace Plugin.Maui.SimpleSearchPicker;

/// <summary>
/// This type is used to find difference between <see cref="IStringPresentable"/> 
/// and <see cref="SearchPicker.Placeholder"/> in visible selected value. 
/// With multibinding and pattern matching.
/// </summary>
public class PlaceholderString
{
    public PlaceholderString() { }

    public PlaceholderString(string? value)
    {
        Value = value;
    }


    public string? Value { get; set; }

    public static implicit operator PlaceholderString(string? value) => new(value);
}
