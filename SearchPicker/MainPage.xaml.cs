

using Plugin.Maui.SimpleSearchPicker;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SearchPicker;

public record StringRepresentable(string VisibleData) : IStringPresentable;

public class Context : INotifyPropertyChanged
{
    private IStringPresentable _selectedData;

    public Context()
    {
        Data = [new("Value 1"), new("Value 2"), new("Value 3"), new("Value 4"), new("Value 5"),
        new("Value 6"), new("Value 7"), new("Value 8"), new("Value 11"), new("Value 12")];
        _selectedData = Data[0];
    }


    public IStringPresentable SelectedData
    { 
        get => _selectedData;
        set
        {
            _selectedData = value;
            OnPropertyChanged(nameof(SelectedData));
        }
    }
    public List<StringRepresentable> Data { get; }



    private void OnPropertyChanged([CallerMemberName]string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();

        BindingContext = new Context();
        Plugin.Maui.SimpleSearchPicker.SearchPicker.SubscribeScrollToWhenFocusedAndroid(searchPicker, scrollView);

    }

    private void Label_Loaded(object sender, EventArgs e)
    {
        if (sender is View view)
        {
            // Add this to your custom data template view loaded event
            // This enables user interaction with you data template
            Plugin.Maui.SimpleSearchPicker.SearchPicker.SubscribeDataTemplateUserAccess(searchPicker, view);
        }
    }

    private void SelectedItemChanged(object sender, IStringPresentable e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var context = (Context)BindingContext;

        StringRepresentable data = new($"New Value {Random.Shared.Next(20)}");

        context.SelectedData = data;
        context.Data.Add(data);
    }
}

