using Android.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Rive.Android;
using View = Android.Views.View;

namespace Rive.Maui;

internal partial class RiveRenderer(Context context) : ViewRenderer<Rive, View>(context)
{
    protected override void OnAttachedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }

        base.OnAttachedToWindow();
    }

    private void Load()
    {
        var context = Platform.AppContext;
        var control = Element!;

        var identifier = context.Resources!.GetIdentifier(control.ResourceName, "drawable", context.PackageName);
        if (identifier == 0)
        {
            return;
        }

        var riveView = new RiveAnimationView(context, null);
        riveView.SetRiveResource(
            identifier,
            control.ArtboardName,
            control.AnimationName,
            control.StateMachineName,
            control.Autoplay,
            Android.Core.Fit.Contain!,
            Android.Core.Alignment.Center!,
            Android.Core.Loop.Auto!
        );

        riveView.LayoutParameters = new LayoutParams(
            LayoutParams.MatchParent,
            LayoutParams.MatchParent
        );

        SetNativeControl(riveView);
    }
}