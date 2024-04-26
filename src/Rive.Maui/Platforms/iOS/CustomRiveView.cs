using System.Diagnostics.CodeAnalysis;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public sealed class CustomRiveView : RiveRendererView
{
    private RiveFile? _riveFile;
    private RiveArtboard? _riveArtboard;
    private RiveLinearAnimationInstance? _riveAnimation;
    private RiveStateMachineInstance? _riveStateMachine;
    private CADisplayLink? _displayLink;
    private double? _lastTime;
    private string? _resourceName;

    public WeakReference<RivePlayer?> Control { get; set; } = new(null);
    public string? ArtboardName { get; set; }
    public string? StateMachineName { get; set; }
    public string? AnimationName { get; set; }
    public bool AutoPlay { get; set; }
    public RiveFit Fit { get; set; }
    public RiveAlignment Alignment { get; set; }
    public RiveLoop Loop { get; set; }
    public RiveDirection Direction { get; set; }

    public CustomRiveView()
    {
        Opaque = false;
    }

    public void SetRiveResource(string? resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName) ||
            string.Equals(resourceName, _resourceName, StringComparison.OrdinalIgnoreCase))
            return;

        if (_resourceName != null)
            ResetProperties();

        _resourceName = resourceName;

        var resourceUrl = NSBundle.MainBundle.GetUrlForResource(resourceName, ".riv");
        if (resourceUrl == null)
            return;

        var resourceData = NSData.FromUrl(resourceUrl);
        if (resourceData == null)
            return;

        _riveFile = new RiveFile(resourceData, true, out _);

        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        _riveArtboard = !string.IsNullOrWhiteSpace(ArtboardName)
            ? _riveFile?.ArtboardFromName(ArtboardName, out _)
            : _riveFile?.Artboard(out _);

        if (!string.IsNullOrWhiteSpace(AnimationName) || _riveArtboard?.StateMachineCount == 0)
        {
            _riveAnimation = !string.IsNullOrWhiteSpace(AnimationName)
                ? _riveArtboard?.AnimationFromName(AnimationName, out _)
                : _riveArtboard?.AnimationFromIndex(0, out _);

            _riveAnimation?.Loop((int)Loop);
            _riveAnimation?.Direction((int)Direction);
        }
        else
        {
            _riveStateMachine = !string.IsNullOrWhiteSpace(StateMachineName)
                ? _riveArtboard?.StateMachineFromName(StateMachineName, out _)
                : _riveArtboard?.StateMachineFromIndex(0, out _);
        }

        if (AutoPlay)
        {
            CreateDisplayLink();
        }
        else
        {
            Tick();
            _lastTime = null;
        }
    }

    private void CreateDisplayLink()
    {
        if (_displayLink == null)
        {
            _displayLink = CADisplayLink.Create(Tick);
            _displayLink.PreferredFrameRateRange = CAFrameRateRange.Create(30, 60, 60);
            _displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        }
    }

    private void Tick()
    {
        var timestamp = _displayLink?.TargetTimestamp ?? new NSDate().SecondsSince1970;
        _lastTime ??= timestamp;

        var elapsedTime = timestamp - _lastTime.Value;

        if (_riveStateMachine != null)
        {
            if (Control.TryGetTarget(out var control))
            {
                var inputs = new Dictionary<string, object>();
                for (var i = 0; i < _riveStateMachine.InputCount; i++)
                {
                    var input = _riveStateMachine.InputFromIndex(i, out _);
                    switch (input)
                    {
                        case RiveSMINumber smiNumber:
                            inputs.Add(smiNumber.Name, smiNumber.Value);
                            break;
                        case RiveSMIBool smiBool:
                            inputs.Add(smiBool.Name, smiBool.Value);
                            break;
                    }
                }

                for (var i = 0; i < _riveStateMachine.StateChangedCount; i++)
                {
                    var stateChanged = _riveStateMachine.StateChangedFromIndex(i, out _);
                    if (stateChanged != null)
                    {
                        var args = new StateMachineChangeArgs(_riveStateMachine.Name, stateChanged.Name, inputs);
                        control.StateChangedEventManager.HandleEvent(this, args, nameof(RivePlayer.StateChanged));
                        control.StateChangedCommand?.Execute(args);
                    }
                }
            }

            _riveStateMachine.AdvanceBy(elapsedTime);
        }
        else
        {
            _riveAnimation?.AdvanceBy(elapsedTime);
        }

        MainThread.BeginInvokeOnMainThread(SetNeedsDisplay);

        _lastTime = timestamp;
    }

    private void HandleTouch(NSSet touches, Action<CGPoint> action)
    {
        var first = touches.FirstOrDefault();
        if (_riveStateMachine == null || _riveArtboard == null || first is not UITouch touch)
            return;

        var location = touch.LocationInView(this);
        var artboardLocation = ArtboardLocationFromTouchLocation(location, _riveArtboard.Bounds, Fit, Alignment);

        action(artboardLocation);
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _riveStateMachine!.TouchBeganAtLocation(location));

    public override void TouchesMoved(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _riveStateMachine!.TouchMovedAtLocation(location));

    public override void TouchesEnded(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _riveStateMachine!.TouchEndedAtLocation(location));

    public override void TouchesCancelled(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _riveStateMachine!.TouchCancelledAtLocation(location));

    public override void DrawRive(CGRect rect, CGSize size)
    {
        if (_riveArtboard == null) return;

        var newFrame = new CGRect(rect.Location, size);
        AlignWithRect(newFrame, _riveArtboard.Bounds, Alignment, Fit);
        DrawWithArtboard(_riveArtboard);
    }

    public void PlayAnimation(string animationName)
    {
        if (string.Equals(animationName, AnimationName, StringComparison.OrdinalIgnoreCase))
            return;

        if (_riveArtboard?.AnimationFromName(animationName, out var error) is { } animation && error == null)
        {
            animation.Loop((int)Loop);
            animation.Direction((int)Direction);

            if (animation.HasEnded())
            {
                animation.Time = 0;
            }

            _riveAnimation?.Dispose();
            _riveAnimation = null;
            _riveAnimation = animation;
        }
    }

    public void Play()
    {
        if (_displayLink == null)
        {
            CreateDisplayLink();
        }
        else
        {
            _displayLink.Paused = false;
            _lastTime = null;
        }
    }

    public void Pause()
    {
        if (_displayLink != null)
        {
            _displayLink.Paused = true;
        }
    }

    public void Stop()
    {
        Pause();
    }

    public void Reset()
    {
        ResetProperties(false);
        UpdateAnimation();
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        if (_riveStateMachine?.GetBool(inputName) is { } riveBool)
        {
            riveBool.Value = value;
        }
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        if (_riveStateMachine?.GetNumber(inputName) is { } riveNumber)
        {
            riveNumber.Value = value;
        }
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        _riveStateMachine?.GetTrigger(inputName)?.Fire();
    }

    public void ResetProperties(bool disposeFile = true)
    {
        _resourceName = null;
        _displayLink?.RemoveFromRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        _displayLink?.Invalidate();
        _displayLink?.Dispose();
        _displayLink = null;

        if (disposeFile)
        {
            _riveFile?.Dispose();
            _riveFile = null;
        }

        _riveArtboard?.Dispose();
        _riveArtboard = null;

        _riveAnimation?.Dispose();
        _riveAnimation = null;

        _riveStateMachine?.Dispose();
        _riveStateMachine = null;
    }

    public new void Dispose()
    {
        ResetProperties();

        if (Control.TryGetTarget(out var control))
            control.StateMachineInputs.Dispose();

        Control.SetTarget(null);

        base.Dispose();
    }
}