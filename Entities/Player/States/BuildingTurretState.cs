using Godot;
using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Player.States;

public partial class BuildingTurretState : State
{
	[Export] public PlayerScene Player;

	private Timer _buildTimer;

	public override void _Ready()
	{
		_buildTimer = GetNode<Timer>("BuildTimer");
		_buildTimer.Timeout += BuildTurret;
	}

	public override void Enter()
	{
		Player.CaptureMousePosition();
		Player.ConfirmBuilding();
		Player.ShowBuildingProgress(true);
		_buildTimer.Start();
	}

	public override void Exit()
	{
		Player.ShowBuildingProgress(false);
	}

	public override void Update(double delta)
	{
		var totalTime = _buildTimer.WaitTime;
		var elapsedTime = totalTime - _buildTimer.TimeLeft;
		var progress = (elapsedTime / totalTime) * 100;
		Player.UpdateBuildingProgress(progress);
	}

	private void BuildTurret()
	{
		Player.FinishBuilding();
		TransitionToState("IdleState");
	}
}
