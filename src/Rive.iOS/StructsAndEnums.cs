using ObjCRuntime;

namespace Rive.iOS
{
	[Native]
	public enum RiveHitResult : long
	{
		none,
		hit,
		hitOpaque
	}

	[Native]
	public enum RendererType : long
	{
		riveRenderer,
		cgRenderer
	}

	[Native]
	public enum RiveFontStyleWeight : long
	{
		Thin = 100,
		UltraLight = 200,
		Light = 300,
		Regular = 400,
		Medium = 500,
		Semibold = 600,
		Bold = 700,
		Heavy = 800,
		Black = 900
	}

	[Native]
	public enum RiveLoop : long
	{
		oneShot,
		loop,
		pingPong,
		autoLoop
	}

	[Native]
	public enum RiveDirection : long
	{
		backwards,
		forwards,
		autoDirection
	}

	[Native]
	public enum RiveFit : long
	{
		fill,
		contain,
		cover,
		fitHeight,
		fitWidth,
		scaleDown,
		noFit,
		layout
	}

	[Native]
	public enum RiveAlignment : long
	{
		topLeft,
		topCenter,
		topRight,
		centerLeft,
		center,
		centerRight,
		bottomLeft,
		bottomCenter,
		bottomRight
	}

	[Native]
	public enum RiveErrorCode : long
	{
		NoArtboardsFound = 100,
		NoArtboardFound = 101,
		NoAnimations = 200,
		NoAnimationFound = 201,
		NoStateMachines = 300,
		NoStateMachineFound = 301,
		NoStateMachineInputFound = 400,
		UnknownStateMachineInput = 401,
		NoStateChangeFound = 402,
		UnsupportedVersion = 500,
		MalformedFile = 600,
		UnknownError = 700
	}

	[Native]
	public enum RiveFallbackFontDescriptorDesign : long
	{
		Default = 0,
		Rounded = 1,
		Monospaced = 2,
		Serif = 3
	}

	[Native]
	public enum RiveFallbackFontDescriptorWeight : long
	{
		UltraLight = 0,
		Thin = 1,
		Light = 2,
		Regular = 3,
		Medium = 4,
		Semibold = 5,
		Bold = 6,
		Heavy = 7,
		Black = 8
	}

	[Native]
	public enum RiveFallbackFontDescriptorWidth : long
	{
		Compressed = 0,
		Condensed = 1,
		Standard = 2,
		Expanded = 3
	}

	[Native]
	public enum RiveTouchEvent : long
	{
		Began = 0,
		Moved = 1,
		Ended = 2,
		Cancelled = 3
	}

	[Native]
	public enum StateMachineInputType : long
	{
		Trigger = 0,
		Number = 1,
		Boolean = 2
	}
}
