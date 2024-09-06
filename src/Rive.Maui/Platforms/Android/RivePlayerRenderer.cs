using Android.Content;
using Rive.Android;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

namespace Rive.Maui;

public partial class RivePlayerRenderer(Context context) : ViewRenderer<RivePlayer, View>(context, PropertyMapper, CommandMapper), IRivePlayerRenderer
{
    internal RiveAnimationView? _riveAnimationView;
    private View? _tmpView;
    private StateListener? _stateListener;
    private EventListener? _eventListener;
    private string? _resourceName;

    protected override void OnElementChanged(ElementChangedEventArgs<RivePlayer> e)
    {
        base.OnElementChanged(e);

        if (_riveAnimationView == null)
            CreatePlatformView();
    }

    protected override void OnAttachedToWindow()
    {
        if (_riveAnimationView == null)
            CreatePlatformView();

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        // https://github.com/rive-app/rive-android/blob/master/MEMORY_MANAGEMENT.md
        if (_tmpView != null)
            SetNativeControl(_tmpView);

        Destroy();

        base.OnDetachedFromWindow();
    }

    private void Destroy()
    {
        if (_stateListener != null)
        {
            _riveAnimationView?.UnregisterListener(_stateListener);
            _stateListener.RivePlayerHandlerReference.SetTarget(null);
            _stateListener.Dispose();
            _stateListener = null;
        }

        if (_eventListener != null)
        {
            _riveAnimationView?.RemoveEventListener(_eventListener);
            _eventListener.RivePlayerHandlerReference.SetTarget(null);
            _eventListener.Dispose();
            _eventListener = null;
        }

        Element?.StateMachineInputs.Dispose();
        _riveAnimationView?.Dispose();
        _riveAnimationView = null;
    }

    protected override void DisconnectHandler(View oldPlatformView)
    {
        Destroy();
        oldPlatformView.Dispose();

        base.DisconnectHandler(oldPlatformView);
    }

    private void CreatePlatformView()
    {
        if (Context == null || string.IsNullOrWhiteSpace(Element?.ResourceName))
            return;

        var resourceIdentifier = Context.Resources?.GetIdentifier(Element.ResourceName, "drawable", Context.PackageName) ?? 0;
        if (resourceIdentifier == 0)
            return;

        _resourceName = Element.ResourceName;

        var riveAlignment = Element.Alignment.AsRive();
        var riveFit = Element.Fit.AsRive();
        var riveLoop = Element.Loop.AsRive();

        _tmpView = new View(Context);
        _riveAnimationView = new RiveAnimationView(Context, null);

        if (Element.DynamicAssets?.Count > 0)
        {
            _riveAnimationView.SetAssetLoader(new AssetLoader(Context, Element.DynamicAssets));
        }

        _riveAnimationView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        _riveAnimationView.SetRiveResource(
            resourceIdentifier,
            Element.ArtboardName,
            Element.AnimationName,
            Element.StateMachineName,
            Element.AutoPlay,
            riveFit,
            riveAlignment,
            riveLoop
        );

        _stateListener = new StateListener();
        _stateListener.RivePlayerHandlerReference.SetTarget(this);
        _riveAnimationView.RegisterListener(_stateListener);

        _eventListener = new EventListener();
        _eventListener.RivePlayerHandlerReference.SetTarget(this);
        _riveAnimationView.AddEventListener(_eventListener);

        SetNativeControl(_riveAnimationView);
    }

    public static void MapArtboardName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null &&
            !string.Equals(handler._riveAnimationView.ArtboardName, view.ArtboardName, StringComparison.OrdinalIgnoreCase))
            handler._riveAnimationView.ArtboardName = view.ArtboardName;
    }

    public static void MapAnimationName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.AnimationName))
            return;

        var riveLoop = view.Loop.AsRive();
        var riveDirection = view.Direction.AsRive();

        if (view.AutoPlay)
        {
            handler._riveAnimationView?.Stop();
            handler._riveAnimationView?.Play(view.AnimationName, riveLoop, riveDirection, false, true);
        }
    }

    public static void MapStateMachineName(RivePlayerRenderer handler, RivePlayer view)
    {
        var rendererAttributes = handler._riveAnimationView?.GetRendererAttributes();

        if (rendererAttributes != null &&
            !string.Equals(rendererAttributes.StateMachineName, view.StateMachineName, StringComparison.OrdinalIgnoreCase))
            rendererAttributes.StateMachineName = view.StateMachineName;
    }

    public static void MapResourceName(RivePlayerRenderer handler, RivePlayer view)
    {
        if (string.IsNullOrWhiteSpace(view.ResourceName) ||
            string.Equals(view.ResourceName, handler._resourceName, StringComparison.OrdinalIgnoreCase))
            return;

        handler.CreatePlatformView();
    }

    public static void MapAutoPlay(RivePlayerRenderer handler, RivePlayer view)
    {
        if (handler._riveAnimationView != null && handler._riveAnimationView.Autoplay != view.AutoPlay)
            handler._riveAnimationView.Autoplay = view.AutoPlay;
    }

    public static void MapFit(RivePlayerRenderer handler, RivePlayer view)
    {
        var riveFit = view.Fit.AsRive();
        if (handler._riveAnimationView != null && handler._riveAnimationView.Fit != riveFit)
            handler._riveAnimationView.Fit = riveFit;
    }

    public static void MapAlignment(RivePlayerRenderer handler, RivePlayer view)
    {
        var riveAlignment = view.Alignment.AsRive();
        if (handler._riveAnimationView != null && handler._riveAnimationView.Alignment != riveAlignment)
            handler._riveAnimationView.Alignment = riveAlignment;
    }

    public static void MapLoop(RivePlayerRenderer handler, RivePlayer view)
    {
        var rendererAttributes = handler._riveAnimationView?.GetRendererAttributes();
        if (rendererAttributes != null)
            rendererAttributes.Loop = view.Loop.AsRive();
    }

    public static void MapDirection(RivePlayerRenderer handler, RivePlayer view)
    {
        //
    }

    public static void MapPlay(RivePlayerRenderer handler, RivePlayer view, object? args)
    {
        var riveLoop = view.Loop.AsRive();
        var riveDirection = view.Direction.AsRive();

        if (!string.IsNullOrWhiteSpace(view.AnimationName))
        {
            handler._riveAnimationView?.Play(view.AnimationName, riveLoop, riveDirection, false, true);
            return;
        }

        var stateMachineName = !string.IsNullOrWhiteSpace(view.StateMachineName)
            ? view.StateMachineName
            : handler._riveAnimationView?.Controller.ActiveArtboard?.StateMachineNames.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(stateMachineName))
        {
            handler._riveAnimationView?.Play(stateMachineName, riveLoop, riveDirection, true, true);
            return;
        }

        var firstAnimationName = handler._riveAnimationView?.Controller.ActiveArtboard?.AnimationNames.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(firstAnimationName))
        {
            handler._riveAnimationView?.Play(firstAnimationName, riveLoop, riveDirection, false, true);
        }
    }

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
                handler._riveAnimationView?.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, (float)doubleValue);
                break;
            case float floatValue:
                handler._riveAnimationView?.SetNumberState(inputArgs.StateMachineName, inputArgs.InputName, floatValue);
                break;
            case bool boolValue:
                handler._riveAnimationView?.SetBooleanState(inputArgs.StateMachineName, inputArgs.InputName, boolValue);
                break;
        }
    }

    public static void MapTriggerInput(RivePlayerRenderer handler, RivePlayer view, object? args)
    {
        if (args is StateMachineTriggerInputArgs triggerInputArgs)
        {
            handler._riveAnimationView?.FireState(triggerInputArgs.StateMachineName, triggerInputArgs.InputName);
        }
    }
}