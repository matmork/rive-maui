using Rive.Android.Controllers;
using Rive.Android.Core;
using Object = Java.Lang.Object;

namespace Rive.Maui;

internal class StateListener : Object, RiveFileController.IListener
{
    public WeakReference<RiveView?> RiveViewReference { get; set; } = new(null);

    public void NotifyAdvance(float elapsed)
    {
        if (RiveViewReference.TryGetTarget(out var view) && view is { RiveAnimation: not null, VirtualView.PlayToPercentage: > 0 })
        {
            var percentElapsed = Math.Floor(view.RiveAnimation.Time / view.RiveAnimation.EffectiveDurationInSeconds * 100);
            if (percentElapsed >= view.VirtualView.PlayToPercentage)
            {
                view.AnimationView?.Pause();
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
        if (!RiveViewReference.TryGetTarget(out var handler) || handler.AnimationView == null)
            return;

        var inputs = new Dictionary<string, object>();
        foreach (var stateMachine in handler.AnimationView.StateMachines)
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
        handler.VirtualView.StateChangedManager.HandleEvent(this, args, nameof(RivePlayer.StateChanged));
        handler.VirtualView.StateChangedCommand?.Execute(args);
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}