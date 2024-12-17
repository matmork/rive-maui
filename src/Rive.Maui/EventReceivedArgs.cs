namespace Rive.Maui;

public record StateMachineEventReceivedArgs(string Name, RivePlayerEvent Type, Dictionary<string, object>? Properties);