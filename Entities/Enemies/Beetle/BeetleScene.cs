using System;
using Godot;
using Zoink.Entities.Player;

namespace Zoink.Entities.Enemies.Beetle;

public partial class BeetleScene : CharacterBody2D
{
	private PackedScene _bloodSplatterScene;

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

	[Export] public int Speed = 1500;
	[Export] public Node2D Target;
	[Signal] public delegate void OnKilledEventHandler();

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_area2D = GetNode<Area2D>("Area2D");
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_healthBar = GetNode<TextureProgressBar>("HealthBar");
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent");
		_navigationTimer = GetNode<Timer>("NavigationTimer");
		_sprite = GetNode<Sprite2D>("Sprite2D");

		_bloodSplatterScene = scripts.PackedSceneLoader.Load("Effects", "BloodSplatter");

		_area2D.AreaEntered += TakeHit;
		_navigationTimer.Timeout += UpdateNavigationTargetPosition;

		SetRandomStrength();
		TargetPlayer();
		PlayAnimation(Animations.Idle);
	}

	public override void _PhysicsProcess(double delta)
	{
		var rotation = (float)(_direction.Angle() * 180 / Math.PI) + 90;
		var tween = GetTree().CreateTween();
		tween.TweenProperty(_sprite, "rotation_degrees", rotation, 0.2f);

		_healthBar.Value = _health;
	}

	public void Chase(double delta)
	{
		var nextPathPosition = ToLocal(_navigationAgent.GetNextPathPosition());
		_direction = nextPathPosition.Normalized();
		Velocity = _direction * (float)(Speed * delta);
		MoveAndSlide();
	}

	public void PlayAnimation(string name)
	{
		_animationPlayer?.Play(name);
	}

	private void PlayFootstepAudio()
	{
		SetRandomPitch(_audioStreamPlayer);
		_audioStreamPlayer.Play();
	}

	private void SetRandomStrength()
	{
		var random = new Random();

		// Set health.
		_health = random.Next(50, 1000);
		_healthBar.MaxValue = _health;

		// Scale relative to health.
		var scaleValue = 1 + (float)((_health - 50) * (2f - 1) / (1000 - 50));
		_sprite.Scale = new Vector2(scaleValue, scaleValue);

		// Speed relative to health.
		Speed = 4000 + ((_health - 50) * (500 - 4000) / (1000 - 50));
	}

	private void TargetPlayer()
	{
		Target = GetTree().GetNodesInGroup("Player")[0] as PlayerScene;
	}

	private void TakeHit(Area2D area)
	{
		_health -= 30;

		if (_health <= 0)
		{
			EmitSignal(SignalName.OnKilled);

			var splatter = _bloodSplatterScene.Instantiate<Node2D>();
			GetTree().CurrentScene.AddChild(splatter);
			splatter.GlobalPosition = GlobalPosition;
			QueueFree();
		}
	}

	private void SetRandomPitch(AudioStreamPlayer2D streamPlayer)
	{
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		streamPlayer.PitchScale = (float)pitch;
	}

	private void UpdateNavigationTargetPosition()
		=> _navigationAgent.TargetPosition = Target.GlobalPosition;
}
