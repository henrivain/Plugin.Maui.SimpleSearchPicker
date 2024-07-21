using MauiColor = Microsoft.Maui.Graphics.Color;

namespace MauiExtension.SimpleSearchPicker;

internal class ResourceProvider
{
    public static T FindResourceOrThrow<T>(string key)
    {
        Type? typeFound = null;
        foreach (var dict in Application.Current!.Resources.MergedDictionaries)
        {
            if (dict.TryGetValue(key, out object dictValue))
            {
                if (dictValue is T resource)
                {
                    return resource;
                }
                typeFound = dictValue.GetType();
            }
        }
        if (typeFound is not null)
        {
            throw new InvalidCastException($"Cannot cast type {typeof(T)} to {typeFound}");
        }
        throw new KeyNotFoundException($"Key '{key}' not found on app resources");
    }

    public static MauiColor? GetColorOrNull(string key)
    {
        try
        {
            return FindResourceOrThrow<MauiColor>(key);
        }
        catch (Exception ex)
        {
            if (ex is InvalidCastException or KeyNotFoundException)
            {
                return null;
            }
            throw;
        }
    }
}
