using MauiExtension.SimpleSearchPicker;

namespace SearchPicker;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();
        searchPicker.ItemsSource = _items;
        //searchPicker.SelectedItem = _items[0];

    }

    internal readonly StringRepresentable[] _items = [new("Value 1"), new("Value 2"), new("Value 3"), new("Value 4"), new("Value 5"),
        new("Value 6"), new("Value 7"), new("Value 8"), new("Value 11"), new("Value 12")];

    private void SearchPicker_Focused(object sender, FocusEventArgs e)
    {
#if ANDROID
        if (e.IsFocused)
        {
            scrollView.ScrollToAsync(0, searchPicker.Y, false);
        }
#endif
    }

    public record StringRepresentable(string VisibleData) : IStringPresentable;


}

