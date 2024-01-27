using Godot;

public partial class BuildingState : State
{
	public override void Enter()
	{
		GD.Print("Entering building state");
		Player.StartBuilding();
	}

	public override void Update(double delta)
	{
		if (Input.IsActionJustPressed("build"))
		{
			Player.CancelBuilding();
			TransitionToState("IdleState");
		}

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			TransitionToState("BuildingTurretState");
		}
	}
}
