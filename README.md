# MauiExtension.SimpleSearchPicker

by [Henri Vainio](https://github.com/henrivain)


###  Simple search picker for .NET MAUI (.NET 8 and later). 

![image](https://github.com/user-attachments/assets/3458d4e1-1543-44c8-ac2a-feee54518d68)

###  Just like any other picker but with search capabilities!

![image](https://github.com/user-attachments/assets/3782d430-8d1a-4170-bb05-3e364cce0f6b)

### Simple design, takes only small space in your UI

![image](https://github.com/user-attachments/assets/4403c383-0b8a-4596-b229-9d68ed2f871f)

### Easy to use

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns:ui="clr-namespace:MauiExtension.SimpleSearchPicker;assembly=MauiExtension.SimpleSearchPicker"
    ... >
      <VerticalStackLayout Padding="30,0,30,300"
                           Spacing="25">
          <ui:SearchPicker x:Name="searchPicker"
                           IsFocusedChanged="SearchPicker_Focused" 
                           MaximumWidthRequest="200"
                           ItemsSource="{Binding Data}"
                           SelectedItem="{Binding SelectedData}"
                           SelectedItemChanged="SelectedItemChanged"
                           />
    </VerticalStackLayout>

</ContentPage>
```

### Use your own data templates  

![image](https://github.com/user-attachments/assets/968b46bc-0355-4cb4-99b0-9abe28ae296c)

```xaml
<ui:SearchPicker x:Name="searchPicker"
                 IsFocusedChanged="SearchPicker_Focused"
                 MaximumWidthRequest="200"
                 ItemsSource="{Binding Data}"
                 SelectedItem="{Binding SelectedData}"
                 SelectedItemChanged="SelectedItemChanged">
    <ui:SearchPicker.ItemTemplate>
        <DataTemplate x:DataType="{x:Type ui:IStringPresentable}">
            <VerticalStackLayout Loaded="Label_Loaded" Padding="4">
                <Label Text="This data includes:" />
                <Label Text="{Binding VisibleData}" />
            </VerticalStackLayout>
        </DataTemplate>
    </ui:SearchPicker.ItemTemplate>
</ui:SearchPicker>
```

### Multiplatform

Notice that currently only Windows and Android checked to work   

![image](https://github.com/user-attachments/assets/087fb6b2-381e-4122-818c-e90bf1db70c0)

