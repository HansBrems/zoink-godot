using System;
using Godot;

public partial class Beetle : CharacterBody2D
{
	private Vector2 _direction = Vector2.Zero;
	private int _health = 100;
	private readonly Random _random = new();

	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioStreamPlayer;
	private TextureProgressBar _healthBar;
	private NavigationAgent2D _navigationAgent;
	private Timer _navigationTimer;
	private Sprite2D _sprite;

	[Export]
	public int Speed = 1000;

	[Export]
	public Node2D Target;

	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent");
		_navigationTimer = GetNode<Timer>("NavigationTimer");
		_healthBar = GetNode<TextureProgressBar>("HealthBar");
		_sprite = GetNode<Sprite2D>("Sprite2D");

		_healthBar.Value = 100;
		_navigationTimer.Timeout += UpdateNavigationTargetPosition;
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
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		_audioStreamPlayer.PitchScale = (float)pitch;
		_audioStreamPlayer.Play();
	}

	private void UpdateNavigationTargetPosition()
		=> _navigationAgent.TargetPosition = Target.GlobalPosition;
}
