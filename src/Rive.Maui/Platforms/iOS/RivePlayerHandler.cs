using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Rive.Maui;

public partial class RivePlayerHandler() : ViewHandler<RivePlayer, WrapperView>(PropertyMapper, CommandMapper)
{
    private CustomRiveView? _riveView;

    protected override WrapperView CreatePlatformView()
    {
        var platformView = new WrapperView();
        _riveView = new CustomRiveView
        {
            ArtboardName = VirtualView.ArtboardName,
            AnimationName = VirtualView.AnimationName,
            StateMachineName = VirtualView.StateMachineName,
            AutoPlay = VirtualView.AutoPlay,
            Fit = VirtualView.Fit.AsRive(),
            Alignment = VirtualView.Alignment.AsRive(),
            Loop = VirtualView.Loop.AsRive(),
            Direction = VirtualView.Direction.AsRive()
        };

        _riveView.VirtualView.SetTarget(VirtualView);
        _riveView.Frame = VirtualView.Bounds;
        _riveView.SetRiveResource(VirtualView.ResourceName);

        platformView.AddSubview(_riveView);

        return platformView;
    }

    protected override void DisconnectHandler(WrapperView platformView)
    {
        VirtualView.StateMachineInputs.Dispose();
        platformView.RemoveFromSuperview();
        _riveView?.Dispose();
        platformView.Dispose();
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        if (!string.Equals(handler._riveView.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
        {
            handler._riveView.ArtboardName = view.ArtboardName;
            handler._riveView.ResetProperties(false);
            handler._riveView.UpdateAnimation();
        }
    }

    public static void MapAnimationName(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.AnimationName) || handler._riveView is null)
            return;

        handler._riveView.AnimationName = view.AnimationName;
        handler._riveView.ResetProperties(false);
        handler._riveView.UpdateAnimation();
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        if (!string.Equals(handler._riveView.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
        {
            handler._riveView.StateMachineName = view.StateMachineName;
            handler._riveView.ResetProperties(false);
            handler._riveView.UpdateAnimation();
        }
    }

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        handler._riveView.AutoPlay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        handler._riveView.Fit = view.Fit.AsRive();
    }

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        handler._riveView.Alignment = view.Alignment.AsRive();
    }

    public static void MapLoop(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        handler._riveView.Loop = view.Loop.AsRive();
    }

    public static void MapDirection(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler._riveView is null)
            return;

        handler._riveView.Direction = view.Direction.AsRive();
    }

    public static void MapPlay(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler._riveView?.Play();

    public static void MapPause(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler._riveView?.Pause();

    public static void MapStop(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler._riveView?.Stop();

    public static void MapReset(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler._riveView?.Reset();

    public static void MapSetInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is not StateMachineInputArgs inputArgs)
            return;

        switch (inputArgs.Value)
        {
            case double doubleValue:
                handler._riveView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler._riveView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler._riveView?.SetInput(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler._riveView?.TriggerInput(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }

    public static void MapSetTextRun(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is TextRun setTextRun)
        {
            handler._riveView?.SetTextRun(setTextRun.TextRunName, setTextRun.Value);
        }
    }
}