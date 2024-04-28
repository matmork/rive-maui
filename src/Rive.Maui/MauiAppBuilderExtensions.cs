namespace Rive.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseRive(this MauiAppBuilder builder)
        {
#if ANDROID
            var renderer = Android.Core.RendererType.Skia;
            Android.Core.Rive.Instance.Init(Platform.AppContext, renderer!);
#endif

            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<RivePlayer, RivePlayerRenderer>();
            });

            return builder;
        }
    }
}