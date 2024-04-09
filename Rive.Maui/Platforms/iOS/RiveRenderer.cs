using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using ObjCRuntime;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

internal partial class RiveRenderer : ViewRenderer<Rive, UIView>
{
    private CustomRiveViewModel? _riveVM;

    public override void MovedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }

        base.MovedToWindow();
    }

    private void Load()
    {
        var control = Element!;

        var riveFit = control.Fit switch
        {
            Fit.Fill => RiveFit.fill,
            Fit.Contain => RiveFit.contain,
            Fit.Cover => RiveFit.cover,
            Fit.FitHeight => RiveFit.fitHeight,
            Fit.FitWidth => RiveFit.fitWidth,
            Fit.ScaleDown => RiveFit.scaleDown,
            Fit.NoFit => RiveFit.noFit,
            _ => RiveFit.contain
        };

        var riveAlignment = control.Alignment switch
        {
            Alignment.TopLeft => RiveAlignment.topLeft,
            Alignment.TopCenter => RiveAlignment.topCenter,
            Alignment.TopRight => RiveAlignment.topRight,
            Alignment.CenterLeft => RiveAlignment.centerLeft,
            Alignment.Center => RiveAlignment.center,
            Alignment.CenterRight => RiveAlignment.centerRight,
            Alignment.BottomLeft => RiveAlignment.bottomLeft,
            Alignment.BottomCenter => RiveAlignment.bottomCenter,
            Alignment.BottomRight => RiveAlignment.bottomRight,
            _ => RiveAlignment.center
        };

        _riveVM = new CustomRiveViewModel(
            control.ResourceName!,
            ".riv",
            NSBundle.MainBundle,
            control.StateMachineName,
            riveFit,
            riveAlignment,
            control.AutoPlay,
            control.AnimationName,
            true,
            null
        );

        _riveVM.Control.SetTarget(control);

        var riveView = _riveVM.CreateRiveView;
        riveView.Frame = Element!.Bounds;

        SetNativeControl(riveView);
    }

    public void PlayAnimation(string animationName, Loop loop, Direction direction)
    {
        if (_riveVM is null) return;

        var riveLoop = loop switch
        {
            Loop.OneShot => RiveLoop.oneShot,
            Loop.Loop => RiveLoop.loop,
            Loop.PingPong => RiveLoop.pingPong,
            Loop.AutoLoop => RiveLoop.autoLoop,
            _ => RiveLoop.autoLoop
        };

        var riveDirection = direction switch
        {
            Direction.Backwards => RiveDirection.backwards,
            Direction.Forwards => RiveDirection.forwards,
            Direction.AutoDirection => RiveDirection.autoDirection,
            _ => RiveDirection.autoDirection
        };

        _riveVM.PlayWithAnimationName(animationName, riveLoop, riveDirection);
    }

    public void Pause()
    {
        _riveVM?.Pause();
    }

    public void Stop()
    {
        _riveVM?.Stop();
    }

    public void Reset()
    {
        _riveVM?.Reset();
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        _riveVM?.SetBoolInput(inputName, value);
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        _riveVM?.SetInput(inputName, value);
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        _riveVM?.TriggerInput(inputName);
    }
}

internal class CustomRiveViewModel : RiveViewModel
{
    #region Constructors

    protected CustomRiveViewModel(NSObjectFlag t) : base(t)
    {
    }

    protected internal CustomRiveViewModel(NativeHandle handle) : base(handle)
    {
    }

    public CustomRiveViewModel(RiveModel model, string? animationName, RiveFit fit, RiveAlignment alignment, bool autoPlay, string? artboardName) :
        base(model, animationName, fit, alignment, autoPlay, artboardName)
    {
    }

    public CustomRiveViewModel(string fileName, string extension, NSBundle bundle, string? stateMachineName, RiveFit fit, RiveAlignment alignment,
        bool autoPlay, string? artboardName, bool loadCdn, LoadAsset? customLoader) : base(fileName, extension, bundle, stateMachineName, fit,
        alignment, autoPlay, artboardName, loadCdn, customLoader)
    {
    }

    #endregion

    public WeakReference<Rive?> Control { get; set; } = new(null);

    public override void StateMachine(RiveStateMachineInstance stateMachine, string stateName)
    {
        if (Control.TryGetTarget(out var control))
        {
            control.OnStateMachineChangeCommand?.Execute(new StateMachineChange(stateMachine.Name, stateName));
        }
    }
}