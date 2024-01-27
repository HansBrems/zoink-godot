using Godot;

public partial class FightingState : State
{
	public override void Enter()
	{
		GD.Print("Entering Fighting state");
		Player.PlayAnimation("Walking");
	}

	public override void Update(double delta)
	{
		if (Input.IsActionJustPressed("shoot"))
			Player.Shoot();

		if (Player.CanDash && Input.IsActionJustPressed("dash"))
			TransitionToState("DashingState");

		if (Player.CanBuild && Input.IsActionJustPressed("build"))
			TransitionToState("BuildingState");

		var inputVector = Player.GetInputVector();
		if (inputVector == Vector2.Zero)
			TransitionToState("IdleState");

		Player.LookAtMousePosition();
		Player.Move(inputVector);
	}
}
