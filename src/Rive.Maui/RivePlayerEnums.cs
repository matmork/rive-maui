namespace Rive.Maui;

public enum RivePlayerLoop
{
    OneShot,
    Loop,
    PingPong,
    AutoLoop
}

public enum RivePlayerDirection
{
    Backwards,
    Forwards,
    AutoDirection
}

public enum RivePlayerFit
{
    Fill,
    Contain,
    Cover,
    FitHeight,
    FitWidth,
    ScaleDown,
    NoFit
}

public enum RivePlayerAlignment
{
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    Center,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}

public enum RiveAndroidRendererType
{
    Rive,
    Skia,
    Canvas
}

public enum RiveIOSRendererType
{
    Rive,
    CoreGraphics
}

public enum RivePlayerEvent
{
    GeneralEvent,
    OpenURLEvent
}