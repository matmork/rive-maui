using Rive.iOS;

namespace Rive.Maui;

public record CustomRiveViewProperties(
    string Resource,
    string? Artboard,
    string? Animation,
    string? StateMachine,
    bool AutoPlay,
    RiveFit Fit,
    RiveAlignment Alignment,
    RiveLoop Loop,
    RiveDirection Direction
);