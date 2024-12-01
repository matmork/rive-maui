using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Example.Pages;

public partial class UpdatePropertiesPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _artboardName;

    [RelayCommand]
    private void UpdateProperty(string property)
    {
        ArtboardName = property switch
        {
            "artboard" => ArtboardName == "Emoji_package" ? "Onfire" : "Emoji_package",
            _ => ArtboardName
        };
    }
}