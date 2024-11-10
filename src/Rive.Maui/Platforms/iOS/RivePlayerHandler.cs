using Microsoft.Maui.Handlers;

namespace Rive.Maui;

public partial class RivePlayerHandler() : ViewHandler<RivePlayer, CustomRiveView>(PropertyMapper, CommandMapper)
{
    protected override CustomRiveView CreatePlatformView()
    {
        var platformView = new CustomRiveView
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

        platformView.Control.SetTarget(VirtualView);
        platformView.Frame = VirtualView.Bounds;
        platformView.SetRiveResource(VirtualView.ResourceName);

        return platformView;
    }

    protected override void DisconnectHandler(CustomRiveView platformView)
    {
        VirtualView.StateMachineInputs.Dispose();
        platformView.Dispose();
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.Equals(handler.PlatformView.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
        {
            handler.PlatformView.ArtboardName = view.ArtboardName;
            handler.PlatformView.ResetProperties(false);
            handler.PlatformView.UpdateAnimation();
        }
    }

    public static void MapAnimationName(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.AnimationName))
            return;

        handler.PlatformView.AnimationName = view.AnimationName;
        handler.PlatformView.ResetProperties(false);
        handler.PlatformView.UpdateAnimation();
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.Equals(handler.PlatformView.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
        {
            handler.PlatformView.StateMachineName = view.StateMachineName;
            handler.PlatformView.ResetProperties(false);
            handler.PlatformView.UpdateAnimation();
        }
    }

    public static void MapResourceName(RivePlayerHandler handler, RivePlayer view)
        => handler.PlatformView?.SetRiveResource(view.ResourceName);

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
    {
        handler.PlatformView.AutoPlay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
    {
        handler.PlatformView.Fit = view.Fit.AsRive();
    }

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
    {
        handler.PlatformView.Alignment = view.Alignment.AsRive();
    }

    public static void MapLoop(RivePlayerHandler handler, RivePlayer view)
    {
        handler.PlatformView.Loop = view.Loop.AsRive();
    }

    public static void MapDirection(RivePlayerHandler handler, RivePlayer view)
    {
        handler.PlatformView.Direction = view.Direction.AsRive();
    }

    public static void MapPlay(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.Play();

    public static void MapPause(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.Pause();

    public static void MapStop(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.Stop();

    public static void MapReset(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.Reset();

    public static void MapSetInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is not StateMachineInputArgs inputArgs)
            return;

        switch (inputArgs.Value)
        {
            case double doubleValue:
                handler.PlatformView.SetInput(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler.PlatformView.SetInput(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler.PlatformView.SetInput(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler.PlatformView.TriggerInput(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }
}