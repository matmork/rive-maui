using System.Windows.Input;

namespace Rive.Maui;

[ContentProperty(nameof(StateMachineInputs))]
public class RivePlayer : View
{
    internal readonly WeakEventManager StateChangedManager = new();

    internal readonly WeakEventManager EventReceivedManager = new();

    public event EventHandler<string> StateChanged
    {
        add => StateChangedManager.AddEventHandler(value);
        remove => StateChangedManager.RemoveEventHandler(value);
    }

    public event EventHandler<string> EventReceived
    {
        add => EventReceivedManager.AddEventHandler(value);
        remove => EventReceivedManager.RemoveEventHandler(value);
    }

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

    public static readonly BindableProperty StateChangedCommandProperty = BindableProperty.Create(
        nameof(StateChangedCommand),
        typeof(ICommand),
        typeof(RivePlayer)
    );

    public static readonly BindableProperty EventReceivedCommandProperty = BindableProperty.Create(
        nameof(EventReceivedCommand),
        typeof(ICommand),
        typeof(RivePlayer)
    );

    public static readonly BindableProperty StateMachineInputsProperty = BindableProperty.Create(
        nameof(StateMachineInputs),
        typeof(StateMachineInputCollection),
        typeof(RivePlayer)
    );

    public static readonly BindableProperty DynamicAssetsProperty = BindableProperty.Create(
        nameof(DynamicAssets),
        typeof(List<DynamicAsset>),
        typeof(RivePlayer)
    );

    public static readonly BindableProperty TextRunsProperty = BindableProperty.Create(
        nameof(TextRuns),
        typeof(List<TextRun>),
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

    public ICommand? StateChangedCommand
    {
        get => (ICommand?)GetValue(StateChangedCommandProperty);
        set => SetValue(StateChangedCommandProperty, value);
    }

    public ICommand? EventReceivedCommand
    {
        get => (ICommand?)GetValue(EventReceivedCommandProperty);
        set => SetValue(EventReceivedCommandProperty, value);
    }

    public StateMachineInputCollection StateMachineInputs
    {
        get => (StateMachineInputCollection)GetValue(StateMachineInputsProperty);
        set => SetValue(StateMachineInputsProperty, value);
    }

    public List<DynamicAsset>? DynamicAssets
    {
        get => (List<DynamicAsset>?)GetValue(DynamicAssetsProperty);
        set => SetValue(DynamicAssetsProperty, value);
    }

    public List<TextRun>? TextRuns
    {
        get => (List<TextRun>?)GetValue(TextRunsProperty);
        set => SetValue(TextRunsProperty, value);
    }

    public ICommand PlayCommand
        => new Command(Play);

    public ICommand PauseCommand
        => new Command(Pause);

    public ICommand StopCommand
        => new Command(Stop);

    public ICommand ResetCommand
        => new Command(Reset);

    public RivePlayer()
        => StateMachineInputs = new StateMachineInputCollection(this);

    public void Play()
        => Handler?.Invoke(nameof(Play));

    public void Pause()
        => Handler?.Invoke(nameof(Pause));

    public void Stop()
        => Handler?.Invoke(nameof(Stop));

    public void Reset()
        => Handler?.Invoke(nameof(Reset));

    public void SetInput(StateMachineInputArgs args)
        => Handler?.Invoke(nameof(SetInput), args);

    public void TriggerInput(StateMachineTriggerInputArgs args)
        => Handler?.Invoke(nameof(TriggerInput), args);

    public void SetTextRun(TextRun args)
        => Handler?.Invoke(nameof(SetTextRun), args);

    protected override void OnHandlerChanged()
    {
        if (Handler == null) return;

        foreach (var input in StateMachineInputs)
        {
            input.Apply();
        }
    }

    protected override void OnBindingContextChanged()
    {
        foreach (var input in StateMachineInputs)
        {
            input.BindingContext = BindingContext;
        }

        base.OnBindingContextChanged();
    }
}