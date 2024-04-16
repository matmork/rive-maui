using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
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
            control.ResourceName!,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.Fit,
            control.Alignment,
            control.Loop,
            control.Direction
        );

        _view.Control.SetTarget(control);

        _view.Frame = control.Bounds;
        SetNativeControl(_view);
    }

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction) => _view?.PlayAnimation(animationName, loop, direction);
    public void Play() => _view?.Play();

    public void Pause() => _view?.Pause();

    public void Stop() => _view?.Stop();

    public void Reset() => _view?.Reset();

    public void SetInput(string stateMachineName, string inputName, bool value) => _view?.SetInput(stateMachineName, inputName, value);

    public void SetInput(string stateMachineName, string inputName, float value) => _view?.SetInput(stateMachineName, inputName, value);

    public void TriggerInput(string stateMachineName, string inputName) => _view?.TriggerInput(stateMachineName, inputName);
}