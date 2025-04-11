using Godot;

using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Enemies.Beetle;

public partial class ChaseState : State
{
	[Export]
	public Entities.Enemies.Beetle.BeetleScene BeetleScene;

	public override void Enter()
	{
		BeetleScene.PlayAnimation(Animations.Walking);
	}

	public override void Update(double delta)
	{
		BeetleScene.Chase(delta);
	}
}
