using Godot;

namespace Zoink.scenes.Enemies.Beetle;

public partial class ChaseState : Components.StateMachine.State
{
	[Export] public global::Zoink.scenes.Enemies.Beetle.Beetle Beetle;

	public override void Enter()
	{
		Beetle.PlayAnimation(Animations.Walking);
	}

	public override void Update(double delta)
	{
		Beetle.Chase(delta);
	}
}
