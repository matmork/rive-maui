namespace Rive.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseRive(this MauiAppBuilder builder, RiveRendererType rendererType = RiveRendererType.Skia)
        {
#if ANDROID
            var renderer = rendererType switch
            {
                RiveRendererType.Rive => Android.Core.RendererType.Rive!,
                RiveRendererType.Skia => Android.Core.RendererType.Skia!
            };

            Android.Core.Rive.Instance.Init(Platform.AppContext, renderer);
#endif

#if IOS
            // At this time, the Rive Renderer is not supported in simulator environments, default is Skia
            // See https://help.rive.app/runtimes/renderer
            if (rendererType == RiveRendererType.Rive && DeviceInfo.DeviceType == DeviceType.Physical)
                Rive.iOS.RenderContextManager.Shared.DefaultRenderer = Rive.iOS.RendererType.riveRenderer;
#endif

            builder.ConfigureMauiHandlers(handlers => handlers.AddHandler<RivePlayer, RivePlayerRenderer>());

            return builder;
        }
    }
}