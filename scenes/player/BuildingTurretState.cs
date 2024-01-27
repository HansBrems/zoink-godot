using Godot;

public partial class BuildingTurretState : State
{
	private Timer _buildTimer;

	public override void _Ready()
	{
		_buildTimer = GetNode<Timer>("BuildTimer");
		_buildTimer.Timeout += BuildTurret;
	}

	public override void Enter()
	{
		GD.Print("Entering building turret state");
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
		GD.Print("Turret Placed");
		TransitionToState("IdleState");
	}
}
