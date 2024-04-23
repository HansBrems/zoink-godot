using System.Linq;
using Godot;
using Godot.Collections;
using Zoink.scenes.Core;
using Zoink.scenes.Core.Interactions;
using Zoink.scenes.Core.Projectiles;

namespace Zoink.scenes.Player;

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
	private PlayerCam _camera;
	private Array<Marker2D> _bulletSpawnLocations;
	private Hurtbox _hurtbox;
	private InteractionManager _interactionManager;
	private ProgressBar _buildingProgressBar;
	private Sprite2D _playerSprite;
	private Timer _shootCooldownTimer;

	private Vector2 _buildingPosition;
	private int _turretCount = 2;

	private Health _health;

	public bool CanBuild => _turretCount > 0;
	public bool CanDash { get; set; } = true;
	public bool CanShoot { get; private set; } = true;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_camera = GetNode<PlayerCam>("PlayerCam");
		_buildingProgressBar = GetNode<ProgressBar>("InteractionProgress");
		_bulletSpawnLocations = new Array<Marker2D>(GetNode("PlayerSprite/BulletSpawnLocations").GetChildren().Cast<Marker2D>());
		_health = GetNode<Health>("Health");
		_health.OnNoHealth += () => GD.Print("Dead");
		_hurtbox = GetNode<Hurtbox>("Hurtbox");
		_hurtbox.OnHurt += (damage) => _health.CurrentHealth -= damage;
		_interactionManager = GetNode<InteractionManager>("/root/InteractionManager");
		_interactionManager.RegisterPlayer(this);
		_playerSprite = GetNode<Sprite2D>("PlayerSprite");
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
		_playerSprite.LookAt(GetGlobalMousePosition());
		_playerSprite.Rotation += Mathf.DegToRad(90);
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
