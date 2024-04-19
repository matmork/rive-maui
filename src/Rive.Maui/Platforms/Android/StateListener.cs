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
        if (RivePlayerHandlerReference.TryGetTarget(out var handler)
            && handler is { PlatformView: not null, VirtualView.OnStateMachineChangeCommand: not null })
        {
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

            handler.VirtualView.OnStateMachineChangeCommand?.Execute(new StateMachineChange(stateMachineName, stateName, inputs));
        }
    }

    public void NotifyStop(IPlayableInstance animation)
    {
    }
}