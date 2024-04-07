

namespace Rive.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseRive(this MauiAppBuilder builder)
        {
#if ANDROID
            var renderer = Rive.Android.Core.RendererType.Skia;
            Rive.Android.Core.Rive.Instance.Init(Platform.AppContext, renderer!);
#endif

            builder.ConfigureMauiHandlers(handlers =>
             {
                 handlers.AddHandler<RiveView, RiveViewRenderer>();
             });

            return builder;
        }
    }
}
