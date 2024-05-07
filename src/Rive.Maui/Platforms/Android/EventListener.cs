using Rive.Android.Controllers;
using Rive.Android.Core;
using Object = Java.Lang.Object;

namespace Rive.Maui;

public class EventListener : Object, RiveFileController.IRiveEventListener
{
    public WeakReference<RivePlayerRenderer?> RivePlayerHandlerReference { get; set; } = new(null);

    public void NotifyEvent(RiveEvent evt)
    {
        if (!RivePlayerHandlerReference.TryGetTarget(out var handler)
            || handler.Element == null
            || handler._riveAnimationView == null)
            return;

        var type = RivePlayerEvent.GeneralEvent;
        if (evt.Type == EventType.OpenURLEvent)
        {
            type = RivePlayerEvent.OpenURLEvent;
        }

        var properties = evt.Properties
            .ToDictionary<KeyValuePair<string, Object>, string, object>(k => k.Key, k => k.Value);
        var args = new EventReceivedArgs(evt.Name, type, properties);
        handler.Element.EventReceivedManager.HandleEvent(this, args, nameof(RivePlayer.EventReceived));
        handler.Element.EventReceivedCommand?.Execute(args);
    }
}