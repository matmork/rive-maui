using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Rive.iOS;
using UIKit;

namespace Rive.Maui;

public partial class RiveViewRenderer : ViewRenderer<RiveView, UIView>
{
    public void Load(string animation)
    {
        var riveVM = new RiveViewModel(
            animation,
            ".riv",
            NSBundle.MainBundle,
            null,
            RiveFit.contain,
            RiveAlignment.center,
            true,
            null,
            true,
            null
        );

        var riveView = riveVM.CreateRiveView;
        riveView.Frame = Element!.Bounds;

        SetNativeControl(riveView);
    }
}
