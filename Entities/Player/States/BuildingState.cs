using Godot;
using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Player.States;

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
