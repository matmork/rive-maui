namespace Rive.Maui;

public partial class RivePlayerHandler
{
    public static readonly IPropertyMapper<RivePlayer, RivePlayerHandler> PropertyMapper =
        new PropertyMapper<RivePlayer, RivePlayerHandler>(ViewMapper)
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

    public static readonly CommandMapper<RivePlayer, RivePlayerHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(RivePlayer.Play)] = MapPlay,
        [nameof(RivePlayer.Pause)] = MapPause,
        [nameof(RivePlayer.Stop)] = MapStop,
        [nameof(RivePlayer.Reset)] = MapReset,
        [nameof(RivePlayer.SetInput)] = MapSetInput,
        [nameof(RivePlayer.TriggerInput)] = MapTriggerInput,
    };
}