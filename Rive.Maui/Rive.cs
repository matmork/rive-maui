using System.Windows.Input;

namespace Rive.Maui;

public class Rive : View
{
    public static readonly BindableProperty ArtboardNameProperty = BindableProperty.Create(
        nameof(ArtboardName),
        typeof(string),
        typeof(Rive),
        defaultValue: null
    );

    public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create(
        nameof(AnimationName),
        typeof(string),
        typeof(Rive),
        defaultValue: null,
        propertyChanged: OnAnimationNameChanged
    );

    public static readonly BindableProperty StateMachineNameProperty = BindableProperty.Create(
        nameof(StateMachineName),
        typeof(string),
        typeof(Rive),
        defaultValue: null
    );

    public static readonly BindableProperty ResourceNameProperty = BindableProperty.Create(
        nameof(ResourceName),
        typeof(string),
        typeof(Rive),
        defaultValue: null
    );

    public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(
        nameof(AutoPlay),
        typeof(bool),
        typeof(Rive),
        defaultValue: true
    );

    public static readonly BindableProperty FitProperty = BindableProperty.Create(
        nameof(OnStateMachineChangeCommand),
        typeof(Fit),
        typeof(Rive),
        defaultValue: Fit.Contain
    );

    public static readonly BindableProperty AlignmentProperty = BindableProperty.Create(
        nameof(OnStateMachineChangeCommand),
        typeof(Alignment),
        typeof(Rive),
        defaultValue: Alignment.Center
    );

    public static readonly BindableProperty OnStateMachineChangeCommandProperty = BindableProperty.Create(
        nameof(OnStateMachineChangeCommand),
        typeof(ICommand),
        typeof(Rive)
    );

    public string? ArtboardName
    {
        get => (string?)GetValue(ArtboardNameProperty);
        set => SetValue(ArtboardNameProperty, value);
    }

    public string? AnimationName
    {
        get => (string?)GetValue(AnimationNameProperty);
        set => SetValue(AnimationNameProperty, value);
    }

    public string? StateMachineName
    {
        get => (string?)GetValue(StateMachineNameProperty);
        set => SetValue(StateMachineNameProperty, value);
    }

    public string? ResourceName
    {
        get => (string?)GetValue(ResourceNameProperty);
        set => SetValue(ResourceNameProperty, value);
    }

    public bool AutoPlay
    {
        get => (bool)GetValue(AutoPlayProperty);
        set => SetValue(AutoPlayProperty, value);
    }

    public Fit Fit
    {
        get => (Fit)GetValue(FitProperty);
        set => SetValue(FitProperty, value);
    }

    public Alignment Alignment
    {
        get => (Alignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    public ICommand? OnStateMachineChangeCommand
    {
        get => (ICommand?)GetValue(OnStateMachineChangeCommandProperty);
        set => SetValue(OnStateMachineChangeCommandProperty, value);
    }

    private static void OnAnimationNameChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is Rive { Handler: RiveRenderer renderer }
            && newvalue is string animationName
            && !string.IsNullOrWhiteSpace(animationName))
        {
            renderer.PlayAnimation(animationName, Loop.Loop, Direction.AutoDirection);
        }
    }

    public void PlayAnimation(string animationName, Loop loop, Direction direction)
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.PlayAnimation(animationName, Loop.Loop, Direction.AutoDirection);
        }
    }

    public void Pause()
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.Pause();
        }
    }

    public void Stop()
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.Stop();
        }
    }

    public void Reset()
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.Reset();
        }
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.SetInput(stateMachineName, inputName, value);
        }
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.SetInput(stateMachineName, inputName, value);
        }
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        if (Handler is RiveRenderer renderer)
        {
            renderer.TriggerInput(stateMachineName, inputName);
        }
    }
}