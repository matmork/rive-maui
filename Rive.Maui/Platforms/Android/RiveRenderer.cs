using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Rive.Android;
using Rive.Android.Controllers;
using Rive.Android.Core;
using View = Android.Views.View;

namespace Rive.Maui;

internal partial class RiveRenderer(Context context) : ViewRenderer<Rive, View>(context)
{
    private RiveAnimationView? _riveView;
    private StateListener? _listener;

    protected override void OnAttachedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        if (_listener != null)
        {
            _riveView?.UnregisterListener(_listener);
            _listener.Control.SetTarget(null);
            _listener.Dispose();
            _listener = null;
        }

        Element?.StateMachineInputs.Dispose();

        // https://github.com/rive-app/rive-android/blob/master/MEMORY_MANAGEMENT.md
        _riveView = null;

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

        var riveFit = control.Fit switch
        {
            Fit.Fill => Android.Core.Fit.Fill!,
            Fit.Contain => Android.Core.Fit.Contain!,
            Fit.Cover => Android.Core.Fit.Cover!,
            Fit.FitHeight => Android.Core.Fit.FitHeight!,
            Fit.FitWidth => Android.Core.Fit.FitWidth!,
            Fit.ScaleDown => Android.Core.Fit.ScaleDown!,
            Fit.NoFit => Android.Core.Fit.None!,
            _ => Android.Core.Fit.Contain!
        };

        var riveAlignment = control.Alignment switch
        {
            Alignment.TopLeft => Android.Core.Alignment.TopLeft!,
            Alignment.TopCenter => Android.Core.Alignment.TopCenter!,
            Alignment.TopRight => Android.Core.Alignment.TopRight!,
            Alignment.CenterLeft => Android.Core.Alignment.CenterLeft!,
            Alignment.Center => Android.Core.Alignment.Center!,
            Alignment.CenterRight => Android.Core.Alignment.CenterRight!,
            Alignment.BottomLeft => Android.Core.Alignment.BottomLeft!,
            Alignment.BottomCenter => Android.Core.Alignment.BottomCenter!,
            Alignment.BottomRight => Android.Core.Alignment.BottomRight!,
            _ => Android.Core.Alignment.Center!
        };

        _riveView = new RiveAnimationView(context, null);

        _listener = new StateListener();
        _listener.Control.SetTarget(control);
        _riveView.RegisterListener(_listener);

        _riveView.SetRiveResource(
            identifier,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.AutoPlay,
            riveFit,
            riveAlignment,
            Android.Core.Loop.Auto!
        );

        _riveView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        SetNativeControl(_riveView);
    }

    public void PlayAnimation(string animationName, Loop loop, Direction direction)
    {
        if (_riveView is null) return;

        var riveLoop = loop switch
        {
            Loop.OneShot => Android.Core.Loop.Oneshot!,
            Loop.Loop => Android.Core.Loop.Loop2!,
            Loop.PingPong => Android.Core.Loop.Pingpong!,
            Loop.AutoLoop => Android.Core.Loop.Auto!,
            _ => Android.Core.Loop.Auto!
        };

        var riveDirection = direction switch
        {
            Direction.Backwards => Android.Core.Direction.Backwards!,
            Direction.Forwards => Android.Core.Direction.Forwards!,
            Direction.AutoDirection => Android.Core.Direction.Auto!,
            _ => Android.Core.Direction.Auto!
        };

        _riveView.Play(animationName, riveLoop, riveDirection, false, true);
    }

    public void Pause()
    {
        _riveView?.Pause();
    }

    public void Stop()
    {
        _riveView?.Stop();
    }

    public void Reset()
    {
        _riveView?.Reset();
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        _riveView?.SetBooleanState(stateMachineName, inputName, value);
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        _riveView?.SetNumberState(stateMachineName, inputName, value);
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        _riveView?.FireState(stateMachineName, inputName);
    }
}

internal class StateListener : Java.Lang.Object, RiveFileController.IListener
{
    public WeakReference<Rive?> Control { get; set; } = new(null);

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
        if (Control.TryGetTarget(out var control))
        {
            control.OnStateMachineChangeCommand?.Execute(new StateMachineChange(stateMachineName, stateName));
        }
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}