using MauiExtension.SimpleSearchPicker;

namespace SearchPicker;

public record StringRepresentable(string VisibleData) : IStringPresentable;

public class Context
{
    public Context()
    {
        Data = [new("Value 1"), new("Value 2"), new("Value 3"), new("Value 4"), new("Value 5"),
        new("Value 6"), new("Value 7"), new("Value 8"), new("Value 11"), new("Value 12")];
        SelectedData = Data[0];
    }


    public IStringPresentable SelectedData { get; set; } 
    public StringRepresentable[] Data { get; } 
}

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();

        BindingContext = new Context();
        //searchPicker.ItemsSource = _items;
        //searchPicker.SelectedItem = _items[0];
        MauiExtension.SimpleSearchPicker.SearchPicker.SubscribeScrollToWhenFocusedAndroid(searchPicker, scrollView);

    }

    private void Label_Loaded(object sender, EventArgs e)
    {
        if (sender is View view)
        {
            MauiExtension.SimpleSearchPicker.SearchPicker.SubscribeDataTemplateUserAccess(searchPicker, view);
        }
    }

    private void SelectedItemChanged(object sender, IStringPresentable e)
    {

    }
}

