using System;
using Godot;

public partial class Beetle : CharacterBody2D
{
	private Vector2 _direction = Vector2.Zero;
	private int _health = 50;
	private readonly Random _random = new();

	private AnimationPlayer _animationPlayer;
	private Area2D _area2D;
	private AudioStreamPlayer2D _audioStreamPlayer;
	private TextureProgressBar _healthBar;
	private NavigationAgent2D _navigationAgent;
	private Timer _navigationTimer;
	private Sprite2D _sprite;

	private Timer _shootCooldownTimer;

	private Marker2D _bulletStartingPosition;

	[Signal]
	public delegate void OnShootEventHandler(OnShootEventArgs args);

	[Signal]
	public delegate void OnKilledEventHandler();

	[Export]
	public int Speed = 2000;

	[Export]
	public Node2D Target;

	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_area2D = GetNode<Area2D>("Area2D");
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent");
		_navigationTimer = GetNode<Timer>("NavigationTimer");
		_healthBar = GetNode<TextureProgressBar>("HealthBar");
		_sprite = GetNode<Sprite2D>("Sprite2D");

		_area2D.AreaEntered += OnAreaEntered;
		_navigationTimer.Timeout += UpdateNavigationTargetPosition;

		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.Timeout += Shoot;
		_bulletStartingPosition = GetNode<Marker2D>("Sprite2D/BulletStartingPosition");

		_health = GetNode<Global>("/root/Global").MaxHealth;
		_healthBar.MaxValue = _health;
		GD.Print(_health);
	}

	private void Shoot()
	{
		var direction = Target.GlobalPosition - Position;
		EmitSignal("OnShoot", new OnShootEventArgs
		{
			Direction = direction.Normalized(),
			Position = _bulletStartingPosition.GlobalPosition,
			ProjectileType = ProjectileType.Bullet
		});
	}

	private void OnAreaEntered(Area2D area)
	{
		_health -= 30;

		if (_health <= 0)
		{
			EmitSignal("OnKilled");
			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var nextPathPosition = ToLocal(_navigationAgent.GetNextPathPosition());
		_direction = nextPathPosition.Normalized();

		var rotation = (float)(_direction.Angle() * 180 / Math.PI) + 90;
		var tween = GetTree().CreateTween();
		tween.TweenProperty(_sprite, "rotation_degrees", rotation, 0.2f);

		if (_direction == Vector2.Zero)
			Idle();
		else
			Move(delta);

		_healthBar.Value = _health;
	}

	private void Idle()
	{
		_animationPlayer.Play("Idle");
	}

	private void Move(double delta)
	{
		_animationPlayer.Play("Walking");
		Velocity = _direction * (float)(Speed * delta);
		MoveAndSlide();
	}

	private void PlayFootstepAudio()
	{
		SetRandomPitch(_audioStreamPlayer);
		_audioStreamPlayer.Play();
	}

	private void SetRandomPitch(AudioStreamPlayer2D streamPlayer)
	{
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		streamPlayer.PitchScale = (float)pitch;
	}

	private void UpdateNavigationTargetPosition()
		=> _navigationAgent.TargetPosition = Target.GlobalPosition;
}
