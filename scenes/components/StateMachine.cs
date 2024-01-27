using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
	private readonly Dictionary<string, State> _states = new();
	private State _currentState;

	[Export]
	public State InitialState { get; set; }

	[Export]
	public Player Player { get; set; }

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
			_currentState.Player = Player;
			InitialState.Enter();
		}
	}

	public override void _Process(double delta)
	{
		if (Player != null)
			_currentState.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Player != null)
			_currentState?.PhysicsUpdate(delta);
	}

	private void TransitionToNewState(string newStateName)
	{
		_currentState?.Exit();
		var newState = _states[newStateName];
		newState.Player = Player;
		_currentState = newState;
		_currentState.Enter();
	}
}
