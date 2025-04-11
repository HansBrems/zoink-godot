using System.Linq;
using Godot;
using Godot.Collections;
using Zoink.scenes.Core;
using Zoink.scenes.Core.Projectiles;
using Zoink.Common.Interactions.InteractionManager;

namespace Zoink.scenes.Player;

public partial class PlayerScene : CharacterBody2D
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

	private PlayerCamScene _camera;
	private Array<Marker2D> _bulletSpawnLocations;
	private HurtboxScene _hurtbox;
	private InteractionManagerScene _interactionManager;
	private ProgressBar _buildingProgressBar;
	private AnimatedSprite2D _playerSprite;
	private Timer _shootCooldownTimer;

	private Vector2 _buildingPosition;
	private int _turretCount = 2;

	public HealthScene Health;

	private Label _healthLabel;

	public bool CanBuild => _turretCount > 0;
	public bool CanDash { get; set; } = true;
	public bool CanShoot { get; private set; } = true;

	public override void _Ready()
	{
		_camera = GetNode<PlayerCamScene>("PlayerCam");
		_buildingProgressBar = GetNode<ProgressBar>("InteractionProgress");
		_bulletSpawnLocations = new Array<Marker2D>(GetNode("PlayerSprite/BulletSpawnLocations").GetChildren().Cast<Marker2D>());
		Health = GetNode<HealthScene>("Health");

		_hurtbox = GetNode<HurtboxScene>("Hurtbox");
		_hurtbox.OnHurt += (damage) => Health.CurrentHealth -= damage;
		_interactionManager = GetNode<InteractionManagerScene>("/root/InteractionManager");
		_interactionManager.RegisterPlayer(this);
		_playerSprite = GetNode<AnimatedSprite2D>("PlayerSprite");
		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.WaitTime = AttackSpeed;
		_shootCooldownTimer.Timeout += () => CanShoot = true;
	}

	public Vector2 GetInputVector()
	{
		return Input.GetVector("left", "right", "up", "down");
	}

	public void Hurt(int damage)
	{
		Health.CurrentHealth -= damage;
	}

	public void Move(Vector2 direction)
	{
		if (_playerSprite.FlipH && direction.X > 0) _playerSprite.FlipH = false;
		if (!_playerSprite.FlipH && direction.X < 0) _playerSprite.FlipH = true;

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
		EmitSignal(SignalName.OnBuildingStarted);
	}

	public void CancelBuilding()
	{
		EmitSignal(SignalName.OnBuildingCancelled);
	}

	public void ConfirmBuilding()
	{
		EmitSignal(SignalName.OnBuildingConfirmed);
	}

	public void FinishBuilding()
	{
		EmitSignal(SignalName.OnBuildingFinished, _buildingPosition);
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
		_playerSprite?.Play(name);
	}

	public void Shoot()
	{
		var direction = GetGlobalMousePosition() - Position;
		var spawnLocation = _bulletSpawnLocations.PickRandom();

		EmitSignal(SignalName.OnShoot, new OnShootEventArgs
		{
			Direction = direction.Normalized(),
			Position = spawnLocation.GlobalPosition,
			ProjectileType = ProjectileType.Bullet,
		});

		CanShoot = false;
		_shootCooldownTimer.Start();
	}
}
