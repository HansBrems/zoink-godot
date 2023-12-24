using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 _direction = new Vector2(0, 0);
	private int _speed = 50;

	private bool _canDash = true;
	private bool _isDashing = false;
	private Timer _dashCooldownTimer;
	private Timer _dashTimer;

	public override void _Ready()
	{
		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
		_dashTimer = GetNode<Timer>("DashTimer");
	}

	public override void _Process(double delta)
	{
		_direction = _isDashing
			? _direction
			: Input.GetVector("left", "right", "up", "down");
		Velocity = _direction * _speed;
		MoveAndSlide();

		if (_canDash && Input.IsActionPressed("dash")) Dash();
	}

	private void Dash()
	{
		_canDash = false;
		_isDashing = true;
		_speed = 150;
		_dashTimer.Start();
	}

	private void EnableDashing()
	{
		_canDash = true;
	}

	private void StopDashing()
	{
		_speed = 50;
		_isDashing = false;
		_dashCooldownTimer.Start();
	}
}
