using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Example.Pages;

public partial class UpdatePropertiesPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _resourceName = "emoji";

    [ObservableProperty]
    private string? _artboardName;

    [RelayCommand]
    private Task UpdateProperty(string property)
    {
        switch (property)
        {
            case "resource":
                ResourceName = ResourceName == "emoji" ? "runner" : "emoji";
                break;
            case "artboard":
                if (ResourceName == "emoji")
                    ArtboardName = ArtboardName == "Emoji_package" ? "Onfire" : "Emoji_package";
                break;
        }

        return Task.CompletedTask;
    }
}