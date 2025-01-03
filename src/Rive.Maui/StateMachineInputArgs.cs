namespace Rive.Maui;

public record StateMachineTriggerInputArgs(string? StateMachineName, string InputName, string? Path);

public record StateMachineInputArgs(string? StateMachineName, string InputName, object Value, string? Path);