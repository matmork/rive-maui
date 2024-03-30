using Android.App;
using Android.Runtime;

namespace RiveMaui;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();

        var renderer = Rive.Android.Core.RendererType.Rive;
        Rive.Android.Core.Rive.Instance.Init(Context, renderer!);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}