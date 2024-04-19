using System.Windows.Input;
using static Android.Provider.MediaStore;

namespace Rive.Maui;

[ContentProperty(nameof(StateMachineInputs))]
public class RivePlayer : View
{
    public event EventHandler PlayRequested;
    public event EventHandler PauseRequested;
    public event EventHandler StopRequested;
    public event EventHandler ResetRequested;

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
        defaultValue: null
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

    //public ICommand PlayAnimationCommand => new Command<string>(animationName => PlayAnimation(animationName, Loop, Direction));

    public ICommand PlayCommand => new Command(Play);

    public ICommand PauseCommand => new Command(Pause);

    public ICommand StopCommand => new Command(Stop);

    public ICommand ResetCommand => new Command(Reset);

    public RivePlayer()
    {
        StateMachineInputs = new StateMachineInputCollection(this);
    }

    public void Play()
    {
        this.PlayRequested?.Invoke(this, null!);
        Handler?.Invoke(nameof(RivePlayer.PlayRequested));
    }

    public void Pause()
    {
        this.PauseRequested?.Invoke(this, null!);
        Handler?.Invoke(nameof(RivePlayer.PauseRequested));
    }

    public void Stop()
    {
        this.StopRequested?.Invoke(this, null!);
        Handler?.Invoke(nameof(RivePlayer.StopRequested));
    }

    public void Reset()
    {
        this.ResetRequested?.Invoke(this, null!);
        Handler?.Invoke(nameof(RivePlayer.ResetRequested));
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        if (Handler is RivePlayerHandler handler)
        {
            handler.SetInput(stateMachineName, inputName, value);
        }
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        if (Handler is RivePlayerHandler handler)
        {
            handler.SetInput(stateMachineName, inputName, value);
        }
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        if (Handler is RivePlayerHandler handler)
        {
            handler.TriggerInput(stateMachineName, inputName);
        }
    }
}