using System.Diagnostics.CodeAnalysis;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
internal sealed class CustomRiveView : RiveRendererView
{
    public WeakReference<RivePlayer?> Control { get; set; } = new(null);

    private RiveFile? _riveFile;
    private RiveArtboard? _artboard;
    private RiveLinearAnimationInstance? _animation;
    private RiveStateMachineInstance? _stateMachine;
    private CADisplayLink? _displayLink;
    private readonly CustomRiveViewProperties _properties;
    private double? _lastTime;

    public CustomRiveView(CustomRiveViewProperties properties)
    {
        _properties = properties;
        Opaque = false;

        var resourceUrl = NSBundle.MainBundle.GetUrlForResource(_properties.Resource, ".riv");
        var resourceData = NSData.FromUrl(resourceUrl);
        _riveFile = new RiveFile(resourceData, true, out _);

        SetAnimation(_properties.AutoPlay, _properties.Artboard, _properties.Animation, _properties.StateMachine);
    }

    private void SetAnimation(bool autoplay, string? artboard, string? animation, string? stateMachine)
    {
        _artboard = !string.IsNullOrWhiteSpace(artboard)
            ? _riveFile?.ArtboardFromName(artboard, out _)
            : _riveFile?.Artboard(out _);

        if (!string.IsNullOrWhiteSpace(animation))
        {
            _animation = _artboard?.AnimationFromName(animation, out _);
            _animation?.Loop((int)_properties.Loop);
            _animation?.Direction((int)_properties.Direction);
        }
        else
        {
            _stateMachine = !string.IsNullOrWhiteSpace(stateMachine)
                ? _artboard?.StateMachineFromName(stateMachine, out _)
                : _artboard?.StateMachineFromIndex(0, out _);
        }

        if (autoplay)
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

        if (_stateMachine != null)
        {
            if (Control.TryGetTarget(out var control) && control.OnStateMachineChangeCommand != null)
            {
                var inputs = new Dictionary<string, object>();
                for (var i = 0; i < _stateMachine.InputCount; i++)
                {
                    var input = _stateMachine.InputFromIndex(i, out _);
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

                for (var i = 0; i < _stateMachine.StateChangedCount; i++)
                {
                    var stateChanged = _stateMachine.StateChangedFromIndex(i, out _);
                    if (stateChanged != null)
                    {
                        control.OnStateMachineChangeCommand.Execute(new StateMachineChange(_stateMachine.Name, stateChanged.Name, inputs));
                    }
                }
            }

            _stateMachine.AdvanceBy(elapsedTime);
        }
        else if (_animation != null)
        {
            _animation.AdvanceBy(elapsedTime);
        }

        MainThread.BeginInvokeOnMainThread(SetNeedsDisplay);

        _lastTime = timestamp;
    }

    private void HandleTouch(NSSet touches, Action<CGPoint> action)
    {
        var first = touches.FirstOrDefault();
        if (_stateMachine == null || _artboard == null || first is not UITouch touch)
            return;

        var location = touch.LocationInView(this);
        var artboardLocation = ArtboardLocationFromTouchLocation(location, _artboard.Bounds, _properties.Fit, _properties.Alignment);

        action(artboardLocation);
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _stateMachine!.TouchBeganAtLocation(location));

    public override void TouchesMoved(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _stateMachine!.TouchMovedAtLocation(location));

    public override void TouchesEnded(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _stateMachine!.TouchEndedAtLocation(location));

    public override void TouchesCancelled(NSSet touches, UIEvent? evt)
        => HandleTouch(touches, location => _stateMachine!.TouchCancelledAtLocation(location));

    public override void DrawRive(CGRect rect, CGSize size)
    {
        if (_artboard == null) return;

        var newFrame = new CGRect(rect.Location, size);
        AlignWithRect(newFrame, _artboard.Bounds, _properties.Alignment, _properties.Fit);
        DrawWithArtboard(_artboard);
    }

    public void PlayAnimation(string animationName, RivePlayerLoop loop, RivePlayerDirection direction)
    {
        if (_artboard?.AnimationFromName(animationName, out var error) is { } animation && error == null)
        {
            animation.Loop((int)_properties.Loop);
            animation.Direction((int)_properties.Direction);

            if (animation.HasEnded())
            {
                animation.Time = 0;
            }

            _animation?.Dispose();
            _animation = null;
            _animation = animation;
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
        SetAnimation(_properties.AutoPlay, _properties.Artboard, _properties.Animation, _properties.StateMachine);
    }

    public void SetInput(string stateMachineName, string inputName, bool value)
    {
        if (_stateMachine?.GetBool(inputName) is { } riveBool)
        {
            riveBool.Value = value;
        }
    }

    public void SetInput(string stateMachineName, string inputName, float value)
    {
        if (_stateMachine?.GetNumber(inputName) is { } riveNumber)
        {
            riveNumber.Value = value;
        }
    }

    public void TriggerInput(string stateMachineName, string inputName)
    {
        _stateMachine?.GetTrigger(inputName)?.Fire();
    }

    private void ResetProperties(bool disposeFile = true)
    {
        _displayLink?.RemoveFromRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        _displayLink?.Invalidate();
        _displayLink?.Dispose();
        _displayLink = null;

        if (disposeFile)
        {
            _riveFile?.Dispose();
            _riveFile = null;
        }

        _artboard?.Dispose();
        _artboard = null;

        _animation?.Dispose();
        _animation = null;

        _stateMachine?.Dispose();
        _stateMachine = null;
    }

    public new void Dispose()
    {
        ResetProperties();
        Control.SetTarget(null);
        base.Dispose();
    }
}