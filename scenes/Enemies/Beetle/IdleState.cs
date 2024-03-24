using Godot;

namespace Zoink.scenes.Enemies.Beetle;

public partial class IdleState : State
{
	[Export] public global::Beetle Beetle;

	public override void Enter()
	{
		Beetle.PlayAnimation(Animations.Idle);
		GetTree().CreateTimer(5).Timeout += () => TransitionToState(States.ChaseState);
	}

	public override void Update(double delta)
	{
	}
}