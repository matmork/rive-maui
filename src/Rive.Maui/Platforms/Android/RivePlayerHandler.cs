using Microsoft.Maui.Handlers;

namespace Rive.Maui;

public partial class RivePlayerHandler() : ViewHandler<RivePlayer, RiveView>(PropertyMapper, CommandMapper)
{
    protected override RiveView CreatePlatformView()
    {
        var riveView = new RiveView(Context, VirtualView);
        return riveView;
    }

    protected override void ConnectHandler(RiveView platformView)
    {
        platformView.CreateAnimationView();
        //riveView.CreateAnimationView();
    }

    protected override void DisconnectHandler(RiveView platformView)
    {
        platformView.Dispose();
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.Equals(handler.PlatformView.AnimationView?.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
            handler.PlatformView.AnimationView.ArtboardName = view.ArtboardName;
    }

    public static void MapAnimationName(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.AnimationName))
            return;

        var riveLoop = view.Loop.AsRive();
        var riveDirection = view.Direction.AsRive();

        if (view.AutoPlay)
        {
            handler.PlatformView.AnimationView.Stop();
            handler.PlatformView.AnimationView.Play(view.AnimationName, riveLoop, riveDirection, false, true);
        }
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        var rendererAttributes = handler.PlatformView.AnimationView.GetRendererAttributes();

        if (!string.Equals(rendererAttributes.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
            rendererAttributes.StateMachineName = view.StateMachineName;
    }

    public static void MapResourceName(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.ResourceName) ||
            string.Equals(view.ResourceName, handler.PlatformView.ResourceName, StringComparison.OrdinalIgnoreCase))
            return;

        handler.CreatePlatformView();
    }

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView.AnimationView.Autoplay != view.AutoPlay)
            handler.PlatformView.AnimationView.Autoplay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
    {
        var riveFit = view.Fit.AsRive();
        if (handler.PlatformView.AnimationView.Fit != riveFit)
            handler.PlatformView.AnimationView.Fit = riveFit;
    }

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
    {
        var riveAlignment = view.Alignment.AsRive();
        if (handler.PlatformView.AnimationView.Alignment != riveAlignment)
            handler.PlatformView.AnimationView.Alignment = riveAlignment;
    }

    public static void MapLoop(RivePlayerHandler handler, RivePlayer view)
    {
        var rendererAttributes = handler.PlatformView.AnimationView.GetRendererAttributes();
        rendererAttributes.Loop = view.Loop.AsRive();
    }

    public static void MapDirection(RivePlayerHandler handler, RivePlayer view)
    {
        //
    }

    public static void MapPlay(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        var riveLoop = view.Loop.AsRive();
        var riveDirection = view.Direction.AsRive();

        if (!string.IsNullOrWhiteSpace(view.AnimationName))
        {
            handler.PlatformView.AnimationView.Play(view.AnimationName, riveLoop, riveDirection, false, true);
            handler.PlatformView.RiveAnimation = handler.PlatformView.AnimationView.Animations.FirstOrDefault(x => x.Name == view.AnimationName);
            return;
        }

        var stateMachineName = !string.IsNullOrWhiteSpace(view.StateMachineName)
            ? view.StateMachineName
            : handler.PlatformView.AnimationView.Controller.ActiveArtboard?.StateMachineNames.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(stateMachineName))
        {
            handler.PlatformView.AnimationView.Play(stateMachineName, riveLoop, riveDirection, true, true);
            return;
        }

        var firstAnimationName = handler.PlatformView.AnimationView.Controller.ActiveArtboard?.AnimationNames.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(firstAnimationName))
        {
            handler.PlatformView.AnimationView.Play(firstAnimationName, riveLoop, riveDirection, false, true);
        }
    }

    public static void MapPause(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.AnimationView.Pause();

    public static void MapStop(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.AnimationView.Stop();

    public static void MapReset(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView.AnimationView.Reset();

    public static void MapSetInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is not StateMachineInputArgs inputArgs)
            return;

        switch (inputArgs.Value)
        {
            case double doubleValue:
                handler.PlatformView.AnimationView.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler.PlatformView.AnimationView.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler.PlatformView.AnimationView.SetBooleanState(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler.PlatformView.AnimationView.FireState(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }
}