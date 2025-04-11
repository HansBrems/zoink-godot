using Godot;
using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Player.States;

public partial class FightingState : State
{
	[Export] public PlayerScene Player;

	public override void Enter()
	{
		Player.PlayAnimation("running");
	}

	public override void Update(double delta)
	{
		if (Player.CanShoot && Input.IsActionPressed("shoot"))
			Player.Shoot();

		if (Player.CanDash && Input.IsActionJustPressed("dash"))
			TransitionToState("DashingState");

		if (Player.CanBuild && Input.IsActionJustPressed("build"))
			TransitionToState("BuildingState");

		var inputVector = Player.GetInputVector();
		if (inputVector == Vector2.Zero)
			TransitionToState("IdleState");

		Player.Move(inputVector);
	}
}
