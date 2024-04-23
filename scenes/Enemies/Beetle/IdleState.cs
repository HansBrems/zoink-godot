using Godot;
using Zoink.scenes.Core.States;

namespace Zoink.scenes.Enemies.Beetle;

public partial class IdleState : State
{
	[Export]
	public Beetle Beetle;

	public override void Enter()
	{
		Beetle.PlayAnimation(Animations.Idle);
		GetTree().CreateTimer(5).Timeout += () => TransitionToState(States.ChaseState);
	}

	public override void Update(double delta)
	{
	}
}
