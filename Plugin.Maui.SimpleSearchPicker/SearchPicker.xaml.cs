#if WINDOWS
using Windows.System;
#endif
using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiColor = Microsoft.Maui.Graphics.Color;

namespace Plugin.Maui.SimpleSearchPicker;

public partial class SearchPicker : VerticalStackLayout
{
    string _searchWord = string.Empty;
    CooldownTimer _timer = new(200);

    public SearchPicker()
    {
        PropertyChanged += async (sender, e) =>
        {
            if (e.PropertyName is nameof(SearchWord))
            {
                if (await _timer.WaitIsCooldownOverAsync())
                {
                    Filter();
                }
            }
        };

        InitializeComponent();
        SetFocus(new FocusEventArgs(this, IsFocused), false);
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
        set
        {
            SetValue(SelectedItemProperty, value);
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }
    }

    public int MaxVisibleItemCount { get; set; } = 30;  // -1 means not limited 

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

    public MauiColor DataItemTextColor
    {
        get => (MauiColor)GetValue(DataItemTextColorProperty);
        set => SetValue(DataItemTextColorProperty, value);
    }

    public MauiColor SecondaryTextColor
    {
        get => (MauiColor)GetValue(SecondaryTextColorProperty);
        set => SetValue(SecondaryTextColorProperty, value);
    }

    public double DropdownMaxHeight
    {
        get => (double)GetValue(DropdownMaxHeightProperty);
        set => SetValue(DropdownMaxHeightProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public string SearchWord
    {
        get => _searchWord;
        set
        {
            _searchWord = value;
            // Filter is run in PropertyChanged in ctor
            OnPropertyChanged();
        }
    }

    public void Filter()
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
            return MaxVisibleItemCount > 0 && VisibleItems.Count >= MaxVisibleItemCount;
        }
    }

    public void SetCooldownTimerDelay(int delay_ms)
    {
        _timer = new(delay_ms);
        Filter();   // Refilters to be sure with user input
    }

    public static void SubscribeScrollToWhenFocusedAndroid(SearchPicker picker, ScrollView parent)
    {
#if ANDROID
        picker.IsFocusedChanged += (sender, e) =>
        {
            if (e.IsFocused)
            {
                parent.ScrollToAsync(0, picker.Y, false);
            }
        };
#endif
    }

    public static void SubscribeDataTemplateUserAccess(SearchPicker picker, View pickerTemplateChild)
    {
        PointerGestureRecognizer pointerRecognizer = new();
        pointerRecognizer.PointerEntered += picker.DataItem_PointerEntered;
        pointerRecognizer.PointerExited += picker.DataItem_PointerExited;

        TapGestureRecognizer tapGestureRecognizer = new()
        {
            CommandParameter = pickerTemplateChild.BindingContext
        };
        tapGestureRecognizer.Tapped += picker.DataItem_Tapped;

        pickerTemplateChild.GestureRecognizers.Add(pointerRecognizer);
        pickerTemplateChild.GestureRecognizers.Add(tapGestureRecognizer);
    }

    private void SetFocus(object sender, FocusEventArgs e) => SetFocus(e);

    private void SetFocus(FocusEventArgs e, bool animate = true)
    {
        ArgumentNullException.ThrowIfNull(e);
        IsFocused = e.IsFocused;

        if (IsFocused)
        {
            Filter();
            OpenMenu(animate);
        }
        else
        {
            CloseMenu(animate);
            searchField.Unfocus();
            SearchWord = string.Empty;
        }
        IsFocusedChanged?.Invoke(this, e);


        void OpenMenu(bool animate)
        {
            if (animate)
            {
                new Animation(value => { menu.MaximumHeightRequest = value; }, 0, DropdownMaxHeight)
                    .Commit(menu, "menuOpening", length: 200, easing: Easing.CubicIn);
                return;
            }
            menu.MaximumHeightRequest = DropdownMaxHeight;
        }

        void CloseMenu(bool animate)
        {
            if (animate)
            {
                new Animation(value => { menu.MaximumHeightRequest = value; }, DropdownMaxHeight, 0)
                    .Commit(menu, "menuClosing", length: 200, easing: Easing.CubicIn);
                return;
            }
            menu.MaximumHeightRequest = 0;
        }
    }

    private void DataItem_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is View view)
        {
            view.BackgroundColor = HoverBackgroundColor;
        }
    }

    private void DataItem_PointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is View view)
        {
            view.BackgroundColor = Colors.Transparent;
        }
    }

    private void DataItem_Tapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is null or IStringPresentable)
        {
            // If Unfocus is called on Android, SetFocus is not called
            SelectedItem = (IStringPresentable?)e.Parameter;
            SetFocus(this, new(this, false));
            return;
        }
        throw new UnreachableException("Mismatched data type, this exception should never be thrown.");
    }


    public event EventHandler<IStringPresentable?>? SelectedItemChanged;

    public event EventHandler<FocusEventArgs>? IsFocusedChanged;


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
        nameof(TextColor), typeof(MauiColor), typeof(SearchPicker),
        DeviceInfo.Current.Platform == DevicePlatform.Android ? Colors.Black : Colors.White, BindingMode.OneWay);

    public static readonly BindableProperty SecondaryTextColorProperty = BindableProperty.Create(
        nameof(SecondaryTextColor), typeof(MauiColor), typeof(SearchPicker), ResourceProvider.GetColorOrNull("Gray500"), BindingMode.OneWay);

    public static readonly BindableProperty DataItemTextColorProperty = BindableProperty.Create(
        nameof(DataItemTextColor), typeof(MauiColor), typeof(SearchPicker), Colors.White, BindingMode.OneWay);

    public static readonly BindableProperty DropdownMaxHeightProperty = BindableProperty.Create(
        nameof(DropdownMaxHeight), typeof(double), typeof(SearchPicker), 200d, BindingMode.OneWay);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate), typeof(DataTemplate), typeof(SearchPicker), null, BindingMode.OneWay);

    private void SearchField_Loaded(object sender, EventArgs e)
    {
#if WINDOWS
        if (sender is Entry entry)
        {
            if (entry.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement native)
            {
                KeyEventHandler handler = new(bindingStack, Colors.Transparent, HoverBackgroundColor);

                native.KeyDown += async (sender, e) =>
                {
                    e.Handled = true;
                    double scrollYCoord;
                    switch (e.Key)
                    {
                        case VirtualKey.Down:
                            handler.HighLightValueBelow(out scrollYCoord);
                            await dataScrollView.ScrollToAsync(0, scrollYCoord, true);
                            break;

                        case VirtualKey.Up:
                            handler.HighLightValueOnTop(out scrollYCoord);
                            break;

                        case VirtualKey.Escape:
                            handler.UnhighLightAll();
                            SetFocus(this, new(this, false));
                            e.Handled = false;
                            return;

                        case VirtualKey.Enter:
                            handler.HandleSelected(DataItem_Tapped);
                            return;
                        default:
                            e.Handled = false;
                            return;
                    }
                    if (scrollYCoord >= 0)
                    {
                        await dataScrollView.ScrollToAsync(0, scrollYCoord, true);
                    }
                };
            }
        }
#endif
    }


}