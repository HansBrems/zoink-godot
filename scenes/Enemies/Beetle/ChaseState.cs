using Godot;

namespace Zoink.scenes.Enemies.Beetle;

public partial class ChaseState : State
{
	[Export] public global::Beetle Beetle;

	public override void Enter()
	{
		Beetle.PlayAnimation(Animations.Walking);
	}

	public override void Update(double delta)
	{
		Beetle.Chase(delta);
	}
}
