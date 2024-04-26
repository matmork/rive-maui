using Android.Views;
using Microsoft.Maui.Handlers;
using Rive.Android;
using View = Android.Views.View;

namespace Rive.Maui;

public partial class RivePlayerHandler() : ViewHandler<RivePlayer, RiveAnimationView>(PropertyMapper, CommandMapper)
{
    private StateListener? listener;
    private string? _resourceName;

    protected override RiveAnimationView CreatePlatformView()
    {
        var platformView = new RiveAnimationView(Context, null);
        platformView.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent
        );

        return platformView;
    }

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
        SetRiveResource();
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

    private void SetRiveResource()
    {
        if (string.IsNullOrWhiteSpace(VirtualView.ResourceName))
            return;

        var resourceIdentifier = Context.Resources?.GetIdentifier(VirtualView.ResourceName, "drawable", Context.PackageName) ?? 0;
        if (resourceIdentifier == 0)
            return;

        _resourceName = VirtualView.ResourceName;

        var riveAlignment = VirtualView.Alignment.AsRive();
        var riveFit = VirtualView.Fit.AsRive();
        var riveLoop = VirtualView.Loop.AsRive();

        PlatformView.SetRiveResource(
            resourceIdentifier,
            VirtualView.ArtboardName,
            VirtualView.AnimationName,
            VirtualView.StateMachineName,
            VirtualView.AutoPlay,
            riveFit,
            riveAlignment,
            riveLoop
        );
    }

    public static void MapAnimationProperties(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.IsNullOrWhiteSpace(view.AnimationName))
        {
            var riveLoop = view.Loop.AsRive();
            var riveDirection = view.Direction.AsRive();

            handler.PlatformView.Play(view.AnimationName, riveLoop, riveDirection, string.IsNullOrWhiteSpace(view.StateMachineName), true);
        }
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
    {
        if (!string.Equals(handler.PlatformView.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
            handler.PlatformView.ArtboardName = view.ArtboardName;
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        var rendererAttributes = handler.PlatformView.GetRendererAttributes();

        if (!string.Equals(rendererAttributes.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
            rendererAttributes.StateMachineName = view.StateMachineName;
    }

    public static void MapResourceName(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.ResourceName) ||
            string.Equals(view.ResourceName, handler._resourceName, StringComparison.OrdinalIgnoreCase))
            return;

        handler.SetRiveResource();
    }

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView.Autoplay != view.AutoPlay)
            handler.PlatformView.Autoplay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
    {
        var riveFit = view.Fit.AsRive();
        if (handler.PlatformView.Fit != riveFit)
            handler.PlatformView.Fit = riveFit;
    }

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
    {
        var riveAlignment = view.Alignment.AsRive();
        if (handler.PlatformView.Alignment != riveAlignment)
            handler.PlatformView.Alignment = riveAlignment;
    }

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