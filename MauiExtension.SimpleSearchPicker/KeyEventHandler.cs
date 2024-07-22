#if WINDOWS
using MauiColor = Microsoft.Maui.Graphics.Color;

namespace MauiExtension.SimpleSearchPicker;

class KeyEventHandler(VerticalStackLayout layout, MauiColor backgroundColor, MauiColor hoverBackgroundColor)
{
    readonly MauiColor _backgroundColor = backgroundColor;
    readonly MauiColor _hoverBackgroundColor = hoverBackgroundColor;
    readonly VerticalStackLayout _layout = layout;

    int _keyboardIndex = -1;

    internal void HandleSelected(Action<object, TappedEventArgs> selectorCallback)
    {
        if (_keyboardIndex < 0)
        {
            return;
        }
        if (_keyboardIndex > _layout.Count)
        {
            return;
        }

        var selectedElement = (BindableObject)_layout.Children[_keyboardIndex];
        selectorCallback(selectedElement, new TappedEventArgs(selectedElement.BindingContext));

        _keyboardIndex = -1;
    }

    internal void HighLightValueBelow(out double newYCoord)
    {
        newYCoord = -1;
        int count = _layout.Children.Count;
        if (count is 0)
        {
            return;
        }

        int oldIndex = _keyboardIndex;
        _keyboardIndex = _keyboardIndex == count - 1 ? 0 : oldIndex + 1;

        // Unfocus the old element
        if (oldIndex is not -1 && _layout.Children[oldIndex] is VisualElement oldElement)
        {
            oldElement.BackgroundColor = _backgroundColor;
        }

        // Focus into new element
        if (_layout.Children[_keyboardIndex] is VisualElement element)
        {
            element.BackgroundColor = _hoverBackgroundColor;
            newYCoord = element.Y;
        }
    }

    internal void HighLightValueOnTop(out double newYCoord)
    {
        newYCoord = -1;
        int count = _layout.Children.Count;
        if (count is 0)
        {
            return;
        }

        int oldIndex = _keyboardIndex;
        _keyboardIndex = _keyboardIndex is -1 or 0 ? count - 1 : oldIndex - 1;

        // Unfocus the old element
        if (oldIndex is not -1 && _layout.Children[oldIndex] is VisualElement oldElement)
        {
            oldElement.BackgroundColor = _backgroundColor;
        }

        // Focus into new element
        if (_layout.Children[_keyboardIndex] is VisualElement element)
        {
            element.BackgroundColor = _hoverBackgroundColor;
            newYCoord = element.Y;
        }
    }

    internal void UnhighLightAll()
    {
        if (_keyboardIndex is not -1 && _layout.Children[_keyboardIndex] is VisualElement oldElement)
        {
            oldElement.BackgroundColor = _backgroundColor;
        }
        _keyboardIndex = -1;
    }
}
#endif
