using System.Linq;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void OnBitcoinsReceivedEventHandler(int bitcoins);

	[Signal]
	public delegate void OnShootEventHandler(OnShootEventArgs args);

	[Signal]
	public delegate void OnBuildingStartedEventHandler();

	[Signal]
	public delegate void OnBuildingCancelledEventHandler();

	[Signal]
	public delegate void OnBuildingConfirmedEventHandler();

	[Signal]
	public delegate void OnBuildingFinishedEventHandler(Vector2 buildingPosition);

	[Export]
	public double AttackSpeed { get; set; } = 0.15;

	[Export]
	public int Speed { get; set; } = 50;

	private AnimationPlayer _animationPlayer;
	private Array<Marker2D> _bulletSpawnLocations;
	private ProgressBar _buildingProgressBar;
	private Timer _shootCooldownTimer;

	private Vector2 _buildingPosition;
	private int _turretCount = 2;

	public bool CanBuild => _turretCount > 0;
	public bool CanDash { get; set; } = true;
	public bool CanShoot { get; private set; } = true;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_buildingProgressBar = GetNode<ProgressBar>("InteractionProgress");
		_bulletSpawnLocations = new Array<Marker2D>(GetNode("BulletSpawnLocations").GetChildren().Cast<Marker2D>());
		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.WaitTime = AttackSpeed;
		_shootCooldownTimer.Timeout += () => CanShoot = true;
	}

	public Vector2 GetInputVector()
	{
		return Input.GetVector("left", "right", "up", "down");
	}

	public void LookAtMousePosition()
	{
		LookAt(GetGlobalMousePosition());
	}

	public void Move(Vector2 direction)
	{
		Velocity = direction * Speed;
		MoveAndSlide();
	}

	public void Dash()
	{
		CanDash = false;
		Speed = 200;
	}

	public void EndDash()
	{
		Speed = 50;
	}

	public void StartBuilding()
	{
		EmitSignal("OnBuildingStarted");
	}

	public void CancelBuilding()
	{
		EmitSignal("OnBuildingCancelled");
	}

	public void ConfirmBuilding()
	{
		EmitSignal("OnBuildingConfirmed");
	}

	public void FinishBuilding()
	{
		EmitSignal("OnBuildingFinished", _buildingPosition);
	}

	public void CaptureMousePosition()
	{
		_buildingPosition = GetGlobalMousePosition();
	}

	public void ShowBuildingProgress(bool isVisible)
	{
		_buildingProgressBar.Visible = isVisible;
		_buildingProgressBar.Value = 0;
	}

	public void UpdateBuildingProgress(double progress)
	{
		_buildingProgressBar.Value = progress;
	}

	public void PlayAnimation(string name)
	{
		_animationPlayer?.Play(name);
	}

	public void Shoot()
	{
		var direction = GetGlobalMousePosition() - Position;
		var spawnLocation = _bulletSpawnLocations.PickRandom();

		EmitSignal("OnShoot", new OnShootEventArgs
		{
			Direction = direction.Normalized(),
			Position = spawnLocation.GlobalPosition,
			ProjectileType = ProjectileType.Bullet,
		});

		CanShoot = false;
		_shootCooldownTimer.Start();
	}
}