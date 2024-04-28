using Microsoft.Maui.Controls.Handlers.Compatibility;

namespace Rive.Maui;

public interface IRivePlayerRenderer
{
    static abstract void MapArtboardName(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapAnimationProperties(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapStateMachineName(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapResourceName(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapAutoPlay(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapFit(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapAlignment(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapStateChangedCommand(RivePlayerRenderer handler, RivePlayer view);
    static abstract void MapPlay(RivePlayerRenderer handler, RivePlayer view, object? args);
    static abstract void MapPause(RivePlayerRenderer handler, RivePlayer view, object? args);
    static abstract void MapStop(RivePlayerRenderer handler, RivePlayer view, object? args);
    static abstract void MapReset(RivePlayerRenderer handler, RivePlayer view, object? args);
    static abstract void MapSetInput(RivePlayerRenderer handler, RivePlayer view, object? args);
    static abstract void MapTriggerInput(RivePlayerRenderer handler, RivePlayer view, object? args);
}

public partial class RivePlayerRenderer
{
    public static readonly IPropertyMapper<RivePlayer, RivePlayerRenderer> PropertyMapper =
        new PropertyMapper<RivePlayer, RivePlayerRenderer>(ViewRenderer.VisualElementRendererMapper)
        {
            [nameof(RivePlayer.ArtboardName)] = MapArtboardName,
            [nameof(RivePlayer.AnimationName)] = MapAnimationProperties,
            [nameof(RivePlayer.StateMachineName)] = MapStateMachineName,
            [nameof(RivePlayer.ResourceName)] = MapResourceName,
            [nameof(RivePlayer.AutoPlay)] = MapAutoPlay,
            [nameof(RivePlayer.Fit)] = MapFit,
            [nameof(RivePlayer.Alignment)] = MapAlignment,
            [nameof(RivePlayer.Loop)] = MapAnimationProperties,
            [nameof(RivePlayer.Direction)] = MapAnimationProperties,
            [nameof(RivePlayer.StateChangedCommand)] = MapStateChangedCommand
        };

    public static readonly CommandMapper<RivePlayer, RivePlayerRenderer> CommandMapper = new(ViewRenderer.VisualElementRendererCommandMapper)
    {
        [nameof(RivePlayer.Play)] = MapPlay,
        [nameof(RivePlayer.Pause)] = MapPause,
        [nameof(RivePlayer.Stop)] = MapStop,
        [nameof(RivePlayer.Reset)] = MapReset,
        [nameof(RivePlayer.SetInput)] = MapSetInput,
        [nameof(RivePlayer.TriggerInput)] = MapTriggerInput,
    };
}