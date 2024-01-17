using System;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	private AnimationPlayer _animationPlayer;

	private int _bitcoins = 0;
	private Vector2 _direction = new (0, 0);

	private bool _canDash = true;
	private bool _isDashing = false;
	private Timer _dashCooldownTimer;
	private Timer _dashTimer;

	private bool _canShoot = true;
	private Timer _shootCooldownTimer;
	private Array<Marker2D> _bulletSpawnLocations;

	[Signal]
	public delegate void OnShootEventHandler(OnShootEventArgs args);

	[Signal]
	public delegate void OnBitcoinsReceivedEventHandler(int bitcoins);

	[Export]
	public double AttackSpeed = 0.15;

	[Export]
	public int Speed { get; set; } = 50;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
		_dashCooldownTimer.Timeout += EnableDashing;
		_dashTimer = GetNode<Timer>("DashTimer");
		_dashTimer.Timeout += StopDashing;

		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.WaitTime = AttackSpeed;
		_shootCooldownTimer.Timeout += EnableShooting;

		_bulletSpawnLocations = new Array<Marker2D>(
			GetNode("BulletSpawnLocations").GetChildren().Cast<Marker2D>());
	}

	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());

		Move();
		if (_canShoot && Input.IsActionPressed("shoot")) Shoot();
		if (_canDash && Input.IsActionPressed("dash")) Dash();
	}

	private void Dash()
	{
		_canDash = false;
		_isDashing = true;
		Speed = 200;
		_dashTimer.Start();
	}

	private void EnableDashing() => _canDash = true;

	private void EnableShooting() => _canShoot = true;

	private void Move()
	{
		_direction = _isDashing
			? _direction
			: Input.GetVector("left", "right", "up", "down");
		Velocity = _direction * Speed;
		MoveAndSlide();

		var animation = _direction == Vector2.Zero ? "Idle" : "Walking";
		_animationPlayer.Play(animation);
	}

	public void ReceiveBitcoin()
	{
		_bitcoins += 1;
		EmitSignal("OnBitcoinsReceived", _bitcoins);
	}

	private void Shoot()
	{
		var direction = GetGlobalMousePosition() - Position;
		var spawnLocation = _bulletSpawnLocations.PickRandom();

		EmitSignal("OnShoot", new OnShootEventArgs
		{
			Direction = direction.Normalized(),
			Position = spawnLocation.GlobalPosition,
			ProjectileType = ProjectileType.Bullet,
		});

		_canShoot= false;
		_shootCooldownTimer.Start();
	}

	private void StopDashing()
	{
		Speed = 50;
		_isDashing = false;
		_dashCooldownTimer.Start();
	}
}
