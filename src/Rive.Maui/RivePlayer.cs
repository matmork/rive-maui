using System.Windows.Input;

namespace Rive.Maui;

[ContentProperty(nameof(StateMachineInputs))]
public class RivePlayer : View
{
    public static readonly BindableProperty ArtboardNameProperty = BindableProperty.Create(
        nameof(ArtboardName),
        typeof(string),
        typeof(RivePlayer),
        defaultValue: null
    );

    public static readonly BindableProperty AnimationNameProperty = BindableProperty.Create(
        nameof(AnimationName),
        typeof(string),
        typeof(RivePlayer),
        defaultValue: null,
        propertyChanged: OnAnimationNameChanged
    );

    public static readonly BindableProperty StateMachineNameProperty = BindableProperty.Create(
        nameof(StateMachineName),
        typeof(string),
        typeof(RivePlayer),
        defaultValue: null
    );

    public static readonly BindableProperty ResourceNameProperty = BindableProperty.Create(
        nameof(ResourceName),
        typeof(string),
        typeof(RivePlayer),
        defaultValue: null
    );

    public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(
        nameof(AutoPlay),
        typeof(bool),
        typeof(RivePlayer),
        defaultValue: true
    );

    public static readonly BindableProperty FitProperty = BindableProperty.Create(
        nameof(Fit),
        typeof(RivePlayerFit),
        typeof(RivePlayer),
        defaultValue: RivePlayerFit.Contain
    );

    public static readonly BindableProperty AlignmentProperty = BindableProperty.Create(
        nameof(Alignment),
        typeof(RivePlayerAlignment),
        typeof(RivePlayer),
        defaultValue: RivePlayerAlignment.Center
    );

    public static readonly BindableProperty LoopProperty = BindableProperty.Create(
        nameof(Loop),
        typeof(RivePlayerLoop),
        typeof(RivePlayer),
        defaultValue: RivePlayerLoop.AutoLoop
    );

    public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
        nameof(Direction),
        typeof(RivePlayerDirection),
        typeof(RivePlayer),
        defaultValue: RivePlayerDirection.AutoDirection
    );

    public static readonly BindableProperty OnStateMachineChangeCommandProperty = BindableProperty.Create(
        nameof(OnStateMachineChangeCommand),
        typeof(ICommand),
        typeof(RivePlayer)
    );

    public static readonly BindableProperty StateMachineInputsProperty = BindableProperty.Create(
        nameof(StateMachineInputs),
        typeof(StateMachineInputCollection),
        typeof(RivePlayer)
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

    public RivePlayerFit Fit
    {
        get => (RivePlayerFit)GetValue(FitProperty);
        set => SetValue(FitProperty, value);
    }

    public RivePlayerAlignment Alignment
    {
        get => (RivePlayerAlignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    public RivePlayerLoop Loop
    {
        get => (RivePlayerLoop)GetValue(LoopProperty);
        set => SetValue(LoopProperty, value);
    }

    public RivePlayerDirection Direction
    {
        get => (RivePlayerDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public ICommand? OnStateMachineChangeCommand
    {
        get => (ICommand?)GetValue(OnStateMachineChangeCommandProperty);
        set => SetValue(OnStateMachineChangeCommandProperty, value);
    }

    public StateMachineInputCollection StateMachineInputs
    {
        get => (StateMachineInputCollection)GetValue(StateMachineInputsProperty);
        set => SetValue(StateMachineInputsProperty, value);
    }

    public RivePlayer()
    {
        StateMachineInputs = new StateMachineInputCollection(this);
    }

    private static void OnAnimationNameChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is RivePlayer { Handler: RivePlayerRenderer renderer } rivePlayer
            && newvalue is string animationName
            && !string.IsNullOrWhiteSpace(animationName))
        {
            renderer.PlayAnimation(animationName, rivePlayer.Loop, rivePlayer.Direction);
        }
    }

    public void PlayAnimation(string animationName, RivePlayerLoop? loop = null, RivePlayerDirection? direction = null)
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.PlayAnimation(animationName, loop ?? Loop, direction ?? Direction);
        }
    }

    public void Play()
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.Play();
        }
    }

    public void Pause()
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.Pause();
        }
    }

    public void Stop()
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.Stop();
        }
    }

    public void Reset()
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.Reset();
        }
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.SetInput(stateMachineName, inputName, value);
        }
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.SetInput(stateMachineName, inputName, value);
        }
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        if (Handler is RivePlayerRenderer renderer)
        {
            renderer.TriggerInput(stateMachineName, inputName);
        }
    }
}