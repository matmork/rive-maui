namespace Rive.Maui;

public record EventReceivedArgs(string Name, RivePlayerEvent Type, Dictionary<string, object>? Properties);