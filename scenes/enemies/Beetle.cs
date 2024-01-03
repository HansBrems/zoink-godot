using System;
using Godot;

public partial class Beetle : CharacterBody2D
{
	private const int Speed = 1000;
	private NavigationAgent2D _navigationAgent;
	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioStreamPlayer;
	private readonly Random _random = new Random();
	private int _health = 100;

	private TextureProgressBar _healthBar;

	[Export]
	public Player Player { get; set; }

	public override void _Ready()
	{
		_healthBar = GetNode<TextureProgressBar>("HealthBar");
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	public override void _Process(double delta)
	{
		_healthBar.Value = 100;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_navigationAgent.DistanceToTarget() < 1000f)
		{
			Move(delta);
			// LookAt(Player.GlobalPosition);
		}
		else
		{
			Idle();
		}
	}

	private void Idle()
	{
		_animationPlayer.Play("Idle");
	}

	private void Move(double delta)
	{
		var direction = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		_animationPlayer.Play("Walking");
		Velocity = direction * (float)(Speed * delta);
		MoveAndSlide();
	}

	private void PlayFootstepAudio()
	{
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		_audioStreamPlayer.PitchScale = (float)pitch;
		_audioStreamPlayer.Play();
	}

	private void ReconsiderAction()
	{
		_navigationAgent.TargetPosition = Player.GlobalPosition;
	}
}
