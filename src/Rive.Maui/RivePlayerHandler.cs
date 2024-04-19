#if IOS || MACCATALYST
using PlatformView = Rive.iOS.RiveRendererView;
#elif ANDROID
using PlatformView = Rive.Android.RiveAnimationView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#elif (NETSTANDARD || !PLATFORM) || (NET8_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using Microsoft.Maui.Handlers;

namespace Rive.Maui;

public partial class RivePlayerHandler
{
    public static IPropertyMapper<RivePlayer, RivePlayerHandler> PropertyMapper =
        new PropertyMapper<RivePlayer, RivePlayerHandler>(ViewHandler.ViewMapper)
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
            [nameof(RivePlayer.OnStateMachineChangeCommand)] = MapOnStateMachineChangeCommand,
            //[nameof(RivePlayer.StateMachineInputs)] = MapStateMachineInputs,
        };

    public static CommandMapper<RivePlayer, RivePlayerHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(RivePlayer.PlayRequested)] = MapPlay,
        [nameof(RivePlayer.PauseRequested)] = MapPause,
        [nameof(RivePlayer.StopRequested)] = MapStop,
        [nameof(RivePlayer.ResetRequested)] = MapReset,
    };

    public RivePlayerHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}