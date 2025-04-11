using Godot;
using Zoink.scenes.Core.States;

namespace Zoink.Entities.Enemies.Beetle;

public partial class IdleState : State
{
	[Export]
	public Entities.Enemies.Beetle.BeetleScene BeetleScene;

	public override void Enter()
	{
		BeetleScene.PlayAnimation(Animations.Idle);
		GetTree().CreateTimer(5).Timeout += () => TransitionToState(States.ChaseState);
	}

	public override void Update(double delta)
	{
	}
}
