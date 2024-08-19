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
		noFit
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
