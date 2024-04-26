namespace Rive.Maui;

public record StateMachineTriggerInputArgs(string StateMachineName, string InputName);

public record StateMachineInputArgs(string StateMachineName, string InputName, object Value);