namespace Rive.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseRive(
            this MauiAppBuilder builder,
            RiveAndroidRendererType riveAndroidRendererType = RiveAndroidRendererType.Rive,
            RiveIOSRendererType riveIosRendererType = RiveIOSRendererType.Rive)
        {
#if ANDROID
            var renderer = riveAndroidRendererType switch
            {
                RiveAndroidRendererType.Rive => Android.Core.RendererType.Rive!,
                RiveAndroidRendererType.Skia => Android.Core.RendererType.Skia!,
                RiveAndroidRendererType.Canvas => Android.Core.RendererType.Canvas!,
            };

            Android.Core.Rive.Instance.Init(Platform.AppContext, renderer);
#endif

#if IOS
            var renderer = riveIosRendererType switch
            {
                RiveIOSRendererType.Rive => Rive.iOS.RendererType.riveRenderer,
                RiveIOSRendererType.CoreGraphics => Rive.iOS.RendererType.cgRenderer,
            };

            Rive.iOS.RenderContextManager.Shared.DefaultRenderer = renderer;
#endif

            builder.ConfigureMauiHandlers(handlers => handlers.AddHandler<RivePlayer, RivePlayerRenderer>());

            return builder;
        }
    }
}