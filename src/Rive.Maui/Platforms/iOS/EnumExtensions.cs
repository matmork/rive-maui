using Rive.iOS;

namespace Rive.Maui;

public static class EnumExtensions
{
    public static RiveFit AsRive(this RivePlayerFit fit)
    {
        return fit switch
        {
            RivePlayerFit.Fill => RiveFit.fill,
            RivePlayerFit.Contain => RiveFit.contain,
            RivePlayerFit.Cover => RiveFit.cover,
            RivePlayerFit.FitHeight => RiveFit.fitHeight,
            RivePlayerFit.FitWidth => RiveFit.fitWidth,
            RivePlayerFit.ScaleDown => RiveFit.scaleDown,
            RivePlayerFit.NoFit => RiveFit.noFit,
            _ => RiveFit.contain
        };
    }

    public static RiveAlignment AsRive(this RivePlayerAlignment alignment)
    {
        return alignment switch
        {
            RivePlayerAlignment.TopLeft => RiveAlignment.topLeft,
            RivePlayerAlignment.TopCenter => RiveAlignment.topCenter,
            RivePlayerAlignment.TopRight => RiveAlignment.topRight,
            RivePlayerAlignment.CenterLeft => RiveAlignment.centerLeft,
            RivePlayerAlignment.Center => RiveAlignment.center,
            RivePlayerAlignment.CenterRight => RiveAlignment.centerRight,
            RivePlayerAlignment.BottomLeft => RiveAlignment.bottomLeft,
            RivePlayerAlignment.BottomCenter => RiveAlignment.bottomCenter,
            RivePlayerAlignment.BottomRight => RiveAlignment.bottomRight,
            _ => RiveAlignment.center
        };
    }

    public static RiveLoop AsRive(this RivePlayerLoop loop)
    {
        return loop switch
        {
            RivePlayerLoop.OneShot => RiveLoop.oneShot,
            RivePlayerLoop.Loop => RiveLoop.loop,
            RivePlayerLoop.PingPong => RiveLoop.pingPong,
            RivePlayerLoop.AutoLoop => RiveLoop.autoLoop,
            _ => RiveLoop.autoLoop
        };
    }

    public static RiveDirection AsRive(this RivePlayerDirection direction)
    {
        return direction switch
        {
            RivePlayerDirection.Backwards => RiveDirection.backwards,
            RivePlayerDirection.Forwards => RiveDirection.forwards,
            RivePlayerDirection.AutoDirection => RiveDirection.autoDirection,
            _ => RiveDirection.autoDirection
        };
    }
}