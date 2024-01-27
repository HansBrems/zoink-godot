using Godot;

public partial class IdleState : State
{
	public override void Enter()
	{
		GD.Print("Entering Idle state");
		if (Player != null)
			Player.PlayAnimation("Idle");
	}

	public override void Update(double delta)
	{
		if (Input.IsActionJustPressed("shoot"))
			Player.Shoot();

		if (Player.CanBuild && Input.IsActionJustPressed("build"))
			TransitionToState("BuildingState");

		var inputVector = Player.GetInputVector();
		if (inputVector != Vector2.Zero)
			EmitSignal("OnTransition", "FightingState");

		Player.LookAtMousePosition();
	}
}
