using System;
using Godot;

public partial class Beetle : CharacterBody2D
{
	private const int Speed = 1000;

	private Vector2 _direction = Vector2.Zero;
	private int _health = 100;

	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioStreamPlayer;
	private Follower _follower;
	private TextureProgressBar _healthBar;
	private readonly Random _random = new Random();

	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_follower = GetNode<Follower>("Follower");
		_healthBar = GetNode<TextureProgressBar>("HealthBar");

		_healthBar.Value = 100;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_direction == Vector2.Zero)
		{
			Idle();
		}
		else
		{
			Move(delta);
		}
	}

	public void Target(Node2D node)
	{
		_follower.Target = node;
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

	private void OnDirectionChanged(Vector2 direction)
	{
		_direction = direction;
	}

	private void PlayFootstepAudio()
	{
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		_audioStreamPlayer.PitchScale = (float)pitch;
		_audioStreamPlayer.Play();
	}
}
