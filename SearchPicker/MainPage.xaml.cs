using MauiExtension.SimpleSearchPicker;

namespace SearchPicker;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();
        searchPicker.ItemsSource = _items;
        searchPicker.SelectedItem = _items[0];
    }

    internal readonly StringRepresentable[] _items = [new("Value 1"), new("Value 2"), new("Value 3"), new("Value 4"), new("Value 5")];

    public record StringRepresentable(string VisibleData) : IStringPresentable;


}

