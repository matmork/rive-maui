using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Rive.Android;
using Rive.Android.Controllers;
using Rive.Android.Core;
using View = Android.Views.View;

namespace Rive.Maui;

internal partial class RivePlayerRenderer(Context context) : ViewRenderer<RivePlayer, View>(context)
{
    public RiveAnimationView? RiveView;
    private StateListener? _listener;

    protected override void OnElementChanged(ElementChangedEventArgs<RivePlayer> e)
    {
        base.OnElementChanged(e);

        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }
    }

    // https://github.com/rive-app/rive-android/blob/master/MEMORY_MANAGEMENT.md
    protected override void OnAttachedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName) && RiveView == null)
        {
            Load();
        }

        base.OnAttachedToWindow();
    }

    protected override void OnDetachedFromWindow()
    {
        if (_listener != null)
        {
            RiveView?.UnregisterListener(_listener);
            _listener.RivePlayerRendererReference.SetTarget(null);
            _listener.Dispose();
            _listener = null;
        }

        Element?.StateMachineInputs.Dispose();
        RiveView = null;

        base.OnDetachedFromWindow();
    }

    private void Load()
    {
        var context = Platform.AppContext;
        var control = Element!;

        var identifier = context.Resources!.GetIdentifier(control.ResourceName, "drawable", context.PackageName);
        if (identifier == 0)
        {
            return;
        }

        var riveFit = control.Fit switch
        {
            RivePlayerFit.Fill => Android.Core.Fit.Fill!,
            RivePlayerFit.Contain => Android.Core.Fit.Contain!,
            RivePlayerFit.Cover => Android.Core.Fit.Cover!,
            RivePlayerFit.FitHeight => Android.Core.Fit.FitHeight!,
            RivePlayerFit.FitWidth => Android.Core.Fit.FitWidth!,
            RivePlayerFit.ScaleDown => Android.Core.Fit.ScaleDown!,
            RivePlayerFit.NoFit => Android.Core.Fit.None!,
            _ => Android.Core.Fit.Contain!
        };

        var riveAlignment = control.Alignment switch
        {
            RivePlayerAlignment.TopLeft => Android.Core.Alignment.TopLeft!,
            RivePlayerAlignment.TopCenter => Android.Core.Alignment.TopCenter!,
            RivePlayerAlignment.TopRight => Android.Core.Alignment.TopRight!,
            RivePlayerAlignment.CenterLeft => Android.Core.Alignment.CenterLeft!,
            RivePlayerAlignment.Center => Android.Core.Alignment.Center!,
            RivePlayerAlignment.CenterRight => Android.Core.Alignment.CenterRight!,
            RivePlayerAlignment.BottomLeft => Android.Core.Alignment.BottomLeft!,
            RivePlayerAlignment.BottomCenter => Android.Core.Alignment.BottomCenter!,
            RivePlayerAlignment.BottomRight => Android.Core.Alignment.BottomRight!,
            _ => Android.Core.Alignment.Center!
        };

        RiveView = new RiveAnimationView(context, null);

        if (control.OnStateMachineChangeCommand != null)
        {
            _listener = new StateListener();
            _listener.RivePlayerRendererReference.SetTarget(this);
            RiveView.RegisterListener(_listener);
        }

        RiveView.SetRiveResource(
            identifier,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.AutoPlay,
            riveFit,
            riveAlignment,
            Android.Core.Loop.Auto!
        );

        RiveView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        SetNativeControl(RiveView);
    }

    private static Android.Core.Loop GetLoop(RivePlayerLoop loop)
    {
        return loop switch
        {
            RivePlayerLoop.OneShot => Android.Core.Loop.Oneshot!,
            RivePlayerLoop.Loop => Android.Core.Loop.Loop2!,
            RivePlayerLoop.PingPong => Android.Core.Loop.Pingpong!,
            RivePlayerLoop.AutoLoop => Android.Core.Loop.Auto!,
            _ => Android.Core.Loop.Auto!
        };
    }

    private static Android.Core.Direction GetDirection(RivePlayerDirection direction)
    {
        return direction switch
        {
            RivePlayerDirection.Backwards => Android.Core.Direction.Backwards!,
            RivePlayerDirection.Forwards => Android.Core.Direction.Forwards!,
            RivePlayerDirection.AutoDirection => Android.Core.Direction.Auto!,
            _ => Android.Core.Direction.Auto!
        };
    }

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction)
    {
        if (RiveView is null) return;

        var riveLoop = GetLoop(loop);
        var riveDirection = GetDirection(direction);

        RiveView.Play(animationName, riveLoop, riveDirection, false, true);
    }

    public void Play()
    {
        var riveLoop = GetLoop(Element!.Loop);
        var riveDirection = GetDirection(Element.Direction);

        RiveView?.Play(riveLoop, riveDirection, true);
    }

    public void Pause() => RiveView?.Pause();
    public void Stop() => RiveView?.Stop();

    public void Reset() => RiveView?.Reset();

    public void SetInput(string stateMachineName, string inputName, bool value) => RiveView?.SetBooleanState(stateMachineName, inputName, value);

    public void SetInput(string stateMachineName, string inputName, float value) => RiveView?.SetNumberState(stateMachineName, inputName, value);

    public void TriggerInput(string stateMachineName, string inputName) => RiveView?.FireState(stateMachineName, inputName);
}

internal class StateListener : Java.Lang.Object, RiveFileController.IListener
{
    public WeakReference<RivePlayerRenderer?> RivePlayerRendererReference { get; set; } = new(null);

    public void NotifyAdvance(float elapsed)
    {
    }

    public void NotifyLoop(IPlayableInstance animation)
    {
    }

    public void NotifyPause(IPlayableInstance animation)
    {
    }

    public void NotifyPlay(IPlayableInstance animation)
    {
    }

    public void NotifyStateChanged(string stateMachineName, string stateName)
    {
        if (RivePlayerRendererReference.TryGetTarget(out var renderer)
            && renderer is { RiveView: not null, Element.OnStateMachineChangeCommand: not null })
        {
            var inputs = new Dictionary<string, object>();
            foreach (var stateMachine in renderer.RiveView.StateMachines)
            {
                foreach (var input in stateMachine.Inputs)
                {
                    switch (input)
                    {
                        case SMINumber smiNumber:
                            inputs.Add(smiNumber.Name, smiNumber.Value);
                            break;
                        case SMIBoolean smiBool:
                            inputs.Add(smiBool.Name, smiBool.Value);
                            break;
                    }
                }
            }

            renderer.Element.OnStateMachineChangeCommand?.Execute(new StateMachineChange(stateMachineName, stateName, inputs));
        }
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}