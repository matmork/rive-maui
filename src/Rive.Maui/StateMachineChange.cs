namespace Rive.Maui;

public record StateMachineChange(string StateMachine, string StateName, Dictionary<string, object> Inputs);