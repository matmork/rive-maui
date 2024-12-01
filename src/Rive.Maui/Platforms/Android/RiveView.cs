using Android.Content;
using Android.Views;
using Rive.Android;
using Rive.Android.Core;

namespace Rive.Maui;

public class RiveView(Context context, RivePlayer virtualView) : ViewGroup(context)
{
    internal RiveAnimationView? AnimationView;
    internal LinearAnimationInstance? RiveAnimation;
    internal RivePlayer VirtualView = virtualView;

    internal string? ResourceName;

    //private readonly View _tempView = new(context);
    private StateListener? _stateListener;
    private EventListener? _eventListener;
    private readonly Context _context = context;

    public void CreateAnimationView()
    {
        if (string.IsNullOrWhiteSpace(VirtualView.ResourceName))
            throw new Exception("Invalid ResourceName");

        var resourceIdentifier = _context.Resources?.GetIdentifier(VirtualView.ResourceName, "drawable", _context.PackageName) ?? 0;
        if (resourceIdentifier == 0)
            throw new Exception("Resource not found");

        ResourceName = VirtualView.ResourceName;

        var riveAlignment = VirtualView.Alignment.AsRive();
        var riveFit = VirtualView.Fit.AsRive();
        var riveLoop = VirtualView.Loop.AsRive();

        var animationView = new RiveAnimationView(_context, null);

        if (VirtualView.DynamicAssets?.Count > 0)
        {
            animationView.SetAssetLoader(new AssetLoader(_context, VirtualView.DynamicAssets));
        }

        animationView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        animationView.SetRiveResource(
            resourceIdentifier,
            VirtualView.ArtboardName,
            VirtualView.AnimationName,
            VirtualView.StateMachineName,
            VirtualView.AutoPlay,
            riveFit,
            riveAlignment,
            riveLoop
        );

        _stateListener = new StateListener();
        _stateListener.RiveViewReference.SetTarget(this);
        animationView.RegisterListener(_stateListener);

        _eventListener = new EventListener();
        _eventListener.RiveViewReference.SetTarget(this);
        animationView.AddEventListener(_eventListener);

        AnimationView = animationView;
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        
    }

    protected override void OnAttachedToWindow()
    {
        if (AnimationView == null)
            CreateAnimationView();

        AddView(AnimationView);

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        // dispose and replace
        RemoveView(AnimationView);
        base.OnDetachedFromWindow();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_stateListener != null)
            {
                AnimationView?.UnregisterListener(_stateListener);
                _stateListener.RiveViewReference.SetTarget(null);
                _stateListener.Dispose();
                _stateListener = null;
            }

            if (_eventListener != null)
            {
                AnimationView?.RemoveEventListener(_eventListener);
                _eventListener.RiveViewReference.SetTarget(null);
                _eventListener.Dispose();
                _eventListener = null;
            }

            VirtualView.StateMachineInputs.Dispose();

            RiveAnimation?.Dispose();
            RiveAnimation = null;
        }

        base.Dispose(disposing);
    }
}