using Rive.Android.Controllers;
using Rive.Android.Core;
using Object = Java.Lang.Object;

namespace Rive.Maui;

internal class StateListener : Object, RiveFileController.IListener
{
    public WeakReference<RivePlayerRenderer?> RivePlayerHandlerReference { get; set; } = new(null);

    public void NotifyAdvance(float elapsed)
    {
        if (RivePlayerHandlerReference.TryGetTarget(out var handler) && handler is { _animation: not null, Element.PlayToPercentage: > 0 })
        {
            var percentElapsed = Math.Floor(handler._animation.Time / handler._animation.EffectiveDurationInSeconds * 100);
            if (percentElapsed >= handler.Element.PlayToPercentage)
            {
                handler._riveAnimationView?.Pause();
            }
        }
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
        if (!RivePlayerHandlerReference.TryGetTarget(out var handler)
            || handler.Element == null
            || handler._riveAnimationView == null)
            return;

        var inputs = new Dictionary<string, object>();
        foreach (var stateMachine in handler._riveAnimationView.StateMachines)
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

        var args = new StateMachineChangeArgs(stateMachineName, stateName, inputs);
        handler.Element.StateChangedManager.HandleEvent(this, args, nameof(RivePlayer.StateChanged));
        handler.Element.StateChangedCommand?.Execute(args);
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}