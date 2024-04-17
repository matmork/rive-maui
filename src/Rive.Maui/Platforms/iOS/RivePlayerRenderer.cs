using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

internal partial class RivePlayerRenderer : ViewRenderer<RivePlayer, UIView>
{
    private CustomRiveView? _view;

    protected override void OnElementChanged(ElementChangedEventArgs<RivePlayer> e)
    {
        base.OnElementChanged(e);

        if (e.OldElement != null)
        {
            e.OldElement?.StateMachineInputs.Dispose();
            _view?.Dispose();
            _view = null;
        }

        if (e.NewElement != null && !string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }
    }

    private void Load()
    {
        var control = Element!;

        _view = new CustomRiveView(
            new CustomRiveViewProperties(
                control.ResourceName!,
                control.ArtboardName,
                control.AnimationName,
                control.StateMachineName,
                control.AutoPlay,
                GetFit(control.Fit),
                GetAlignment(control.Alignment),
                GetLoop(control.Loop),
                getDirection(control.Direction)
            )
        );

        _view.Control.SetTarget(control);

        _view.Frame = control.Bounds;
        SetNativeControl(_view);
    }

    private static RiveFit GetFit(RivePlayerFit fit)
    {
        return fit switch
        {
            RivePlayerFit.Fill => RiveFit.fill,
            RivePlayerFit.Contain => RiveFit.contain,
            RivePlayerFit.Cover => RiveFit.cover,
            RivePlayerFit.FitHeight => RiveFit.fitHeight,
            RivePlayerFit.FitWidth => RiveFit.fitWidth,
            RivePlayerFit.ScaleDown => RiveFit.scaleDown,
            RivePlayerFit.NoFit => RiveFit.noFit,
            _ => RiveFit.contain
        };
    }

    private static RiveAlignment GetAlignment(RivePlayerAlignment alignment)
    {
        return alignment switch
        {
            RivePlayerAlignment.TopLeft => RiveAlignment.topLeft,
            RivePlayerAlignment.TopCenter => RiveAlignment.topCenter,
            RivePlayerAlignment.TopRight => RiveAlignment.topRight,
            RivePlayerAlignment.CenterLeft => RiveAlignment.centerLeft,
            RivePlayerAlignment.Center => RiveAlignment.center,
            RivePlayerAlignment.CenterRight => RiveAlignment.centerRight,
            RivePlayerAlignment.BottomLeft => RiveAlignment.bottomLeft,
            RivePlayerAlignment.BottomCenter => RiveAlignment.bottomCenter,
            RivePlayerAlignment.BottomRight => RiveAlignment.bottomRight,
            _ => RiveAlignment.center
        };
    }

    private static RiveLoop GetLoop(RivePlayerLoop loop)
    {
        return loop switch
        {
            RivePlayerLoop.OneShot => RiveLoop.oneShot,
            RivePlayerLoop.Loop => RiveLoop.loop,
            RivePlayerLoop.PingPong => RiveLoop.pingPong,
            RivePlayerLoop.AutoLoop => RiveLoop.autoLoop,
            _ => RiveLoop.autoLoop
        };
    }

    private static RiveDirection getDirection(RivePlayerDirection direction)
    {
        return direction switch
        {
            RivePlayerDirection.Backwards => RiveDirection.backwards,
            RivePlayerDirection.Forwards => RiveDirection.forwards,
            RivePlayerDirection.AutoDirection => RiveDirection.autoDirection,
            _ => RiveDirection.autoDirection
        };
    }

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction)
        => _view?.PlayAnimation(animationName, loop, direction);

    public void Play() => _view?.Play();

    public void Pause() => _view?.Pause();

    public void Stop() => _view?.Stop();

    public void Reset() => _view?.Reset();

    public void SetInput(string stateMachineName, string inputName, bool value) => _view?.SetInput(stateMachineName, inputName, value);

    public void SetInput(string stateMachineName, string inputName, float value) => _view?.SetInput(stateMachineName, inputName, value);

    public void TriggerInput(string stateMachineName, string inputName) => _view?.TriggerInput(stateMachineName, inputName);
}