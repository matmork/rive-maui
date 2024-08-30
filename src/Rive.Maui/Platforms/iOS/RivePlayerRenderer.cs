using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using UIKit;

namespace Rive.Maui;

public partial class RivePlayerRenderer() : ViewRenderer<RivePlayer, UIView>(PropertyMapper, CommandMapper), IRivePlayerRenderer
{
    internal CustomRiveView? _riveAnimationView;

    protected override void OnElementChanged(ElementChangedEventArgs<RivePlayer> e)
    {
        base.OnElementChanged(e);

        if (e.OldElement != null)
        {
            e.OldElement.StateMachineInputs.Dispose();
            _riveAnimationView?.Dispose();
            _riveAnimationView = null;
        }

        if (e.NewElement != null && !string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            CreatePlatformView(e.NewElement);
        }
    }

    protected override void DisconnectHandler(UIView oldNativeView)
    {
        Element?.StateMachineInputs.Dispose();
        _riveAnimationView?.Dispose();
        _riveAnimationView = null;
        oldNativeView.Dispose();

        base.DisconnectHandler(oldNativeView);
    }

    private void CreatePlatformView(RivePlayer control)
    {
        if (_riveAnimationView != null)
            return;

        var platformView = new CustomRiveView
        {
            ArtboardName = control.ArtboardName,
            AnimationName = control.AnimationName,
            StateMachineName = control.StateMachineName,
            AutoPlay = control.AutoPlay,
            Fit = control.Fit.AsRive(),
            Alignment = control.Alignment.AsRive(),
            Loop = control.Loop.AsRive(),
            Direction = control.Direction.AsRive()
        };

        platformView.SetRiveResource(control.ResourceName);
        platformView.Control.SetTarget(control);
        platformView.Frame = control.Bounds;

        _riveAnimationView = platformView;
        SetNativeControl(_riveAnimationView);
    }

    public static void MapArtboardName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null &&
            !string.Equals(handler._riveAnimationView.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
        {
            handler._riveAnimationView.ArtboardName = view.ArtboardName;
            handler._riveAnimationView.ResetProperties(false);
            handler._riveAnimationView.UpdateAnimation();
        }
    }

    public static void MapAnimationName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView == null || string.IsNullOrWhiteSpace(view.AnimationName))
            return;

        handler._riveAnimationView.AnimationName = view.AnimationName;
        handler._riveAnimationView.ResetProperties(false);
        handler._riveAnimationView.UpdateAnimation();
    }

    public static void MapStateMachineName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null &&
            !string.Equals(handler._riveAnimationView.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
        {
            handler._riveAnimationView.StateMachineName = view.StateMachineName;
            handler._riveAnimationView.ResetProperties(false);
            handler._riveAnimationView.UpdateAnimation();
        }
    }

    public static void MapResourceName(RivePlayerRenderer handler, RivePlayer view)
        => handler._riveAnimationView?.SetRiveResource(view.ResourceName);

    public static void MapAutoPlay(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null)
            handler._riveAnimationView.AutoPlay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null)
            handler._riveAnimationView.Fit = view.Fit.AsRive();
    }

    public static void MapAlignment(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null)
            handler._riveAnimationView.Alignment = view.Alignment.AsRive();
    }

    public static void MapLoop(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null)
            handler._riveAnimationView.Loop = view.Loop.AsRive();
    }

    public static void MapDirection(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null)
            handler._riveAnimationView.Direction = view.Direction.AsRive();
    }

    public static void MapPlay(RivePlayerRenderer handler, RivePlayer view, object? args)
        => handler._riveAnimationView?.Play();

    public static void MapPause(RivePlayerRenderer handler, RivePlayer view, object? args)
        => handler._riveAnimationView?.Pause();

    public static void MapStop(RivePlayerRenderer handler, RivePlayer view, object? args)
        => handler._riveAnimationView?.Stop();

    public static void MapReset(RivePlayerRenderer handler, RivePlayer view, object? args)
        => handler._riveAnimationView?.Reset();

    public static void MapSetInput(RivePlayerRenderer handler, RivePlayer view, object? args)
    {
        if (args is not StateMachineInputArgs inputArgs)
            return;

        switch (inputArgs.Value)
        {
            case double doubleValue:
                handler._riveAnimationView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler._riveAnimationView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler._riveAnimationView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerRenderer handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler._riveAnimationView?.TriggerInput(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }
}