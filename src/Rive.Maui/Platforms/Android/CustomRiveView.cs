using Android.Content;
using Android.Views;
using Android.Widget;
using Rive.Android;

namespace Rive.Maui;

public sealed class CustomRiveView : LinearLayout
{
    internal RiveAnimationView? AnimationView;
    internal WeakReference<RivePlayer?> VirtualView = new(null);
    internal string? ResourceName;

    private StateListener? _stateListener;
    private EventListener? _eventListener;
    private readonly Context _context;

    public CustomRiveView(Context context) : base(context)
    {
        _context = context;
        LayoutParameters = new LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent
        );
    }

    public void CreateAnimationView()
    {
        if (!VirtualView.TryGetTarget(out var virtualView) || string.IsNullOrWhiteSpace(virtualView.ResourceName))
            throw new Exception("Invalid ResourceName");

        var resourceIdentifier = _context.Resources?.GetIdentifier(virtualView.ResourceName, "drawable", _context.PackageName) ?? 0;
        if (resourceIdentifier == 0)
            throw new Exception("Resource not found");

        ResourceName = virtualView.ResourceName;

        var riveAlignment = virtualView.Alignment.AsRive();
        var riveFit = virtualView.Fit.AsRive();
        var riveLoop = virtualView.Loop.AsRive();

        var animationView = new RiveAnimationView(_context, null);

        if (virtualView.DynamicAssets?.Count > 0)
        {
            animationView.SetAssetLoader(new AssetLoader(_context, virtualView.DynamicAssets));
        }

        animationView.SetRiveResource(
            resourceIdentifier,
            virtualView.ArtboardName,
            virtualView.AnimationName,
            virtualView.StateMachineName,
            virtualView.AutoPlay,
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

        if (virtualView.TextRuns != null)
        {
            foreach (var textRun in virtualView.TextRuns)
            {
                animationView.SetTextRunValue(textRun.TextRunName, textRun.Value);
            }
        }

        AnimationView = animationView;
    }

    public void RemoveAnimationView()
    {
        if (AnimationView == null)
            return;

        RemoveView(AnimationView);

        if (_stateListener != null)
            AnimationView.UnregisterListener(_stateListener);

        if (_eventListener != null)
            AnimationView.RemoveEventListener(_eventListener);

        AnimationView.Dispose();
        AnimationView = null;
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        AnimationView?.Layout(0, 0, r - l, b - t);
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        if (AnimationView != null)
        {
            AnimationView.Measure(widthMeasureSpec, heightMeasureSpec);
            SetMeasuredDimension(AnimationView.MeasuredWidth, AnimationView.MeasuredHeight);
            return;
        }

        SetMeasuredDimension(0, 0);
    }

    protected override void OnAttachedToWindow()
    {
        if (AnimationView == null)
            CreateAnimationView();

        AddView(AnimationView, ViewGroup.LayoutParams.MatchParent);

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        RemoveAnimationView();

        base.OnDetachedFromWindow();
    }

    public new void Dispose()
    {
        RemoveAnimationView();

        if (_stateListener != null)
        {
            _stateListener.RiveViewReference.SetTarget(null);
            _stateListener.Dispose();
            _stateListener = null;
        }

        if (_eventListener != null)
        {
            _eventListener.RiveViewReference.SetTarget(null);
            _eventListener.Dispose();
            _eventListener = null;
        }

        if (VirtualView.TryGetTarget(out var control))
            control.StateMachineInputs.Dispose();

        VirtualView.SetTarget(null);

        base.Dispose();
    }
}