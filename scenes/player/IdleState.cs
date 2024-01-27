using Godot;

public partial class IdleState : State
{
	[Export] public Player Player;

	public override void Enter()
	{
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
