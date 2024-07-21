using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiColor = Microsoft.Maui.Graphics.Color;

namespace MauiExtension.SimpleSearchPicker;

public partial class SearchPicker : VerticalStackLayout
{
    string _searchWord = string.Empty;

    public SearchPicker()
    {
        InitializeComponent();
        SetFocus(this, new FocusEventArgs(this, IsFocused));
        BindableLayout.SetItemsSource(bindingStack, VisibleItems);
    }



    public IEnumerable<IStringPresentable>? ItemsSource
    {
        get => (IEnumerable<IStringPresentable>?)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
            Filter();
        }
    }

    public IStringPresentable? SelectedItem
    {
        get => (IStringPresentable?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public int MaxVisibleItemCount { get; set; } = -1;

    ObservableCollection<IStringPresentable> VisibleItems { get; } = [];

    public new bool IsFocused
    {
        get => (bool)GetValue(IsFocusedProperty);
        set => SetValue(IsFocusedProperty, value);
    }

    public new MauiColor BackgroundColor
    {
        get => (MauiColor)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public MauiColor HoverBackgroundColor
    {
        get => (MauiColor)GetValue(HoverBackgroundColorProperty);
        set => SetValue(HoverBackgroundColorProperty, value);
    }

    public MauiColor TextColor
    {
        get => (MauiColor)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public MauiColor SecondaryTextColor
    {
        get => (MauiColor)GetValue(SecondaryTextColorProperty);
        set => SetValue(SecondaryTextColorProperty, value);
    }

   

    public string SearchWord
    {
        get => _searchWord;
        set
        {
            _searchWord = value;
            OnPropertyChanged();
            Filter();
        }
    }




    private void Filter()
    {
        /* Filter is not ran if IsFocused is false, 
         * Filter is always called after IsFocused is true
         */

        if (ItemsSource is null)
        {
            return;
        }
        if (IsFocused is false)
        {
            return;
        }

        VisibleItems.Clear();
        if (string.IsNullOrWhiteSpace(SearchWord))
        {
            foreach (var item in ItemsSource)
            {
                VisibleItems.Add(item);
                if (IsVisibleLimitReached())
                {
                    return;
                }
            }
            return;
        }

        foreach (var item in ItemsSource)
        {
            bool isSearchMatch = item.VisibleData.Contains(SearchWord, StringComparison.CurrentCultureIgnoreCase);
            if (isSearchMatch)
            {
                VisibleItems.Add(item);
            }
            if (IsVisibleLimitReached())
            {
                return;
            }
        }


        bool IsVisibleLimitReached()
        {
            return MaxVisibleItemCount > 0 && MaxVisibleItemCount >= VisibleItems.Count;
        }
    }




    private void SetFocus(object? sender, FocusEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);
        IsFocused = e.IsFocused;

        if (e.IsFocused)
        {
            Filter();
            new Animation(value => { menu.MaximumHeightRequest = value; }, 0, 200)
                .Commit(menu, "menuOpening", length: 200, easing: Easing.CubicIn);
        }
        else
        {
            searchField.Unfocus();
            SearchWord = string.Empty;
            new Animation(value => { menu.MaximumHeightRequest = value; }, 200, 0)
                .Commit(menu, "menuClosing", length: 200, easing: Easing.CubicIn);
        }
    }

    private void DataItem_PointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Label label)
        {
            label.BackgroundColor = HoverBackgroundColor;
        }
    }

    private void DataItem_PointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Label label)
        {
            label.BackgroundColor = Colors.Transparent;
        }
    }

    private void DataItem_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is null or IStringPresentable)
        {
            SelectedItem = (IStringPresentable?)e.Parameter;
            Unfocus();
            SelectedItemChanged?.Invoke(this, SelectedItem);
            return;
        }
        throw new UnreachableException("Mismatched data type, this exception should never be thrown.");
    }



    public event EventHandler<IStringPresentable?>? SelectedItemChanged;



    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable<IStringPresentable>), typeof(SearchPicker), null, BindingMode.OneWay);

    public static readonly new BindableProperty IsFocusedProperty = BindableProperty.Create(
        nameof(IsFocused), typeof(bool), typeof(SearchPicker), false, BindingMode.OneWay);

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem), typeof(IStringPresentable), typeof(SearchPicker), null, BindingMode.TwoWay);

    public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor), typeof(MauiColor), typeof(SearchPicker), ResourceProvider.GetColorOrNull("Gray600"), BindingMode.OneWay);

    public static readonly BindableProperty HoverBackgroundColorProperty = BindableProperty.Create(
        nameof(HoverBackgroundColor), typeof(MauiColor), typeof(SearchPicker), ResourceProvider.GetColorOrNull("Gray200"), BindingMode.OneWay);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(MauiColor), typeof(SearchPicker), Colors.White, BindingMode.OneWay);

    public static readonly BindableProperty SecondaryTextColorProperty = BindableProperty.Create(
        nameof(SecondaryTextColor), typeof(MauiColor), typeof(SearchPicker), ResourceProvider.GetColorOrNull("Gray500"), BindingMode.OneWay);

}