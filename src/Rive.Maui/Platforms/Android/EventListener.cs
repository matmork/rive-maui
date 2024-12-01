using Rive.Android.Controllers;
using Rive.Android.Core;
using Object = Java.Lang.Object;

namespace Rive.Maui;

public class EventListener : Object, RiveFileController.IRiveEventListener
{
    public WeakReference<CustomRiveView?> RiveViewReference { get; set; } = new(null);

    public void NotifyEvent(RiveEvent evt)
    {
        if (!RiveViewReference.TryGetTarget(out var handler) || !handler.VirtualView.TryGetTarget(out var virtualView))
            return;

        var type = RivePlayerEvent.GeneralEvent;
        if (evt.Type == EventType.OpenURLEvent)
        {
            type = RivePlayerEvent.OpenURLEvent;
        }

        var properties = evt.Properties
            .ToDictionary<KeyValuePair<string, Object>, string, object>(k => k.Key, k => k.Value);
        var args = new EventReceivedArgs(evt.Name, type, properties);

        virtualView.EventReceivedManager.HandleEvent(this, args, nameof(RivePlayer.EventReceived));
        virtualView.EventReceivedCommand?.Execute(args);
    }
}