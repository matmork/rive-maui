using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rive.Maui;

namespace Example.Pages;

public class TouchInputPageViewModel : INotifyPropertyChanged
{
    private string? _currentStateName;
    private double _numberInputValue;

    public string? CurrentStateName
    {
        get => _currentStateName;
        set => SetField(ref _currentStateName, value);
    }

    public double NumberInputValue
    {
        get => _numberInputValue;
        set => SetField(ref _numberInputValue, value);
    }

    public Command<StateMachineChange> StateChangedCommand => new(state =>
    {
        CurrentStateName = state.StateName;
        if (state.Inputs.TryGetValue("Number 1", out var value)
            && value is float floatValue)
        {
            NumberInputValue = floatValue;
        }
    });

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}