using System.Collections.Generic;
using Godot;

namespace Zoink.scenes.Components.StateMachine;

public partial class StateMachine : Node
{
	private readonly Dictionary<string, State> _states = new();
	private State _currentState;

	[Export]
	public State InitialState { get; set; }

	public override void _Ready()
	{
		foreach (var node in GetChildren())
		{
			if (node is State state)
			{
				_states.Add(node.Name, state);
				state.OnTransition += TransitionToNewState;
			}
		}

		if (InitialState != null)
		{
			_currentState = InitialState;
			InitialState.Enter();
		}
	}

	public override void _Process(double delta)
	{
		_currentState.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_currentState.PhysicsUpdate(delta);
	}

	private void TransitionToNewState(string newStateName)
	{
		_currentState?.Exit();

		var newState = _states[newStateName];
		_currentState = newState;
		GD.Print($"Entering {newStateName} state");
		_currentState.Enter();
	}
}
