namespace Plugin.Maui.SimpleSearchPicker;
public record PlaceholderString(string Value)
{
    public static implicit operator PlaceholderString(string value) => new(value);
}
