using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Rive.Android;
using Rive.Android.Controllers;
using Rive.Android.Core;
using View = Android.Views.View;

namespace Rive.Maui;

internal partial class RivePlayerRenderer(Context context) : ViewRenderer<RivePlayer, View>(context)
{
    public RiveAnimationView? RiveView;
    private StateListener? _listener;

    protected override void OnElementChanged(ElementChangedEventArgs<RivePlayer> e)
    {
        base.OnElementChanged(e);

        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }
    }

    // https://github.com/rive-app/rive-android/blob/master/MEMORY_MANAGEMENT.md
    protected override void OnAttachedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName) && RiveView == null)
        {
            Load();
        }

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        if (_listener != null)
        {
            RiveView?.UnregisterListener(_listener);
            _listener.RivePlayerRendererReference.SetTarget(null);
            _listener.Dispose();
            _listener = null;
        }

        Element?.StateMachineInputs.Dispose();
        RiveView = null;

        base.OnDetachedFromWindow();
    }

    private void Load()
    {
        var context = Platform.AppContext;
        var control = Element!;

        var identifier = context.Resources!.GetIdentifier(control.ResourceName, "drawable", context.PackageName);
        if (identifier == 0)
        {
            return;
        }

        RiveView = new RiveAnimationView(context, null);

        if (control.OnStateMachineChangeCommand != null)
        {
            _listener = new StateListener();
            _listener.RivePlayerRendererReference.SetTarget(this);
            RiveView.RegisterListener(_listener);
        }

        var riveAlignment = GetAlignment(control.Alignment);
        var riveFit = GetFit(control.Fit);
        var riveLoop = GetLoop(control.Loop);

        RiveView.SetRiveResource(
            identifier,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.AutoPlay,
            riveFit,
            riveAlignment,
            riveLoop
        );

        RiveView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        SetNativeControl(RiveView);
    }

    private static Alignment GetAlignment(RivePlayerAlignment alignment)
    {
        return alignment switch
        {
            RivePlayerAlignment.TopLeft => Alignment.TopLeft!,
            RivePlayerAlignment.TopCenter => Alignment.TopCenter!,
            RivePlayerAlignment.TopRight => Alignment.TopRight!,
            RivePlayerAlignment.CenterLeft => Alignment.CenterLeft!,
            RivePlayerAlignment.Center => Alignment.Center!,
            RivePlayerAlignment.CenterRight => Alignment.CenterRight!,
            RivePlayerAlignment.BottomLeft => Alignment.BottomLeft!,
            RivePlayerAlignment.BottomCenter => Alignment.BottomCenter!,
            RivePlayerAlignment.BottomRight => Alignment.BottomRight!,
            _ => Alignment.Center!
        };
    }

    private static Fit GetFit(RivePlayerFit fit)
    {
        return fit switch
        {
            RivePlayerFit.Fill => Fit.Fill!,
            RivePlayerFit.Contain => Fit.Contain!,
            RivePlayerFit.Cover => Fit.Cover!,
            RivePlayerFit.FitHeight => Fit.FitHeight!,
            RivePlayerFit.FitWidth => Fit.FitWidth!,
            RivePlayerFit.ScaleDown => Fit.ScaleDown!,
            RivePlayerFit.NoFit => Fit.None!,
            _ => Fit.Contain!
        };
    }

    private static Loop GetLoop(RivePlayerLoop loop)
    {
        return loop switch
        {
            RivePlayerLoop.OneShot => Loop.Oneshot!,
            RivePlayerLoop.Loop => Loop.Loop2!,
            RivePlayerLoop.PingPong => Loop.Pingpong!,
            RivePlayerLoop.AutoLoop => Loop.Auto!,
            _ => Loop.Auto!
        };
    }

    private static Direction GetDirection(RivePlayerDirection direction)
    {
        return direction switch
        {
            RivePlayerDirection.Backwards => Direction.Backwards!,
            RivePlayerDirection.Forwards => Direction.Forwards!,
            RivePlayerDirection.AutoDirection => Direction.Auto!,
            _ => Direction.Auto!
        };
    }

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction)
    {
        if (RiveView is null) return;

        var riveLoop = GetLoop(loop);
        var riveDirection = GetDirection(direction);

        RiveView.Play(animationName, riveLoop, riveDirection, false, true);
    }

    public void Play()
    {
        var riveLoop = GetLoop(Element!.Loop);
        var riveDirection = GetDirection(Element.Direction);

        RiveView?.Play(riveLoop, riveDirection, true);
    }

    public void Pause() => RiveView?.Pause();
    public void Stop() => RiveView?.Stop();

    public void Reset() => RiveView?.Reset();

    public void SetInput(string stateMachineName, string inputName, bool value) => RiveView?.SetBooleanState(stateMachineName, inputName, value);

    public void SetInput(string stateMachineName, string inputName, float value) => RiveView?.SetNumberState(stateMachineName, inputName, value);

    public void TriggerInput(string stateMachineName, string inputName) => RiveView?.FireState(stateMachineName, inputName);
}

internal class StateListener : Java.Lang.Object, RiveFileController.IListener
{
    public WeakReference<RivePlayerRenderer?> RivePlayerRendererReference { get; set; } = new(null);

    public void NotifyAdvance(float elapsed)
    {
    }

    public void NotifyLoop(IPlayableInstance animation)
    {
    }

    public void NotifyPause(IPlayableInstance animation)
    {
    }

    public void NotifyPlay(IPlayableInstance animation)
    {
    }

    public void NotifyStateChanged(string stateMachineName, string stateName)
    {
        if (RivePlayerRendererReference.TryGetTarget(out var renderer)
            && renderer is { RiveView: not null, Element.OnStateMachineChangeCommand: not null })
        {
            var inputs = new Dictionary<string, object>();
            foreach (var stateMachine in renderer.RiveView.StateMachines)
            {
                foreach (var input in stateMachine.Inputs)
                {
                    switch (input)
                    {
                        case SMINumber smiNumber:
                            inputs.Add(smiNumber.Name, smiNumber.Value);
                            break;
                        case SMIBoolean smiBool:
                            inputs.Add(smiBool.Name, smiBool.Value);
                            break;
                    }
                }
            }

            renderer.Element.OnStateMachineChangeCommand?.Execute(new StateMachineChange(stateMachineName, stateName, inputs));
        }
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}