using System.Windows.Input;

namespace Rive.Maui;

// Modified version of: https://github.com/jsuarezruiz/rive-maui/blob/main/src/RiveSharp.Views.MAUI/StateMachineInput.cs
// Thank you, @jsuarezruiz
public abstract class StateMachineInput : BindableObject
{
    public string? InputName { get; set; }

    public string? StateMachineName { get; set; }

    protected WeakReference<Rive?> Rive { get; private set; } = new(null);

    // Sets _rive to the given rive object and applies our input value to the state
    // machine. Does nothing if _rive was already equal to rive.
    internal void SetRive(WeakReference<Rive?> rive)
    {
        Rive = rive;
        Apply();
    }

    protected void Apply()
    {
        if (!string.IsNullOrWhiteSpace(InputName)
            && !string.IsNullOrWhiteSpace(StateMachineName)
            && Rive.TryGetTarget(out var rive))
        {
            Apply(rive, StateMachineName, InputName);
        }
    }

    // Applies our input value to the rive's state machine.
    // rive and inputName are guaranteed to not be null or empty.
    protected abstract void Apply(Rive rive, string stateMachineName, string inputName);
}

[ContentProperty(nameof(Value))]
public class BoolInput : StateMachineInput
{
    // Define "Value" as a DependencyProperty so it can be data-bound.
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(bool),
        typeof(BoolInput),
        false,
        propertyChanged: OnValueChanged);

    public bool Value
    {
        get => (bool)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((BoolInput)bindable).Apply();
    }

    protected override void Apply(Rive rive, string stateMachineName, string inputName)
    {
        rive.SetInput(stateMachineName, inputName, Value);
    }
}

[ContentProperty(nameof(Value))]
public class NumberInput : StateMachineInput
{
    // Define "Value" as a DependencyProperty so it can be data-bound.
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(double),
        typeof(NumberInput),
        0.0,
        propertyChanged: OnValueChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((NumberInput)bindable).Apply();
    }

    protected override void Apply(Rive rive, string stateMachineName, string inputName)
    {
        rive.SetInput(stateMachineName, inputName, (float)Value);
    }
}

public class TriggerInput : StateMachineInput
{
    public void Fire()
    {
        if (!string.IsNullOrWhiteSpace(InputName)
            && !string.IsNullOrWhiteSpace(StateMachineName)
            && Rive.TryGetTarget(out var rive))
        {
            rive.TriggerInput(StateMachineName, InputName);
        }
    }

    // Make a Fire() overload that matches the RoutedEventHandler delegate.
    // This allows us to do things like <Button Click="MyTriggerInput.Fire" ... />
    public void Fire(object s, EventArgs e) => Fire();

    public ICommand FireCommand => new Command(Fire);

    // Triggers don't have any persistent data to apply.
    protected override void Apply(Rive rive, string stateMachineName, string inputName)
    {
    }
}