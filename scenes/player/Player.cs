using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 _direction = new (0, 0);

	private bool _canShoot = true;
	private Timer _shootCooldownTimer;
	private AudioStreamPlayer2D _shootAudio;

	private bool _canDash = true;
	private bool _isDashing = false;
	private Timer _dashCooldownTimer;
	private Timer _dashTimer;

	private AnimationPlayer _animationPlayer;

	[Signal]
	public delegate void OnShootEventHandler(Vector2 direction);

	[Export]
	public int Speed { get; set; } = 50;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_shootAudio = GetNode<AudioStreamPlayer2D>("ShootAudio");

		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
		_dashCooldownTimer.Timeout += EnableDashing;

		_dashTimer = GetNode<Timer>("DashTimer");
		_dashTimer.Timeout += StopDashing;

		_shootCooldownTimer = GetNode<Timer>("ShootCooldownTimer");
		_shootCooldownTimer.Timeout += EnableShooting;
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

	private void Shoot()
	{
		var direction = GetGlobalMousePosition() - Position;
		EmitSignal("OnShoot", direction.Normalized());
		_shootAudio.Play();
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
