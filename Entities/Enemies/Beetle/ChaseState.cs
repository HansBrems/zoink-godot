using Godot;

using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Enemies.Beetle;

public partial class ChaseState : State
{
	[Export]
	public BeetleScene BeetleScene;

	public override void Enter()
	{
		BeetleScene.PlayAnimation(Animations.Walking);
		GetTree().CreateTimer(5).Timeout += () => TransitionToState(States.IdleState);
	}

	public override void Update(double delta)
	{
		BeetleScene.Chase(delta);
	}
}
