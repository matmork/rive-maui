#if ANDROID
using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Rive.Android;
using View = Android.Views.View;

namespace RiveMaui;

public partial class RiveViewRenderer : ViewRenderer<RiveView, View>
{
    public RiveViewRenderer(Context context) : base(context)
    {
    }

    public void Load(string animation)
    {
        var context = Android.App.Application.Context;

        var identifier = context.Resources!.GetIdentifier(animation, "drawable", context.PackageName);
        if (identifier == 0)
        {
            return;
        }

        var riveView = new RiveAnimationView(context, null);
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

        riveView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        SetNativeControl(riveView);
    }
}

#endif