using Android.Views;
using Microsoft.Maui.Handlers;
using Rive.Android;
using View = Android.Views.View;

namespace Rive.Maui;

public partial class RivePlayerHandler() : ViewHandler<RivePlayer, RiveAnimationView>(PropertyMapper, CommandMapper)
{
    private StateListener? listener;

    protected override RiveAnimationView CreatePlatformView()
        => new(Context, null);

    protected override void ConnectHandler(RiveAnimationView platformView)
    {
        base.ConnectHandler(platformView);

        platformView.ViewAttachedToWindow += OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow += OnViewDetachedFromWindow;
    }

    protected override void DisconnectHandler(RiveAnimationView platformView)
    {
        platformView.ViewAttachedToWindow -= OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow -= OnViewDetachedFromWindow;
        platformView.Dispose();

        base.DisconnectHandler(platformView);
    }

    private void OnViewAttachedToWindow(object? sender, View.ViewAttachedToWindowEventArgs e)
    {
        Load(VirtualView);
    }

    private void OnViewDetachedFromWindow(object? sender, View.ViewDetachedFromWindowEventArgs e)
    {
        if (listener != null)
        {
            PlatformView.UnregisterListener(listener);
            listener.RivePlayerHandlerReference.SetTarget(null);
            listener.Dispose();
            listener = null;
        }

        VirtualView.StateMachineInputs.Dispose();
    }

    private void Load(RivePlayer control)
    {
        var resourceIdentifier = Context.Resources!.GetIdentifier(control.ResourceName, "drawable", Context.PackageName);
        if (resourceIdentifier == 0)
        {
            return;
        }

        var riveAlignment = control.Alignment.AsRive();
        var riveFit = control.Fit.AsRive();
        var riveLoop = control.Loop.AsRive();

        PlatformView.SetRiveResource(
            resourceIdentifier,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.AutoPlay,
            riveFit,
            riveAlignment,
            riveLoop
        );

        PlatformView.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent
        );
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
        => handler.PlatformView.ArtboardName = view.ArtboardName;

    public static void MapAnimationProperties(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.IsNullOrWhiteSpace(view.AnimationName))
        {
            var riveLoop = view.Loop.AsRive();
            var riveDirection = view.Direction.AsRive();

            handler.PlatformView.Play(view.AnimationName, riveLoop, riveDirection, false, true);
        }
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        var rendererAttributes = handler.PlatformView.GetRendererAttributes();
        rendererAttributes.StateMachineName = view.StateMachineName;
    }

    public static void MapResourceName(RivePlayerHandler handler, RivePlayer view)
    {
        handler.Load(view);
    }

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
        => handler.PlatformView.Autoplay = view.AutoPlay;

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
        => handler.PlatformView.Fit = view.Fit.AsRive();

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
        => handler.PlatformView.Alignment = view.Alignment.AsRive();

    public static void MapStateChangedCommand(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.listener == null)
        {
            handler.listener = new StateListener();
            handler.listener.RivePlayerHandlerReference.SetTarget(handler);
            handler.PlatformView.RegisterListener(handler.listener);
        }
    }

    public static void MapPlay(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        var riveLoop = view.Loop.AsRive();
        var riveDirection = view.Direction.AsRive();

        handler.PlatformView.Play(riveLoop, riveDirection, true);
    }

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
                handler.PlatformView.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler.PlatformView.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler.PlatformView.SetBooleanState(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler.PlatformView.FireState(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }
}