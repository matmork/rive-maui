using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

internal partial class RiveRenderer : ViewRenderer<Rive, UIView>
{
    public override void MovedToWindow()
    {
        if (!string.IsNullOrWhiteSpace(Element?.ResourceName))
        {
            Load();
        }

        base.MovedToWindow();
    }

    private void Load()
    {
        var control = Element!;

        var riveVM = new RiveViewModel(
            control.ResourceName,
            ".riv",
            NSBundle.MainBundle,
            control.StateMachineName,
            RiveFit.contain,
            RiveAlignment.center,
            control.Autoplay,
            control.AnimationName,
            true,
            null
        );

        var riveView = riveVM.CreateRiveView;
        riveView.Frame = Element!.Bounds;

        SetNativeControl(riveView);
    }
}