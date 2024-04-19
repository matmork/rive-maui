using Android.Content;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Rive.Android;
using Rive.Android.Core;

namespace Rive.Maui;

public partial class RivePlayerHandler : ViewHandler<RivePlayer, RiveAnimationView>
{
    private StateListener? _listener;

    protected override RiveAnimationView CreatePlatformView() => new RiveAnimationView(Context, null);

    protected override void ConnectHandler(RiveAnimationView platformView)
    {
        base.ConnectHandler(platformView);

        // Perform any control setup here
        platformView.ViewAttachedToWindow += OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow += OnViewDetachedFromWindow;

    }

    private void OnViewDetachedFromWindow(object? sender, global::Android.Views.View.ViewDetachedFromWindowEventArgs e)
    {
        if (_listener != null)
        {
            PlatformView.UnregisterListener(_listener);
            _listener.RivePlayerHandlerReference.SetTarget(null);
            _listener.Dispose();
            _listener = null;
        }

        //TODO check if sender is VirtualView
        VirtualView.StateMachineInputs.Dispose();
        //TODO check how to dispose properly
        //PlatformView = null;
    }

    private void OnViewAttachedToWindow(object? sender, global::Android.Views.View.ViewAttachedToWindowEventArgs e)
    {
        //TODO check if sender is VirtualView
        if (!string.IsNullOrWhiteSpace(VirtualView?.ResourceName) && PlatformView == null)
        {
            Load(VirtualView);
        }
    }

    protected override void DisconnectHandler(RiveAnimationView platformView)
    {
        // Perform any native view cleanup here

        platformView.ViewAttachedToWindow -= OnViewAttachedToWindow;
        platformView.ViewDetachedFromWindow += OnViewDetachedFromWindow;

        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    public static void MapArtboardName(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            platformView.ArtboardName = view.ArtboardName;
        }
    }

    public static void MapAnimationProperties(RivePlayerHandler handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.AnimationName))
        {
            return;
        }

        handler.PlayAnimation(view.AnimationName, view.Loop, view.Direction);
    }

    public static void MapStateMachineName(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            var rendererAttributes = platformView.GetRendererAttributes();
            rendererAttributes.StateMachineName = view.StateMachineName;
        }
    }

    public static void MapResourceName(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            handler.Load(view);
        }
    }

    public static void MapAutoPlay(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            platformView.Autoplay = view.AutoPlay;
        }
    }

    public static void MapFit(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            platformView.Fit = GetFit(view.Fit);
        }
    }

    public static void MapAlignment(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            platformView.Alignment = GetAlignment(view.Alignment);
        }
    }

    public static void MapOnStateMachineChangeCommand(RivePlayerHandler handler, RivePlayer view)
    {
        if (handler.PlatformView is RiveAnimationView platformView)
        {
            if (handler._listener is not null)
            {
                platformView.UnregisterListener(handler._listener);
                handler._listener.RivePlayerHandlerReference.SetTarget(null);
                handler._listener.Dispose();
                handler._listener = null;
            }
            if (view.OnStateMachineChangeCommand != null)
            {
                handler._listener = new StateListener();
                handler._listener.RivePlayerHandlerReference.SetTarget(handler);
                platformView.RegisterListener(handler._listener);
            }
        }
    }

    private void Load(RivePlayer control)
    {
        var resourceIdentifier = Context.Resources!.GetIdentifier(control.ResourceName, "drawable", Context.PackageName);
        if (resourceIdentifier == 0)
        {
            return;
        }

        var riveAlignment = GetAlignment(control.Alignment);
        var riveFit = GetFit(control.Fit);
        var riveLoop = GetLoop(control.Loop);

        this.PlatformView.SetRiveResource(
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

    //public void PlayAnimation(string animationName, RivePlayerLoop? loop = null, RivePlayerDirection? direction = null)
    //{
    //    if (Handler is RivePlayerRenderer renderer && !string.IsNullOrWhiteSpace(animationName))
    //    {
    //        renderer.PlayAnimation(animationName, loop ?? Loop, direction ?? Direction);
    //    }
    //}

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction)
    {
        var riveLoop = GetLoop(loop);
        var riveDirection = GetDirection(direction);

        PlatformView.Play(animationName, riveLoop, riveDirection, false, true);
    }

    public static void MapPlay(RivePlayerHandler handler, RivePlayer view, object? args)
    {
        var riveLoop = GetLoop(view!.Loop);
        var riveDirection = GetDirection(view.Direction);

        handler.PlatformView?.Play(riveLoop, riveDirection, true);
    }

    public static void MapPause(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView?.Pause();

    public static void MapStop(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView?.Stop();

    public static void MapReset(RivePlayerHandler handler, RivePlayer view, object? args)
        => handler.PlatformView?.Reset();

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        PlatformView.SetBooleanState(stateMachineName, inputName, value);
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {

        PlatformView.SetNumberState(stateMachineName, inputName, value);
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        PlatformView.FireState(stateMachineName!, inputName!);
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
}
