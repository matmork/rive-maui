namespace Rive.Maui;

public partial class RivePlayerHandler
{
    private static readonly IPropertyMapper<RivePlayer, RivePlayerHandler> PropertyMapper =
        new PropertyMapper<RivePlayer, RivePlayerHandler>(ViewMapper)
        {
            [nameof(RivePlayer.ArtboardName)] = MapArtboardName,
            [nameof(RivePlayer.AnimationName)] = MapAnimationName,
            [nameof(RivePlayer.StateMachineName)] = MapStateMachineName,
            [nameof(RivePlayer.AutoPlay)] = MapAutoPlay,
            [nameof(RivePlayer.Fit)] = MapFit,
            [nameof(RivePlayer.Alignment)] = MapAlignment,
            [nameof(RivePlayer.Loop)] = MapLoop,
            [nameof(RivePlayer.Direction)] = MapDirection
        };

    private static readonly CommandMapper<RivePlayer, RivePlayerHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(RivePlayer.Play)] = MapPlay,
        [nameof(RivePlayer.Pause)] = MapPause,
        [nameof(RivePlayer.Stop)] = MapStop,
        [nameof(RivePlayer.Reset)] = MapReset,
        [nameof(RivePlayer.SetInput)] = MapSetInput,
        [nameof(RivePlayer.TriggerInput)] = MapTriggerInput,
    };
}