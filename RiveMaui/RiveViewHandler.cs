using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace RiveMaui;

public class RiveViewHandler : ContentViewHandler
{
    public void Load(string animation)
    {
#if IOS
        var riveVM = new Rive.iOS.RiveViewModel(
            animation,
            ".riv",
            Foundation.NSBundle.MainBundle,
            null,
            Rive.iOS.RiveFit.contain,
            Rive.iOS.RiveAlignment.center,
            true,
            null,
            true,
            null
        );

        var riveView = riveVM.CreateRiveView;
        riveView.Frame = PlatformView.Bounds;

        PlatformView.AddSubview(riveView);
#endif
#if ANDROID
        var context = Android.App.Application.Context;

        var identifier = context.Resources!.GetIdentifier(animation, "drawable", context.PackageName);
        if (identifier == 0)
        {
            return;
        }

        var riveView = new Rive.Android.RiveAnimationView(context, null);
        riveView.SetRiveResource(
            identifier,
            null,
            null,
            null,
            true,
            Rive.Android.Core.Fit.Contain!,
            Rive.Android.Core.Alignment.Center!,
            Rive.Android.Core.Loop.Auto!
        );

        riveView.LayoutParameters = new Android.Views.ViewGroup.LayoutParams(
            Android.Views.ViewGroup.LayoutParams.MatchParent,
            Android.Views.ViewGroup.LayoutParams.MatchParent
        );

        PlatformView.AddView(riveView);
#endif
    }
}