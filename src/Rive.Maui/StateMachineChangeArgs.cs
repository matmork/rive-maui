namespace Rive.Maui;

public record StateMachineChangeArgs(string StateMachine, string StateName, Dictionary<string, object> Inputs);