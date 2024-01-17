using System;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	private PackedScene _bulletScene;

	private int _bitcoins = 0;
	private Vector2 _direction = new (0, 0);

	private bool _canShoot = true;
	private Timer _shootCooldownTimer;

	private bool _canDash = true;
	private bool _isDashing = false;
	private Timer _dashCooldownTimer;
	private Timer _dashTimer;

	private Array<Marker2D> _spawnLocations;
	private AnimationPlayer _animationPlayer;

	[Signal]
	public delegate void OnShootEventHandler(Vector2 position, Vector2 direction);

	[Signal]
	public delegate void OnBitcoinsReceivedEventHandler(int bitcoins);

	[Export]
	public double AttackSpeed = 0.15;

	[Export]
	public int Speed { get; set; } = 50;

	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Bullet.tscn");

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
		_dashCooldownTimer.Timeout += EnableDashing;

		_dashTimer = GetNode<Timer>("DashTimer");
		_dashTimer.Timeout += StopDashing;

		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.WaitTime = AttackSpeed;
		_shootCooldownTimer.Timeout += EnableShooting;

		_spawnLocations = new Array<Marker2D>(
			GetNode("BulletSpawnLocations").GetChildren().Cast<Marker2D>());
	}

	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());

		Move();
		if (_canShoot && Input.IsActionPressed("shoot")) Shoot();
		if (_canDash && Input.IsActionPressed("dash")) Dash();
	}

	public void ReceiveBitcoin()
	{
		_bitcoins += 1;
		EmitSignal("OnBitcoinsReceived", _bitcoins);
	}

	public void IncreaseAttackSpeed()
	{
		var attackSpeed = Math.Clamp(_shootCooldownTimer.WaitTime - 0.01, 0.05, 5);
		_shootCooldownTimer.WaitTime = attackSpeed;
		GD.Print("Increasing attack speed: " + attackSpeed);
	}

	private void Dash()
	{
		_canDash = false;
		_isDashing = true;
		Speed = 200;
		_dashTimer.Start();
	}

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


	private void OnPlayerShoot(Vector2 position, Vector2 direction)
	{
		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.Position = position;
		bullet.Direction = direction;
		bullet.RotationDegrees = (float)(direction.Angle() * 180 / Math.PI);

		GetTree().CurrentScene.AddChild(bullet);
	}

	private void Shoot()
	{
		//var spawnIndex = _random.Next(0, _spawnLocations.Length);
		var spawnLocation = _spawnLocations.PickRandom();//   _spawnLocations[spawnIndex];

		var direction = GetGlobalMousePosition() - Position;
		// EmitSignal("OnShoot", spawnLocation.GlobalPosition, direction.Normalized());
		OnPlayerShoot(spawnLocation.GlobalPosition, direction.Normalized());
		_canShoot= false;
		_shootCooldownTimer.Start();
	}

	private void EnableDashing() => _canDash = true;
	private void EnableShooting() => _canShoot = true;

	private void StopDashing()
	{
		Speed = 50;
		_isDashing = false;
		_dashCooldownTimer.Start();
	}
}
