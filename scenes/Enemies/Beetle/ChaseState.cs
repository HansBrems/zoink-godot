using Godot;
using Zoink.scenes.Core.States;

namespace Zoink.scenes.Enemies.Beetle;

public partial class ChaseState : State
{
	[Export]
	public Beetle Beetle;

	public override void Enter()
	{
		Beetle.PlayAnimation(Animations.Walking);
	}

	public override void Update(double delta)
	{
		Beetle.Chase(delta);
	}
}
