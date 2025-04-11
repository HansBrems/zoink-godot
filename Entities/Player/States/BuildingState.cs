using Godot;
using Zoink.scenes.Core.States;

namespace Zoink.scenes.Player;

public partial class BuildingState : State
{
	[Export] public PlayerScene Player;

	public override void Enter()
	{
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
