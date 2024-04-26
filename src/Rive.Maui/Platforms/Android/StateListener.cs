using Rive.Android.Controllers;
using Rive.Android.Core;

namespace Rive.Maui;

internal class StateListener : Java.Lang.Object, RiveFileController.IListener
{
    public WeakReference<RivePlayerHandler?> RivePlayerHandlerReference { get; set; } = new(null);

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
        if (!RivePlayerHandlerReference.TryGetTarget(out var handler))
            return;

        var inputs = new Dictionary<string, object>();
        foreach (var stateMachine in handler.PlatformView.StateMachines)
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
        handler.VirtualView.StateChangedEventManager.HandleEvent(this, args, nameof(RivePlayer.StateChanged));
        handler.VirtualView.StateChangedCommand?.Execute(args);
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}