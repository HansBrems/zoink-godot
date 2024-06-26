using Godot;

namespace Zoink.scenes.Core.States;

public partial class State : Node
{
	[Signal]
	public delegate void OnTransitionEventHandler(string newStateName);

	public virtual void Enter() {}
	public virtual void Exit() {}

	public virtual void Update(double delta) {}
	public virtual void PhysicsUpdate(double delta) {}

	protected void TransitionToState(string newStateName)
	{
		EmitSignal(SignalName.OnTransition, newStateName);
	}
}
