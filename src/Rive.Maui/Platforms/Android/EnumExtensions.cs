using Rive.Android.Core;

namespace Rive.Maui;

public static class EnumExtensions
{
    public static Alignment AsRive(this RivePlayerAlignment alignment)
    {
        return alignment switch
        {
            RivePlayerAlignment.TopLeft => Alignment.TopLeft!,
            RivePlayerAlignment.TopCenter => Alignment.TopCenter!,
            RivePlayerAlignment.TopRight => Alignment.TopRight!,
            RivePlayerAlignment.CenterLeft => Alignment.CenterLeft!,
            RivePlayerAlignment.Center => Alignment.Center!,
            RivePlayerAlignment.CenterRight => Alignment.CenterRight!,
            RivePlayerAlignment.BottomLeft => Alignment.BottomLeft!,
            RivePlayerAlignment.BottomCenter => Alignment.BottomCenter!,
            RivePlayerAlignment.BottomRight => Alignment.BottomRight!,
            _ => Alignment.Center!
        };
    }

    public static Fit AsRive(this RivePlayerFit fit)
    {
        return fit switch
        {
            RivePlayerFit.Fill => Fit.Fill!,
            RivePlayerFit.Contain => Fit.Contain!,
            RivePlayerFit.Cover => Fit.Cover!,
            RivePlayerFit.FitHeight => Fit.FitHeight!,
            RivePlayerFit.FitWidth => Fit.FitWidth!,
            RivePlayerFit.ScaleDown => Fit.ScaleDown!,
            RivePlayerFit.NoFit => Fit.None!,
            _ => Fit.Contain!
        };
    }

    public static Loop AsRive(this RivePlayerLoop loop)
    {
        return loop switch
        {
            RivePlayerLoop.OneShot => Loop.Oneshot!,
            RivePlayerLoop.Loop => Loop.Loop2!,
            RivePlayerLoop.PingPong => Loop.Pingpong!,
            RivePlayerLoop.AutoLoop => Loop.Auto!,
            _ => Loop.Auto!
        };
    }

    public static Direction AsRive(this RivePlayerDirection direction)
    {
        return direction switch
        {
            RivePlayerDirection.Backwards => Direction.Backwards!,
            RivePlayerDirection.Forwards => Direction.Forwards!,
            RivePlayerDirection.AutoDirection => Direction.Auto!,
            _ => Direction.Auto!
        };
    }
}