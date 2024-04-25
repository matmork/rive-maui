using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rive.Maui;

namespace Example.Pages;

public partial class TouchInputPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _currentStateName;

    [ObservableProperty]
    private double _numberInputValue;

    [RelayCommand]
    private Task StateChanged(StateMachineChange state)
    {
        CurrentStateName = state.StateName;
        if (state.Inputs.TryGetValue("Number 1", out var value)
            && value is float floatValue)
        {
            NumberInputValue = floatValue;
        }
        return Task.CompletedTask;
    }
}